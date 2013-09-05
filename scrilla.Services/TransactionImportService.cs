using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using scrilla.Data;

namespace scrilla.Services
{
	public interface ITransactionImportService
	{
		void Import(string fileName);
	}

	public class TransactionImportService : ITransactionImportService
	{
		private IAccountService _accountService;
		private ICategoryService _categoryService;
		private ITransactionService _transactionService;
		private IVendorService _vendorService;

		public TransactionImportService(IAccountService accountService, ICategoryService categoryService, ITransactionService transactionService, IVendorService vendorService)
		{
			_accountService = accountService;
			_categoryService = categoryService;
			_transactionService = transactionService;
			_vendorService = vendorService;
		}

		public void Import(string fileName)
		{
			var transactions = ReadTransactions(fileName);

			if (transactions != null)
			{
				WriteTransactions(transactions);
				_accountService.UpdateAccountBalances();
			}
		}

		private CsvConfiguration CreateCsvConfiguration()
		{
			var config = new CsvConfiguration();
			config.RegisterClassMap<ImportRecordMap>();
			return config;
		}

		private IEnumerable<ImportRecord> ReadTransactions(string fileName)
		{
			if (!File.Exists(fileName))
				return null;

			IEnumerable<ImportRecord> transactions = null;
			using (var stream = new StreamReader(fileName))
			{
				var reader = new CsvHelper.CsvReader(stream, CreateCsvConfiguration());
				transactions = reader.GetRecords<ImportRecord>().OrderBy(x => x.Date).ThenByDescending(x => x.Amount).ToList();
			}

			return transactions;
		}

		public void WriteTransactions(IEnumerable<ImportRecord> transactions)
		{
			// add transactions
			var now = DateTime.Now;
			var accountsResult = _accountService.GetAllAccounts();
			var existingAccounts = accountsResult.Result.ToList();
			var accountGroupsResult = _accountService.GetAllAccountGroups();
			var existingAccountGroups = accountGroupsResult.Result;
			var accountNameMapsResult = _accountService.GetAllAccountNameMaps();
			var existingAccountNameMaps = accountNameMapsResult.Result;
			var categoriesResult = _categoryService.GetAllCategories();
			var existingCategories = categoriesResult.Result;
			var vendorsResult = _vendorService.GetAllVendors();
			var existingVendors = vendorsResult.Result.ToList();
			var mappedVendorsResult = _vendorService.GetAllVendorMaps();
			var existingMappedVendors = mappedVendorsResult.Result;
			var transactionsResult = _transactionService.GetTransactions();
			var existingTransactions = transactionsResult.Result.ToList();

			foreach (ImportRecord row in transactions)
			{
				// is amount credit or debit?
				var amount = row.Amount * (row.TransactionType.Equals("debit") ? -1.0M : 1.0M);

				// transaction a transfer?
				var isTransfer = row.Description.StartsWith("Transfer from") || row.Description.StartsWith("Transfer to");

				// find the account in the local cache
				var accountNameMap = existingAccountNameMaps.FirstOrDefault(x => x.Name.Equals(row.AccountName, StringComparison.CurrentCultureIgnoreCase));
				Account account = accountNameMap == null ? null : existingAccounts.SingleOrDefault(x => x.Id == accountNameMap.AccountId);

				// if we didn't find the account, make one
				if (account == null)
				{
					var addAccountResult = _accountService.AddAccount(row.AccountName);
					if (addAccountResult.HasErrors)
						throw new Exception();

					account = addAccountResult.Result;
					var addAccountNameMapResult = _accountService.AddAccountNameMap(account.Id, row.AccountName);
					if (addAccountNameMapResult.HasErrors)
						throw new Exception();

					existingAccounts.Add(account);
				}
				else
				{
					// if the account does exist, has this transaction already been imported?
					var existingTransaction = existingTransactions.FirstOrDefault(x =>
						x.Amount == amount &&
						x.AccountId == account.Id &&
						x.OriginalTimestamp == row.Date &&
						x.Subtransactions.Any(y => y.Notes.Equals(row.OriginalDescription, StringComparison.CurrentCultureIgnoreCase)));

					// TODO if there are two duplicate transactions to import but only one has been imported so far

					// if the transactions has already been imported, continue
					if (existingTransaction != null)
					{
						continue;
					}
				}

				// find the vendor in the local cache
				var vendor = existingVendors.SingleOrDefault(x => x.Name.Equals(row.Description, StringComparison.CurrentCultureIgnoreCase));

				// find the vendor in the description map
				var mappedVendor = existingMappedVendors.Where(x => x.Description.Equals(row.Description, StringComparison.CurrentCultureIgnoreCase));
				if (mappedVendor.Any())
				{
					vendor = existingVendors.FirstOrDefault(x => x.Id == mappedVendor.FirstOrDefault().VendorId);
				}

				// if we didn't find the vendor, make one
				if (vendor == null)
				{
					var addVendorResult = _vendorService.AddVendor(row.Description);
					if (addVendorResult.HasErrors)
						throw new Exception();

					vendor = addVendorResult.Result;
					var addVendorMapResult = _vendorService.AddVendorMap(vendor.Id, row.Description);
					if (addVendorMapResult.HasErrors)
						throw new Exception();

					existingVendors.Add(vendor);
				}

				// add the transaction
				var addTransactionResult = _transactionService.AddTransaction(account.Id, row.Date, amount, row.Description, row.OriginalDescription, false, false, isTransfer, account.DefaultCategoryId, vendor.Id);
				if (addTransactionResult.HasErrors)
					throw new Exception();
			}
		}
	}

	public class ImportRecord
	{
		public DateTime Date { get; set; }

		/// <summary>
		/// Maps to Vendor name
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Maps to Detailed Vendor name
		/// </summary>
		public string OriginalDescription { get; set; }
		public decimal Amount { get; set; }
		public string TransactionType { get; set; }
		
		/// <summary>
		/// Maps to Mint.com's assigned category. Not used in scrilla.
		/// </summary>
		public string Category { get; set; }
		public string AccountName { get; set; }
		public string Labels { get; set; }
		public string Notes { get; set; }
	}

	class ImportRecordMap : CsvClassMap<ImportRecord>
	{
		public override void CreateMap()
		{
			Map(m => m.OriginalDescription).Name("Original Description");
			Map(m => m.TransactionType).Name("Transaction Type");
			Map(m => m.AccountName).Name("Account Name");
		}
	}
}

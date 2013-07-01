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
	public interface ITransactionImporter
	{
		void Import(string fileName);
	}

	public class TransactionImporter : ITransactionImporter
	{
		private IAccountService _accountService;

		private IEnumerable<ImportRecord> _transactions;
		private List<Transaction> _newTransactions = new List<Transaction>();

		public TransactionImporter(IAccountService accountService)
		{
			_accountService = accountService;
		}

		public void Import(string fileName)
		{
			if (!File.Exists(fileName))
				return;

			using (var stream = new StreamReader(fileName))
			{
				var reader = new CsvHelper.CsvReader(stream);
				_transactions = reader.GetRecords<ImportRecord>().OrderBy(x => x.Date).ThenByDescending(x => x.Amount).ToList();
			}

			UpdateTransactions();
			UpdateAccountBalances();
		}

		private void UpdateAccountBalances()
		{
			throw new NotImplementedException();

			//var transactions = _accountService.GetAllTransactions().Result;
			//foreach (var trx in transactions)
			//{
			//	trx.Amount = trx.Subtransactions.Sum(x => x.Amount);
			//}
			//_unitOfWork.Commit();

			//var accounts = _accountService.GetAllAccounts().Result;
			//foreach (var account in accounts)
			//{
			//	account.Balance = account.InitialBalance + account.Transactions.Sum(x => x.Amount);
			//}

			//_unitOfWork.Commit();
		}

		private void UpdateTransactions()
		{
			throw new NotImplementedException();

			//// add transactions
			//var now = DateTime.Now;
			//var accounts = _accountRepository.GetAll().ToList();
			//var vendors = _vendorRepository.GetAll().ToList();
			//var mappedVendors = _importDescriptionVendorMapRepository.GetAll().ToList();
			//var transactions = _transactionRepository.GetAll().ToList();
			//var importedTransactions = new List<Transaction>();
			//var maxTransactionDate = transactions.Any() ? transactions.Max(x => x.OriginalTimestamp) : DateTime.MinValue;

			//foreach (ImportRecord row in _transactions.Where(x => x.Date >= maxTransactionDate).OrderBy(x => x.Date))
			//{
			//	// is amount credit or debit?
			//	var amount = row.Amount * (row.TransactionType.Equals("debit") ? -1.0M : 1.0M);

			//	// has this transaction already been imported?
			//	var existingTransaction = transactions.FirstOrDefault(x => x.Amount == amount && x.OriginalTimestamp == row.Date && x.Subtransactions.Any(y => y.Notes.Equals(row.OriginalDescription, StringComparison.CurrentCultureIgnoreCase)));
			//	if (existingTransaction != null)
			//	{
			//		Console.WriteLine("FOUND {0} {1} : {2}", row.Date.ToShortDateString(), amount, row.OriginalDescription);

			//		importedTransactions.Add(existingTransaction);
			//		continue;
			//	}

			//	Console.WriteLine("create {0} {1} : {2}", row.Date.ToShortDateString(), amount, row.OriginalDescription);

			//	// find the account in the local cache
			//	var account = accounts.SingleOrDefault(x => x.AccountNameMaps.Any(y => y.Name.Equals(row.AccountName, StringComparison.CurrentCultureIgnoreCase)));

			//	// if we didn't find one, make one
			//	if (account == null)
			//	{
			//		account = new Account()
			//		{
			//			Name = row.AccountName,
			//			AccountGroup = accounts.Any() ? accounts.FirstOrDefault().AccountGroup : new AccountGroup() { Name = row.AccountName },
			//			BalanceTimestamp = now
			//		};
			//		account.AccountNameMaps = new List<AccountNameMap>() { new AccountNameMap() { Account = account, Name = row.AccountName } };

			//		_accountRepository.Add(account);
			//		_unitOfWork.Commit();
			//		accounts.Add(account);
			//	}

			//	account.BalanceTimestamp = now;

			//	// find the vendor in the local cache
			//	var vendor = vendors.SingleOrDefault(x => x.Name.Equals(row.Description, StringComparison.CurrentCultureIgnoreCase));

			//	// find the vendor in the description map
			//	var mappedVendor = mappedVendors.Where(x => x.Description.Equals(row.Description, StringComparison.CurrentCultureIgnoreCase));
			//	if (mappedVendor.Any())
			//	{
			//		vendor = mappedVendor.FirstOrDefault().Vendor;
			//	}

			//	// if we didn't find one, make one
			//	if (vendor == null)
			//	{
			//		vendor = new Vendor() { Name = row.Description };
			//		_vendorRepository.Add(vendor);
			//		_unitOfWork.Commit();
			//		vendors.Add(vendor);
			//	}

			//	Transaction transaction = new Transaction()
			//	{
			//		Timestamp = row.Date,
			//		OriginalTimestamp = row.Date,
			//		Amount = amount,
			//		Vendor = vendor,
			//	};

			//	Subtransaction subtransaction = new Subtransaction()
			//	{
			//		Amount = amount,
			//		Memo = row.Description,
			//		Notes = row.OriginalDescription,
			//		CategoryId = account.DefaultCategoryId,
			//		IsTransfer = row.Description.StartsWith("Transfer from") || row.Description.StartsWith("Transfer to")
			//	};

			//	transaction.Subtransactions.Add(subtransaction);
			//	account.Transactions.Add(transaction);
			//	_newTransactions.Add(transaction);
			//	_unitOfWork.Commit();
			//}
		}
	}

	class ImportRecord
	{
		public DateTime Date { get; set; }
		public string Description { get; set; }
		[CsvField(Name = "Original Description")]
		public string OriginalDescription { get; set; }
		public decimal Amount { get; set; }
		[CsvField(Name = "Transaction Type")]
		public string TransactionType { get; set; }
		public string Category { get; set; }
		[CsvField(Name = "Account Name")]
		public string AccountName { get; set; }
		public string Labels { get; set; }
		public string Notes { get; set; }
	}
}

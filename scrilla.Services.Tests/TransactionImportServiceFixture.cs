using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.AutoFixture;

namespace scrilla.Services.Tests
{
	public class TransactionImportServiceFixture : BaseFixture<TransactionImportService>
	{
		[Fact]
		public void WriteTransactions_OneNewTransaction()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var importRecords = new List<ImportRecord>() { row1 };

			// act
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(1, transactionsResult.Result.Count());

			var transaction = transactionsResult.Result.First();
			Assert.Equal(row1.Date, transaction.Timestamp);
			Assert.Equal(row1.Date, transaction.OriginalTimestamp);
			Assert.Equal(-1 * row1.Amount, transaction.Amount); // debit
			Assert.Equal(row1.OriginalDescription, transaction.Subtransactions.First().Notes);
			Assert.Equal(row1.Description, transaction.Subtransactions.First().Memo);

			var accountService = _fixture.Create<IAccountService>();
			var accountResult = accountService.GetAccount(transaction.AccountId);
			Assert.False(accountResult.HasErrors);
			Assert.Equal(row1.AccountName, accountResult.Result.Name);

			var vendorService = _fixture.Create<IVendorService>();
			var vendorResult = vendorService.GetVendor(transaction.VendorId.Value);
			Assert.False(vendorResult.HasErrors);
			Assert.Equal(row1.Description, vendorResult.Result.Name);
		}

		[Fact]
		public void WriteTransactions_NewAccount()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var row2 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 2" };
			var importRecords = new List<ImportRecord>() { row1 };

			// act
			_sut.WriteTransactions(importRecords);

			importRecords.Add(row2);
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(2, transactionsResult.Result.Count());

			var accountService = _fixture.Create<IAccountService>();
			var accountResult = accountService.GetAllAccounts();
			Assert.False(accountResult.HasErrors);
			Assert.Equal(2, accountResult.Result.Count());
		}

		[Fact]
		public void WriteTransactions_SameAccount()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var row2 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 11.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var importRecords = new List<ImportRecord>() { row1, row2 };

			// act
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(2, transactionsResult.Result.Count());

			var accountService = _fixture.Create<IAccountService>();
			var accountResult = accountService.GetAllAccounts();
			Assert.False(accountResult.HasErrors);
			Assert.Equal(1, accountResult.Result.Count());
		}

		[Fact]
		public void WriteTransactions_ExistingAccount()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var row2 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 20.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var importRecords = new List<ImportRecord>() { row1 };

			// act
			_sut.WriteTransactions(importRecords);

			importRecords.Add(row2);
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(2, transactionsResult.Result.Count());

			var accountService = _fixture.Create<IAccountService>();
			var accountResult = accountService.GetAllAccounts();
			Assert.False(accountResult.HasErrors);
			Assert.Equal(1, accountResult.Result.Count());
		}

		[Fact]
		public void WriteTransactions_NewVendor()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var row2 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 2", OriginalDescription = "VENDOR 2", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var importRecords = new List<ImportRecord>() { row1 };

			// act
			_sut.WriteTransactions(importRecords);

			importRecords.Add(row2);
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(2, transactionsResult.Result.Count());

			var vendorService = _fixture.Create<IVendorService>();
			var vendorResult = vendorService.GetAllVendors();
			Assert.False(vendorResult.HasErrors);
			Assert.Equal(2, vendorResult.Result.Count());
		}

		[Fact]
		public void WriteTransactions_SameVendor()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var row2 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 11.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var importRecords = new List<ImportRecord>() { row1, row2 };

			// act
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(2, transactionsResult.Result.Count());

			var vendorService = _fixture.Create<IVendorService>();
			var vendorResult = vendorService.GetAllVendors();
			Assert.False(vendorResult.HasErrors);
			Assert.Equal(1, vendorResult.Result.Count());
		}

		[Fact]
		public void WriteTransactions_ExistingVendor()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var row2 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 20.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var importRecords = new List<ImportRecord>() { row1 };

			// act
			_sut.WriteTransactions(importRecords);

			importRecords.Add(row2);
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(2, transactionsResult.Result.Count());

			var vendorService = _fixture.Create<IVendorService>();
			var vendorResult = vendorService.GetAllVendors();
			Assert.False(vendorResult.HasErrors);
			Assert.Equal(1, vendorResult.Result.Count());
		}

		[Fact]
		public void WriteTransactions_ExistingTransaction()
		{
			// arrange
			var row1 = new ImportRecord() { Date = new DateTime(2012, 1, 1), Description = "Vendor 1", OriginalDescription = "VENDOR 1", Amount = 10.0M, TransactionType = "debit", Category = "Category 1", AccountName = "Account 1" };
			var importRecords = new List<ImportRecord>() { row1 };

			// act
			_sut.WriteTransactions(importRecords);
			_sut.WriteTransactions(importRecords);

			// assert
			var transactionService = _fixture.Create<ITransactionService>();
			var transactionsResult = transactionService.GetTransactions();
			Assert.False(transactionsResult.HasErrors);
			Assert.Equal(1, transactionsResult.Result.Count());

			var accountService = _fixture.Create<IAccountService>();
			var accountResult = accountService.GetAllAccounts();
			Assert.False(accountResult.HasErrors);
			Assert.Equal(1, accountResult.Result.Count());

			var vendorService = _fixture.Create<IVendorService>();
			var vendorResult = vendorService.GetAllVendors();
			Assert.False(vendorResult.HasErrors);
			Assert.Equal(1, vendorResult.Result.Count());
		}
	}
}

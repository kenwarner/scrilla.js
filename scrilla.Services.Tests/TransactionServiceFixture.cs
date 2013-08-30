using Ploeh.AutoFixture;
using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace scrilla.Services.Tests
{
	public class TransactionServiceFixture : BaseFixture<TransactionService>
	{
		private IAccountService _accountService;
		private ICategoryService _categoryService;
		private IVendorService _vendorService;
		private IBillService _billService;

		public TransactionServiceFixture()
			: base()
		{
			_accountService = _fixture.Create<AccountService>();
			_categoryService = _fixture.Create<CategoryService>();
			_vendorService = _fixture.Create<VendorService>();
			_billService = _fixture.Create<BillService>();
		}

		private ServiceResult<Account> AddTestAccount()
		{
			var accountName = "test account";
			var addAccountResult = _accountService.AddAccount(accountName);
			Assert.False(addAccountResult.HasErrors);
			return addAccountResult;
		}

		private ServiceResult<Category> AddTestCategory()
		{
			var categoryName = "test category";
			var addCategoryResult = _categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);
			return addCategoryResult;
		}

		private ServiceResult<Vendor> AddTestVendor()
		{
			var vendorName = "test vendor";
			var addVendorResult = _vendorService.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);
			return addVendorResult;
		}

		private ServiceResult<Bill> AddTestBill()
		{
			var billName = "test bill";
			var billAmount = 10.5M;
			var billStartDate = new DateTime(2000, 1, 1);
			var billEndDate = billStartDate;
			var addBillResult = _billService.AddBill(billName, billAmount, BillFrequency.OneTime, billStartDate, billEndDate);
			Assert.False(addBillResult.HasErrors);
			return addBillResult;
		}

		private ServiceResult<Transaction> AddTestTransaction()
		{
			var timestamp = new DateTime(2000, 1, 14);
			var amount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			var addAccountResult = AddTestAccount();
			var addCategoryResult = AddTestCategory();
			var addVendorResult = AddTestVendor();
			var addBillResult = AddTestBill();

			var billTransactionsResult = _billService.GetBillTransactions(addBillResult.Result.Id);
			Assert.False(billTransactionsResult.HasErrors);
			Assert.Equal(1, billTransactionsResult.Result.Count());
			var billTransaction = billTransactionsResult.Result.First();

			var addTransactionResult = _sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer, addCategoryResult.Result.Id, addVendorResult.Result.Id, billTransaction.Id);
			Assert.False(addTransactionResult.HasErrors);
			return addTransactionResult;
		}

		[Fact]
		public void GetTransaction_ExistingAccount_NoCategory_NoVendor_NoBill()
		{
			// create test transaction
			var timestamp = new DateTime(2000, 1, 14);
			var amount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			var addAccountResult = AddTestAccount();

			var addTransactionResult = _sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);
			Assert.False(addTransactionResult.HasErrors);

			// act
			var result = _sut.GetTransaction(addTransactionResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(addTransactionResult.Result.AccountId, result.Result.AccountId);
			Assert.Equal(addTransactionResult.Result.Amount, result.Result.Amount);
			Assert.Equal(null, result.Result.BillTransactionId);
			Assert.Equal(addTransactionResult.Result.Id, result.Result.Id);
			Assert.Equal(addTransactionResult.Result.IsReconciled, result.Result.IsReconciled);
			Assert.Equal(addTransactionResult.Result.OriginalTimestamp, result.Result.OriginalTimestamp);
			Assert.Equal(addTransactionResult.Result.Timestamp, result.Result.Timestamp);
			Assert.Equal(null, result.Result.VendorId);
			Assert.Equal(1, result.Result.Subtransactions.Count());
			Assert.Equal(null, result.Result.Subtransactions.First().CategoryId);

			// cleanup
			_sut.DeleteTransaction(result.Result.Id);
			_accountService.DeleteAccount(result.Result.AccountId);
		}

		[Fact]
		public void GetTransaction_ExistingAccount_ExistingCategory_ExistingVendor_ExistingBill()
		{
			// create test transaction
			var addTransactionResult = AddTestTransaction();

			// act
			var result = _sut.GetTransaction(addTransactionResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(addTransactionResult.Result.AccountId, result.Result.AccountId);
			Assert.Equal(addTransactionResult.Result.Amount, result.Result.Amount);
			Assert.Equal(addTransactionResult.Result.BillTransactionId, result.Result.BillTransactionId);
			Assert.Equal(addTransactionResult.Result.Id, result.Result.Id);
			Assert.Equal(addTransactionResult.Result.IsReconciled, result.Result.IsReconciled);
			Assert.Equal(addTransactionResult.Result.OriginalTimestamp, result.Result.OriginalTimestamp);
			Assert.Equal(addTransactionResult.Result.Timestamp, result.Result.Timestamp);
			Assert.Equal(addTransactionResult.Result.VendorId, result.Result.VendorId);
			Assert.Equal(1, result.Result.Subtransactions.Count());
			Assert.Equal(addTransactionResult.Result.Subtransactions.First().CategoryId, result.Result.Subtransactions.First().CategoryId);

			// cleanup
			_sut.DeleteTransaction(result.Result.Id);
			var billTransactionResult = _billService.GetBillTransaction(result.Result.BillTransactionId.Value);
			Assert.False(billTransactionResult.HasErrors);
			_billService.DeleteBill(billTransactionResult.Result.BillId);
			_vendorService.DeleteVendor(result.Result.VendorId.Value);
			_categoryService.DeleteCategory(result.Result.Subtransactions.First().CategoryId.Value);
			_accountService.DeleteAccount(result.Result.AccountId);
		}

		[Fact]
		public void GetTransactions_NoFilters_NoTransactions()
		{
			// act
			var result = _sut.GetTransactions();
			Assert.False(result.HasErrors);
			Assert.Equal(0, result.Result.Count());
		}

		[Fact]
		public void GetTransactions_NonExistantAccount_NoTransactions()
		{
			// create test account
			var nonExistantAccountId = -1;

			// act
			var result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(nonExistantAccountId)));
			Assert.True(result.HasErrors);
			Assert.Equal(1, result.ErrorMessages.Count(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetTransactions_ExistingAccount_NoTransactions()
		{
			// create test account
			var addAccountResult = AddTestAccount();

			// act
			var result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(addAccountResult.Result.Id)));
			Assert.False(result.HasErrors);
			Assert.Equal(0, result.Result.Count());
		}

		[Fact]
		public void GetTransactions_ExistingAccount_OneTransaction()
		{
			var timestamp = new DateTime(2000, 1, 14);
			var amount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			// create test account
			var addAccountResult = AddTestAccount();

			// create test transaction
			var addTransactionResult = _sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);

			// act
			var result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(addAccountResult.Result.Id)));
			Assert.False(result.HasErrors);
			Assert.Equal(1, result.Result.Count());
			Assert.Equal(1, result.Result.First().Subtransactions.Count());
		}

		[Fact]
		public void GetTransactions_ExistingAccount_MultipleTransactions()
		{
			var timestamp = new DateTime(2000, 1, 14);
			var amount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			// create test account
			var addAccountResult = AddTestAccount();

			// create test transactions
			_sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);
			_sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);

			// act
			var result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(addAccountResult.Result.Id)));
			Assert.False(result.HasErrors);
			Assert.Equal(2, result.Result.Count());
			Assert.Equal(1, result.Result.ElementAt(0).Subtransactions.Count());
			Assert.Equal(1, result.Result.ElementAt(1).Subtransactions.Count());
		}

		[Fact]
		public void GetTransactions_MultipleAccounts_OneTransactionEach()
		{
			var timestamp = new DateTime(2000, 1, 14);
			var amount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			// create test account
			var addAccountResult = AddTestAccount();
			var addAccountResult2 = AddTestAccount();

			// create test transactions
			_sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);
			_sut.AddTransaction(addAccountResult2.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);

			// act
			var result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(addAccountResult.Result.Id)));
			Assert.False(result.HasErrors);
			Assert.Equal(1, result.Result.Count());
			Assert.Equal(1, result.Result.ElementAt(0).Subtransactions.Count());
		}

		[Fact]
		public void GetTransactions_MultipleAccounts_OneTransactionEach_WithinFromToDates()
		{
			var timestamp = new DateTime(2000, 1, 14);
			var amount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			// create test account
			var addAccountResult = AddTestAccount();
			var addAccountResult2 = AddTestAccount();

			// create test transactions
			_sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);
			_sut.AddTransaction(addAccountResult2.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);

			// act
			var result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(addAccountResult.Result.Id)), from: timestamp.AddDays(-1), to: timestamp.AddDays(1));
			Assert.False(result.HasErrors);
			Assert.Equal(1, result.Result.Count());
			Assert.Equal(1, result.Result.ElementAt(0).Subtransactions.Count());
		}

		[Fact]
		public void GetTransactions_MultipleAccounts_OneTransactionEach_OutsideFromToDates()
		{
			var timestamp = new DateTime(2000, 1, 14);
			var amount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			// create test account
			var addAccountResult = AddTestAccount();
			var addAccountResult2 = AddTestAccount();

			// create test transactions
			_sut.AddTransaction(addAccountResult.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);
			_sut.AddTransaction(addAccountResult2.Result.Id, timestamp, amount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);

			// act
			var result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(addAccountResult.Result.Id)), from: timestamp.AddDays(-2), to: timestamp.AddDays(-1));
			Assert.False(result.HasErrors);
			Assert.Equal(0, result.Result.Count());

			result = _sut.GetTransactions(new Filter<int?>(new Nullable<int>(addAccountResult.Result.Id)), from: timestamp.AddDays(1), to: timestamp.AddDays(2));
			Assert.False(result.HasErrors);
			Assert.Equal(0, result.Result.Count());
		}


		[Fact]
		public void AddTransaction_ExistingAccount_ExistingCategory_ExistingVendor_ExistingBill()
		{
			var transactionTimestamp = new DateTime(2000, 1, 14);
			var transactionAmount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			var addAccountResult = AddTestAccount();
			var addCategoryResult = AddTestCategory();
			var addVendorResult = AddTestVendor();
			var addBillResult = AddTestBill();

			// get test bill transactions
			var billTransactionResult = _billService.GetBillTransactions(addBillResult.Result.Id);
			Assert.False(billTransactionResult.HasErrors);
			var billTransaction = billTransactionResult.Result.First();

			// act
			var result = _sut.AddTransaction(addAccountResult.Result.Id, transactionTimestamp, transactionAmount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer, addCategoryResult.Result.Id, addVendorResult.Result.Id, billTransaction.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(addAccountResult.Result.Id, result.Result.AccountId);
			Assert.Equal(transactionAmount, result.Result.Amount);
			Assert.Equal(billTransaction.Id, result.Result.BillTransactionId);
			Assert.Equal(isReconciled, result.Result.IsReconciled);
			Assert.Equal(transactionTimestamp, result.Result.OriginalTimestamp);
			Assert.Equal(transactionTimestamp, result.Result.Timestamp);
			Assert.Equal(addVendorResult.Result.Id, result.Result.VendorId);
			Assert.Equal(1, result.Result.Subtransactions.Count());
			Assert.Equal(addCategoryResult.Result.Id, result.Result.Subtransactions.First().CategoryId);
			Assert.Equal(result.Result.Id, result.Result.Subtransactions.First().TransactionId);

			// cleanup
			_sut.DeleteTransaction(result.Result.Id);
			_billService.DeleteBill(addBillResult.Result.Id);
			_vendorService.DeleteVendor(addVendorResult.Result.Id);
			_categoryService.DeleteCategory(addCategoryResult.Result.Id);
			_accountService.DeleteAccount(addAccountResult.Result.Id);
		}

		[Fact]
		public void AddTransaction_ExistingAccount_NoCategory_NoVendor_NoBill()
		{
			var transactionTimestamp = new DateTime(2000, 1, 14);
			var transactionAmount = 12.1M;
			var memo = "test memo";
			var notes = "test notes";
			var isReconciled = true;
			var isExcludedFromBudget = false;
			var isTransfer = true;

			var addAccountResult = AddTestAccount();

			// act
			var result = _sut.AddTransaction(addAccountResult.Result.Id, transactionTimestamp, transactionAmount, memo, notes, isReconciled, isExcludedFromBudget, isTransfer);
			Assert.False(result.HasErrors);
			Assert.Equal(addAccountResult.Result.Id, result.Result.AccountId);
			Assert.Equal(transactionAmount, result.Result.Amount);
			Assert.Equal(null, result.Result.BillTransactionId);
			Assert.Equal(isReconciled, result.Result.IsReconciled);
			Assert.Equal(transactionTimestamp, result.Result.OriginalTimestamp);
			Assert.Equal(transactionTimestamp, result.Result.Timestamp);
			Assert.Equal(null, result.Result.VendorId);
			Assert.Equal(1, result.Result.Subtransactions.Count());
			Assert.Equal(null, result.Result.Subtransactions.First().CategoryId);
			Assert.Equal(result.Result.Id, result.Result.Subtransactions.First().TransactionId);

			// cleanup
			_sut.DeleteTransaction(result.Result.Id);
			_accountService.DeleteAccount(addAccountResult.Result.Id);
		}

		[Fact]
		public void DeleteTransaction_ExistingTransaction()
		{
			var addTransactionResult = AddTestTransaction();

			// act
			var result = _sut.DeleteTransaction(addTransactionResult.Result.Id);
			Assert.False(result.HasErrors);

			// cleanup
			var billTransactionResult = _billService.GetBillTransaction(addTransactionResult.Result.BillTransactionId.Value);
			Assert.False(billTransactionResult.HasErrors);
			_billService.DeleteBill(billTransactionResult.Result.BillId);
			_vendorService.DeleteVendor(addTransactionResult.Result.VendorId.Value);
			_categoryService.DeleteCategory(addTransactionResult.Result.Subtransactions.First().CategoryId.Value);
			_accountService.DeleteAccount(addTransactionResult.Result.AccountId);
		}

		[Fact]
		public void DeleteTransaction_NonExistantTransaction()
		{
			var nonExistantTransactionId = -1;

			// act
			var result = _sut.DeleteTransaction(nonExistantTransactionId);
			Assert.True(result.HasErrors);
		}


		[Fact(Skip = "Not yet implemented")]
		public void UpdateTransaction()
		{

		}
	}
}

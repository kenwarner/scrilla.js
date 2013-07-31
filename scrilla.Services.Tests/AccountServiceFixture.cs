using DapperExtensions;
using DapperExtensions.Sql;
using Ploeh.AutoFixture;
using scrilla.Data;
using scrilla.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace scrilla.Services.Tests
{
	#region GetById Tests

	public class GetAccountTests : BaseFixture
	{
		[Fact]
		public void GetAccount_ExistingAccount_WithDefaultCategory_And_AccountGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var accountName = "test account";
			var balance = 1.23M;

			// get a default category
			var categoryName = "test category";
			var categoryResult = sut.AddCategory(categoryName);
			Assert.False(categoryResult.HasErrors);

			// get a default account group
			var accountGroupName = "test account group";
			var accountGroupResult = sut.AddAccountGroup(accountGroupName);
			Assert.False(accountGroupResult.HasErrors);

			// create test account
			var addAccountResult = sut.AddAccount(accountName, balance, categoryResult.Result.Id, accountGroupResult.Result.Id);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(balance, addAccountResult.Result.Balance);
			Assert.Equal(categoryResult.Result.Id, addAccountResult.Result.DefaultCategoryId);
			Assert.Equal(accountGroupResult.Result.Id, addAccountResult.Result.AccountGroupId);

			// act
			var result = sut.GetAccount(addAccountResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(categoryResult.Result.Id, result.Result.DefaultCategoryId);
			Assert.Equal(accountGroupResult.Result.Id, result.Result.AccountGroupId);

			// cleanup
			sut.DeleteAccount(addAccountResult.Result.Id);
			sut.DeleteCategory(categoryResult.Result.Id);
			sut.DeleteAccountGroup(accountGroupResult.Result.Id);
		}

		[Fact]
		public void GetAccount_ExistingAccount_WithNullDefaultCategory_And_NullAccountGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var accountName = "test account";
			var balance = 1.23M;
			int? defaultCategoryId = null;
			int? accountGroupId = null;

			// create test account
			var addAccountResult = sut.AddAccount(accountName, balance, defaultCategoryId, accountGroupId);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(balance, addAccountResult.Result.Balance);
			Assert.Equal(defaultCategoryId, addAccountResult.Result.DefaultCategoryId);
			Assert.Equal(accountGroupId, addAccountResult.Result.AccountGroupId);

			// act
			var result = sut.GetAccount(addAccountResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(defaultCategoryId, result.Result.DefaultCategoryId);
			Assert.Equal(accountGroupId, result.Result.AccountGroupId);

			// cleanup
			sut.DeleteAccount(addAccountResult.Result.Id);
		}

		[Fact]
		public void GetAccount_NonExistantAccount()
		{
			var sut = _fixture.Create<AccountService>();
			var nonExistantAccountId = -1;

			// act
			var result = sut.GetAccount(nonExistantAccountId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

	public class GetAccountGroupTests : BaseFixture
	{
		[Fact]
		public void GetAccountGroup_ExistingAccountGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var accountGroupName = "test account group";
			var displayOrder = 1;
			var isActive = true;

			// create test account group
			var addAccountGroupResult = sut.AddAccountGroup(accountGroupName, displayOrder, isActive);
			Assert.False(addAccountGroupResult.HasErrors);
			Assert.Equal(accountGroupName, addAccountGroupResult.Result.Name);
			Assert.Equal(displayOrder, addAccountGroupResult.Result.DisplayOrder);
			Assert.Equal(isActive, addAccountGroupResult.Result.IsActive);

			// act
			var result = sut.GetAccountGroup(addAccountGroupResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountGroupName, result.Result.Name);
			Assert.Equal(displayOrder, result.Result.DisplayOrder);
			Assert.Equal(isActive, result.Result.IsActive);

			// cleanup
			sut.DeleteAccountGroup(addAccountGroupResult.Result.Id);
		}

		[Fact]
		public void GetAccountGroup_NonExistantAccount()
		{
			var sut = _fixture.Create<AccountService>();
			var nonExistantAccountGroupId = -1;

			// act
			var result = sut.GetAccountGroup(nonExistantAccountGroupId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

	public class GetCategoryTests : BaseFixture
	{
		[Fact]
		public void GetCategory_ExistingCategory_WithCategoryGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var categoryName = "test category";
			var categoryGroupName = "test category group";

			// create test category group
			var addCategoryGroupResult = sut.AddCategoryGroup(categoryGroupName);
			Assert.False(addCategoryGroupResult.HasErrors);
			Assert.Equal(categoryGroupName, addCategoryGroupResult.Result.Name);

			// create test category
			var addCategoryResult = sut.AddCategory(categoryName, addCategoryGroupResult.Result.Id);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);
			Assert.Equal(addCategoryGroupResult.Result.Id, addCategoryResult.Result.CategoryGroupId);

			// act
			var result = sut.GetCategory(addCategoryResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(categoryName, result.Result.Name);
			Assert.Equal(addCategoryGroupResult.Result.Id, result.Result.CategoryGroupId);

			// cleanup
			sut.DeleteCategory(addCategoryResult.Result.Id);
			sut.DeleteCategoryGroup(addCategoryGroupResult.Result.Id);
		}

		[Fact]
		public void GetCategory_ExistingCategory_WithNullCategoryGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var categoryName = "test category";
			int? categoryGroupId = null;

			// create test category
			var addCategoryResult = sut.AddCategory(categoryName, categoryGroupId);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);
			Assert.Equal(categoryGroupId, addCategoryResult.Result.CategoryGroupId);

			// act
			var result = sut.GetCategory(addCategoryResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(categoryName, result.Result.Name);
			Assert.Equal(categoryGroupId, result.Result.CategoryGroupId);

			// cleanup
			sut.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetCategory_NonExistantCategory()
		{
			var sut = _fixture.Create<AccountService>();
			var nonExistantAccountGroupId = -1;

			// act
			var result = sut.GetAccountGroup(nonExistantAccountGroupId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

	public class GetVendorTests : BaseFixture
	{
		[Fact]
		public void GetVendor_ExistingVendor_WithNullDefaultCategoryId()
		{
			var sut = _fixture.Create<AccountService>();
			var vendorName = "test vendor";
			int? defaultCategoryId = null;

			// create test vendor
			var addVendorResult = sut.AddVendor(vendorName, defaultCategoryId);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(vendorName, addVendorResult.Result.Name);
			Assert.Equal(defaultCategoryId, addVendorResult.Result.DefaultCategoryId);

			// act
			var result = sut.GetVendor(addVendorResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(vendorName, result.Result.Name);
			Assert.Equal(defaultCategoryId, result.Result.DefaultCategoryId);

			// cleanup
			sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void GetVendor_ExistingVendor_WithDefaultCategoryId()
		{
			var sut = _fixture.Create<AccountService>();
			var vendorName = "test vendor";
			var categoryName = "test category";

			// create test category
			var addCategoryResult = sut.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);

			// create test vendor
			var addVendorResult = sut.AddVendor(vendorName, addCategoryResult.Result.Id);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(vendorName, addVendorResult.Result.Name);
			Assert.Equal(addCategoryResult.Result.Id, addVendorResult.Result.DefaultCategoryId);

			// act
			var result = sut.GetVendor(addVendorResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(vendorName, result.Result.Name);
			Assert.Equal(addCategoryResult.Result.Id, result.Result.DefaultCategoryId);

			// cleanup
			sut.DeleteVendor(addVendorResult.Result.Id);
			sut.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetVendor_NonExistantVendor()
		{
			var sut = _fixture.Create<AccountService>();
			var nonExistantVendorId = -1;

			// act
			var result = sut.GetVendor(nonExistantVendorId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

	public class GetBillTests : BaseFixture
	{
		[Fact]
		public void GetBill_ExistingBill_WithNullBillGroup_AndNullCategory_AndNullVendor_AndNullSecondaryDates()
		{
			var sut = _fixture.Create<AccountService>();
			var billName = "test bill";
			var amount = 1.23M;
			int? billGroupId = null;
			int? categoryId = null;
			int? vendorId = null;
			DateTime startDate = new DateTime(2013, 7, 30);
			DateTime endDate = new DateTime(2013, 10, 15);
			BillFrequency frequency = BillFrequency.Daily;
			DateTime? secondaryStartDate = null;
			DateTime? secondaryEndDate = null;

			// create test bill
			var addBillResult = sut.AddBill(billName, amount, billGroupId, categoryId, vendorId, startDate, endDate, frequency, secondaryStartDate, secondaryEndDate);
			Assert.False(addBillResult.HasErrors);
			Assert.Equal(billName, addBillResult.Result.Name);
			Assert.Equal(amount, addBillResult.Result.Amount);
			Assert.Equal(billGroupId, addBillResult.Result.BillGroupId);
			Assert.Equal(categoryId, addBillResult.Result.CategoryId);
			Assert.Equal(vendorId, addBillResult.Result.VendorId);
			Assert.Equal(startDate, addBillResult.Result.StartDate);
			Assert.Equal(endDate, addBillResult.Result.EndDate);
			Assert.Equal(frequency, addBillResult.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, addBillResult.Result.StartDate2);
			Assert.Equal(secondaryEndDate, addBillResult.Result.EndDate2);

			// act
			var result = sut.GetBill(addBillResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(billName, result.Result.Name);
			Assert.Equal(amount, result.Result.Amount);
			Assert.Equal(billGroupId, result.Result.BillGroupId);
			Assert.Equal(categoryId, result.Result.CategoryId);
			Assert.Equal(vendorId, result.Result.VendorId);
			Assert.Equal(startDate, result.Result.StartDate);
			Assert.Equal(endDate, result.Result.EndDate);
			Assert.Equal(frequency, result.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, result.Result.StartDate2);
			Assert.Equal(secondaryEndDate, result.Result.EndDate2);

			// cleanup
			sut.DeleteBill(addBillResult.Result.Id);
		}

		[Fact]
		public void GetBill_ExistingBill_WithBillGroup_AndCategory_AndVendor_AndSecondaryDates()
		{
			var sut = _fixture.Create<AccountService>();
			var billName = "test bill";
			var amount = 1.23M;
			DateTime startDate = new DateTime(2013, 7, 2);
			DateTime endDate = new DateTime(2013, 10, 2);
			BillFrequency frequency = BillFrequency.Daily;
			DateTime? secondaryStartDate = new DateTime(2013, 7, 16);
			DateTime? secondaryEndDate = new DateTime(2013, 10, 16);

			// create test bill group
			var billGroupName = "test bill group";
			var addBillGroupResult = sut.AddBillGroup(billGroupName);
			Assert.False(addBillGroupResult.HasErrors);
			Assert.Equal(billGroupName, addBillGroupResult.Result.Name);

			// create test category
			var categoryName = "test category";
			var addCategoryResult = sut.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);

			// create test vendor
			var vendorName = "test vendor";
			var addVendorResult = sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(vendorName, addVendorResult.Result.Name);

			// create test bill
			var addBillResult = sut.AddBill(billName, amount, addBillGroupResult.Result.Id, addCategoryResult.Result.Id, addVendorResult.Result.Id
				, startDate, endDate, frequency, secondaryStartDate, secondaryEndDate);
			Assert.False(addBillResult.HasErrors);
			Assert.Equal(billName, addBillResult.Result.Name);
			Assert.Equal(amount, addBillResult.Result.Amount);
			Assert.Equal(addBillGroupResult.Result.Id, addBillResult.Result.BillGroupId);
			Assert.Equal(addCategoryResult.Result.Id, addBillResult.Result.CategoryId);
			Assert.Equal(addVendorResult.Result.Id, addBillResult.Result.VendorId);
			Assert.Equal(startDate, addBillResult.Result.StartDate);
			Assert.Equal(endDate, addBillResult.Result.EndDate);
			Assert.Equal(frequency, addBillResult.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, addBillResult.Result.StartDate2);
			Assert.Equal(secondaryEndDate, addBillResult.Result.EndDate2);

			// act
			var result = sut.GetBill(addBillResult.Result.Id);
			Assert.False(addBillResult.HasErrors);
			Assert.Equal(billName, result.Result.Name);
			Assert.Equal(billName, result.Result.Name);
			Assert.Equal(amount, result.Result.Amount);
			Assert.Equal(addBillGroupResult.Result.Id, result.Result.BillGroupId);
			Assert.Equal(addCategoryResult.Result.Id, result.Result.CategoryId);
			Assert.Equal(addVendorResult.Result.Id, result.Result.VendorId);
			Assert.Equal(startDate, result.Result.StartDate);
			Assert.Equal(endDate, result.Result.EndDate);
			Assert.Equal(frequency, result.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, result.Result.StartDate2);
			Assert.Equal(secondaryEndDate, result.Result.EndDate2);

			// cleanup
			sut.DeleteBill(addBillResult.Result.Id);
			sut.DeleteBillGroup(addBillGroupResult.Result.Id);
			sut.DeleteCategory(addCategoryResult.Result.Id);
			sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void GetBill_NonExistantBill()
		{
			var sut = _fixture.Create<AccountService>();
			var nonExistantVendorId = -1;

			// act
			var result = sut.GetVendor(nonExistantVendorId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

	public class GetBillGroupTests : BaseFixture
	{
		[Fact]
		public void GetBillGroup_ExistingBillGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var billGroupName = "test bill group";
			var displayOrder = 1;
			var isActive = true;

			// create test bill group
			var addBillGroupResult = sut.AddBillGroup(billGroupName, displayOrder, isActive);
			Assert.False(addBillGroupResult.HasErrors);
			Assert.Equal(billGroupName, addBillGroupResult.Result.Name);
			Assert.Equal(displayOrder, addBillGroupResult.Result.DisplayOrder);
			Assert.Equal(isActive, addBillGroupResult.Result.IsActive);

			// act
			var result = sut.GetBillGroup(addBillGroupResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(billGroupName, result.Result.Name);
			Assert.Equal(displayOrder, result.Result.DisplayOrder);
			Assert.Equal(isActive, result.Result.IsActive);

			// cleanup
			sut.DeleteBillGroup(addBillGroupResult.Result.Id);
		}

		[Fact]
		public void GetBillGroup_NonExistantBillGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var nonExistantBillGroupId = -1;

			// act
			var result = sut.GetBillGroup(nonExistantBillGroupId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

	#endregion

	#region GetAll Tests

	public class GetAllAccountsTests : BaseFixture
	{
		[Fact]
		public void GetAllAccounts()
		{
			var sut = _fixture.Create<AccountService>();

			var accountsResult = sut.GetAllAccounts();
			Assert.False(accountsResult.HasErrors);
			Assert.Empty(accountsResult.Result);

			var name = "test account";
			var balance = 1.23M;
			var addAccountResult = sut.AddAccount(name, balance);
			Assert.False(addAccountResult.HasErrors);

			accountsResult = sut.GetAllAccounts();
			Assert.False(accountsResult.HasErrors);
			Assert.Equal(1, accountsResult.Result.Count());

			// cleanup
			sut.DeleteAccount(addAccountResult.Result.Id);
		}

	}

	public class GetAllCategoriesTests : BaseFixture
	{

	}

	public class GetAllCategoryGroups : BaseFixture
	{

	}

	public class GetAllVendorsTests : BaseFixture
	{

	}

	public class GetAllBillsTests : BaseFixture
	{

	}

	public class GetAllBillGroupsTests : BaseFixture
	{

	}

	public class GetAllTransactionsTests : BaseFixture
	{

	}

	public class GetTransactionsTests : BaseFixture
	{

	}

	public class GetTransactionsByAccountTests : BaseFixture
	{

	}

	public class GetTransactionsByVendorTests : BaseFixture
	{

	}

	public class GetTransactionsByCategoryTests : BaseFixture
	{

	}

	public class GetBudgetCategoriesTests : BaseFixture
	{

	}

	public class GetBillTransactionsTests : BaseFixture
	{ 
	
	}

	#endregion

	#region Add Tests

	public class AddAccountTests : BaseFixture
	{
		[Fact]
		public void AddAccount_NullDefaultCategory_And_NullAccountGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var name = "test account";
			var balance = 1.23M;

			var result = sut.AddAccount(name, balance);

			Assert.False(result.HasErrors);
			Assert.Equal(name, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Null(result.Result.DefaultCategoryId);
			Assert.Null(result.Result.AccountGroupId);

			// cleanup
			sut.DeleteAccount(result.Result.Id);
		}

		[Fact]
		public void AddAccount_NonNullDefaultCategory()
		{
			var sut = _fixture.Create<AccountService>();
			var accountName = "test account";
			var balance = 1.23M;

			// get a default category
			var categoryName = "test category";
			var categoryResult = sut.AddCategory(categoryName);
			Assert.False(categoryResult.HasErrors);

			var result = sut.AddAccount(accountName, balance, categoryResult.Result.Id);

			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(categoryResult.Result.Id, result.Result.DefaultCategoryId);

			// cleanup
			sut.DeleteAccount(result.Result.Id);
			sut.DeleteCategory(categoryResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NonExistantDefaultCategory()
		{
			var sut = _fixture.Create<AccountService>();
			var accountName = "test account";
			var balance = 1.23M;
			var defaultCategoryId = -1;

			var result = sut.AddAccount(accountName, balance, defaultCategoryId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void AddAccount_NonNullAccountGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var accountName = "test account";
			var balance = 1.23M;

			// get an account group
			var accountGroupName = "test account group";
			var accountGroupResult = sut.AddAccountGroup(accountGroupName);
			Assert.False(accountGroupResult.HasErrors);

			var result = sut.AddAccount(accountName, balance, accountGroupId: accountGroupResult.Result.Id);

			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(accountGroupResult.Result.Id, result.Result.AccountGroupId);

			// cleanup
			sut.DeleteAccount(result.Result.Id);
			sut.DeleteAccountGroup(accountGroupResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NonExistantAccountGroup()
		{
			var sut = _fixture.Create<AccountService>();
			var accountName = "test account";
			var balance = 1.23M;
			var accountGroupId = -1;

			var result = sut.AddAccount(accountName, balance, accountGroupId: accountGroupId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

	public class AddAccountGroupTests : BaseFixture
	{

	}

	public class AddCategoryTests : BaseFixture
	{

	}

	public class AddCategoryGroupTests : BaseFixture
	{

	}

	public class AddBillTests : BaseFixture
	{

	}

	public class AddBillGroupTests : BaseFixture
	{

	}

	public class AddVendorTests : BaseFixture
	{

	}
	
	#endregion

	#region Delete Tests

	public class DeleteAccountTests : BaseFixture
	{
		[Fact]
		public void DeleteAccount_ExistingAccount()
		{
			var sut = _fixture.Create<AccountService>();
			var name = "test account";
			var balance = 1.23M;

			// add a test account
			var addResult = sut.AddAccount(name, balance);
			Assert.False(addResult.HasErrors);

			// delete the test account
			var deletionResult = sut.DeleteAccount(addResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test account does not exist
			var getResult = sut.GetAccount(addResult.Result.Id);
			Assert.True(getResult.HasErrors);
			Assert.True(getResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void DeleteAccount_NonExistantAccount()
		{
			var sut = _fixture.Create<AccountService>();

			var result = sut.DeleteAccount(-1);
			Assert.True(result.HasErrors);
		}
	}

	public class DeleteAccountGroupTests : BaseFixture
	{

	}

	public class DeleteCategoryTests : BaseFixture
	{

	}
	public class DeleteCategoryGroupTests : BaseFixture
	{

	}

	public class DeleteVendorTests : BaseFixture
	{

	}

	public class DeleteVendorMapTests : BaseFixture
	{

	}

	public class DeleteBillTests : BaseFixture
	{

	}

	public class DeleteBillGroupTests : BaseFixture
	{

	}

	#endregion
}

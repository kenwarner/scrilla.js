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
	public class AccountServiceFixture : BaseFixture<AccountService>
	{
		[Fact]
		public void GetAccount_ExistingAccount_WithDefaultCategory_And_AccountGroup()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var accountName = "test account";
			var balance = 1.23M;

			// get a default category
			var categoryName = "test category";
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// get a default account group
			var accountGroupName = "test account group";
			var addAccountGroupResult = _sut.AddAccountGroup(accountGroupName);
			Assert.False(addAccountGroupResult.HasErrors);

			// create test account
			var addAccountResult = _sut.AddAccount(accountName, balance, addCategoryResult.Result.Id, addAccountGroupResult.Result.Id);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(balance, addAccountResult.Result.Balance);
			Assert.Equal(addCategoryResult.Result.Id, addAccountResult.Result.DefaultCategoryId);
			Assert.Equal(addAccountGroupResult.Result.Id, addAccountResult.Result.AccountGroupId);

			// act
			var result = _sut.GetAccount(addAccountResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(addCategoryResult.Result.Id, result.Result.DefaultCategoryId);
			Assert.Equal(addAccountGroupResult.Result.Id, result.Result.AccountGroupId);

			// cleanup
			_sut.DeleteAccount(addAccountResult.Result.Id);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
			_sut.DeleteAccountGroup(addAccountGroupResult.Result.Id);
		}

		[Fact]
		public void GetAccount_ExistingAccount_WithNullDefaultCategory_And_NullAccountGroup()
		{
			var accountName = "test account";
			var initialBalance = 1.23M;
			int? defaultCategoryId = null;
			int? accountGroupId = null;

			// create test account
			var addAccountResult = _sut.AddAccount(accountName, initialBalance, defaultCategoryId, accountGroupId);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(initialBalance, addAccountResult.Result.InitialBalance);
			Assert.Equal(initialBalance, addAccountResult.Result.Balance);
			Assert.Equal(defaultCategoryId, addAccountResult.Result.DefaultCategoryId);
			Assert.Equal(accountGroupId, addAccountResult.Result.AccountGroupId);

			// act
			var result = _sut.GetAccount(addAccountResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(initialBalance, result.Result.InitialBalance);
			Assert.Equal(initialBalance, result.Result.Balance);
			Assert.Equal(defaultCategoryId, result.Result.DefaultCategoryId);
			Assert.Equal(accountGroupId, result.Result.AccountGroupId);

			// cleanup
			_sut.DeleteAccount(addAccountResult.Result.Id);
		}

		[Fact]
		public void GetAccount_NonExistantAccount()
		{
			var nonExistantAccountId = -1;

			// act
			var result = _sut.GetAccount(nonExistantAccountId);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}


		[Fact]
		public void GetAccountGroup_ExistingAccountGroup()
		{
			var accountGroupName = "test account group";
			var displayOrder = 1;
			var isActive = true;

			// create test account group
			var addAccountGroupResult = _sut.AddAccountGroup(accountGroupName, displayOrder, isActive);
			Assert.False(addAccountGroupResult.HasErrors);
			Assert.Equal(accountGroupName, addAccountGroupResult.Result.Name);
			Assert.Equal(displayOrder, addAccountGroupResult.Result.DisplayOrder);
			Assert.Equal(isActive, addAccountGroupResult.Result.IsActive);

			// act
			var result = _sut.GetAccountGroup(addAccountGroupResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountGroupName, result.Result.Name);
			Assert.Equal(displayOrder, result.Result.DisplayOrder);
			Assert.Equal(isActive, result.Result.IsActive);

			// cleanup
			_sut.DeleteAccountGroup(addAccountGroupResult.Result.Id);
		}

		[Fact]
		public void GetAccountGroup_NonExistantAccount()
		{
			var nonExistantAccountGroupId = -1;

			// act
			var result = _sut.GetAccountGroup(nonExistantAccountGroupId);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetAllAccounts_OneAccountExistsAfterAddingAnAccount()
		{
			var accountsResult = _sut.GetAllAccounts();
			Assert.False(accountsResult.HasErrors);
			Assert.Empty(accountsResult.Result);

			// create test account
			var accountName = "test account";
			var initialBalance = 1.23M;
			var addAccountResult = _sut.AddAccount(accountName, initialBalance);
			Assert.False(addAccountResult.HasErrors);

			// act
			accountsResult = _sut.GetAllAccounts();
			Assert.False(accountsResult.HasErrors);
			Assert.Equal(1, accountsResult.Result.Count());

			// cleanup
			_sut.DeleteAccount(addAccountResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NullDefaultCategory_And_NullAccountGroup()
		{
			var accountName = "test account";
			var initialBalance = 1.23M;

			// act
			var addAcountResult = _sut.AddAccount(accountName, initialBalance);
			Assert.False(addAcountResult.HasErrors);
			Assert.Equal(accountName, addAcountResult.Result.Name);
			Assert.Equal(initialBalance, addAcountResult.Result.InitialBalance);
			Assert.Equal(initialBalance, addAcountResult.Result.Balance);
			Assert.Null(addAcountResult.Result.DefaultCategoryId);
			Assert.Null(addAcountResult.Result.AccountGroupId);

			// cleanup
			_sut.DeleteAccount(addAcountResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NonNullDefaultCategory()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var accountName = "test account";
			var initialBalance = 1.23M;

			// get a default category
			var categoryName = "test category";
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// act
			var addAccountResult = _sut.AddAccount(accountName, initialBalance, addCategoryResult.Result.Id);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(initialBalance, addAccountResult.Result.InitialBalance);
			Assert.Equal(initialBalance, addAccountResult.Result.Balance);
			Assert.Equal(addCategoryResult.Result.Id, addAccountResult.Result.DefaultCategoryId);

			// cleanup
			_sut.DeleteAccount(addAccountResult.Result.Id);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NonExistantDefaultCategory()
		{
			var accountName = "test account";
			var defaultCategoryId = -1;

			// act
			var result = _sut.AddAccount(accountName, defaultCategoryId: defaultCategoryId);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void AddAccount_NonNullAccountGroup()
		{
			var accountName = "test account";
			var initialBalance = 1.23M;

			// get an account group
			var accountGroupName = "test account group";
			var addAccountGroupResult = _sut.AddAccountGroup(accountGroupName);
			Assert.False(addAccountGroupResult.HasErrors);

			// act
			var addAccountResult = _sut.AddAccount(accountName, initialBalance, accountGroupId: addAccountGroupResult.Result.Id);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(initialBalance, addAccountResult.Result.InitialBalance);
			Assert.Equal(initialBalance, addAccountResult.Result.Balance);
			Assert.Equal(addAccountGroupResult.Result.Id, addAccountResult.Result.AccountGroupId);

			// cleanup
			_sut.DeleteAccount(addAccountResult.Result.Id);
			_sut.DeleteAccountGroup(addAccountGroupResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NonExistantAccountGroup()
		{
			var accountName = "test account";
			var accountGroupId = -1;

			// act
			var result = _sut.AddAccount(accountName, accountGroupId: accountGroupId);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void AddAccountGroup()
		{
			var accountGroupName = "test account group";
			var displayOrder = 1;
			var isActive = true;

			// act
			var result = _sut.AddAccountGroup(accountGroupName, displayOrder, isActive);
			Assert.False(result.HasErrors);
			Assert.Equal(accountGroupName, result.Result.Name);
			Assert.Equal(displayOrder, result.Result.DisplayOrder);
			Assert.Equal(isActive, result.Result.IsActive);
		}

		[Fact]
		public void DeleteAccount_ExistingAccount()
		{
			var accountName = "test account";

			// add a test account
			var addAccountResult = _sut.AddAccount(accountName);
			Assert.False(addAccountResult.HasErrors);

			// delete the test account
			var deletionResult = _sut.DeleteAccount(addAccountResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test account does not exist
			var getResult = _sut.GetAccount(addAccountResult.Result.Id);
			Assert.True(getResult.HasErrors);
			Assert.True(getResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void DeleteAccount_NonExistantAccount()
		{
			var nonExistantAccountId = -1;

			// act
			var result = _sut.DeleteAccount(nonExistantAccountId);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void DeleteAccountGroup_ExistingAccountGroup()
		{
			var accountGroupName = "test account group";

			// add a test account group
			var addAccountGroupResult = _sut.AddAccountGroup(accountGroupName);
			Assert.False(addAccountGroupResult.HasErrors);

			// delete the test account group
			var deletionResult = _sut.DeleteAccountGroup(addAccountGroupResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test account group does not exist
			var getResult = _sut.GetAccount(addAccountGroupResult.Result.Id);
			Assert.True(getResult.HasErrors);
			Assert.True(getResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void DeleteAccountGroup_NonExistantAccountGroup()
		{
			var nonExistantAccountGroupId = -1;

			// act
			var result = _sut.DeleteAccountGroup(nonExistantAccountGroupId);
			Assert.True(result.HasErrors);
		}

	}

	public class AccountServiceRealDataFixture : BaseFixture<AccountService>
	{
		public AccountServiceRealDataFixture() : base("RealDataConnectionString") { }

		[Fact]
		public void GetAccountBalances_RealData()
		{
			// act
			var result = _sut.GetAccountBalances();

			// assert
		}
	}
}

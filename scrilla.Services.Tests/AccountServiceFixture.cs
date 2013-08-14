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
		public AccountServiceFixture()
		{
			_fixture.Register<ICategoryService>(() => _fixture.Create<CategoryService>());
			_sut = _fixture.Create<AccountService>();
		}

		[Fact]
		public void GetAccount_ExistingAccount_WithDefaultCategory_And_AccountGroup()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var accountName = "test account";
			var balance = 1.23M;

			// get a default category
			var categoryName = "test category";
			var categoryResult = categoryService.AddCategory(categoryName);
			Assert.False(categoryResult.HasErrors);

			// get a default account group
			var accountGroupName = "test account group";
			var accountGroupResult = _sut.AddAccountGroup(accountGroupName);
			Assert.False(accountGroupResult.HasErrors);

			// create test account
			var addAccountResult = _sut.AddAccount(accountName, balance, categoryResult.Result.Id, accountGroupResult.Result.Id);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(balance, addAccountResult.Result.Balance);
			Assert.Equal(categoryResult.Result.Id, addAccountResult.Result.DefaultCategoryId);
			Assert.Equal(accountGroupResult.Result.Id, addAccountResult.Result.AccountGroupId);

			// act
			var result = _sut.GetAccount(addAccountResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(categoryResult.Result.Id, result.Result.DefaultCategoryId);
			Assert.Equal(accountGroupResult.Result.Id, result.Result.AccountGroupId);

			// cleanup
			_sut.DeleteAccount(addAccountResult.Result.Id);
			categoryService.DeleteCategory(categoryResult.Result.Id);
			_sut.DeleteAccountGroup(accountGroupResult.Result.Id);
		}

		[Fact]
		public void GetAccount_ExistingAccount_WithNullDefaultCategory_And_NullAccountGroup()
		{
			var accountName = "test account";
			var balance = 1.23M;
			int? defaultCategoryId = null;
			int? accountGroupId = null;

			// create test account
			var addAccountResult = _sut.AddAccount(accountName, balance, defaultCategoryId, accountGroupId);
			Assert.False(addAccountResult.HasErrors);
			Assert.Equal(accountName, addAccountResult.Result.Name);
			Assert.Equal(balance, addAccountResult.Result.Balance);
			Assert.Equal(defaultCategoryId, addAccountResult.Result.DefaultCategoryId);
			Assert.Equal(accountGroupId, addAccountResult.Result.AccountGroupId);

			// act
			var result = _sut.GetAccount(addAccountResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
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

			var name = "test account";
			var balance = 1.23M;
			var addAccountResult = _sut.AddAccount(name, balance);
			Assert.False(addAccountResult.HasErrors);

			accountsResult = _sut.GetAllAccounts();
			Assert.False(accountsResult.HasErrors);
			Assert.Equal(1, accountsResult.Result.Count());

			// cleanup
			_sut.DeleteAccount(addAccountResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NullDefaultCategory_And_NullAccountGroup()
		{
			
			var name = "test account";
			var balance = 1.23M;

			var result = _sut.AddAccount(name, balance);

			Assert.False(result.HasErrors);
			Assert.Equal(name, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Null(result.Result.DefaultCategoryId);
			Assert.Null(result.Result.AccountGroupId);

			// cleanup
			_sut.DeleteAccount(result.Result.Id);
		}

		[Fact]
		public void AddAccount_NonNullDefaultCategory()
		{
			var categoryService = _fixture.Create<CategoryService>();
			
			var accountName = "test account";
			var balance = 1.23M;

			// get a default category
			var categoryName = "test category";
			var categoryResult = categoryService.AddCategory(categoryName);
			Assert.False(categoryResult.HasErrors);

			var result = _sut.AddAccount(accountName, balance, categoryResult.Result.Id);

			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(categoryResult.Result.Id, result.Result.DefaultCategoryId);

			// cleanup
			_sut.DeleteAccount(result.Result.Id);
			categoryService.DeleteCategory(categoryResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NonExistantDefaultCategory()
		{
			
			var accountName = "test account";
			var balance = 1.23M;
			var defaultCategoryId = -1;

			var result = _sut.AddAccount(accountName, balance, defaultCategoryId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void AddAccount_NonNullAccountGroup()
		{
			
			var accountName = "test account";
			var balance = 1.23M;

			// get an account group
			var accountGroupName = "test account group";
			var accountGroupResult = _sut.AddAccountGroup(accountGroupName);
			Assert.False(accountGroupResult.HasErrors);

			var result = _sut.AddAccount(accountName, balance, accountGroupId: accountGroupResult.Result.Id);

			Assert.False(result.HasErrors);
			Assert.Equal(accountName, result.Result.Name);
			Assert.Equal(balance, result.Result.Balance);
			Assert.Equal(accountGroupResult.Result.Id, result.Result.AccountGroupId);

			// cleanup
			_sut.DeleteAccount(result.Result.Id);
			_sut.DeleteAccountGroup(accountGroupResult.Result.Id);
		}

		[Fact]
		public void AddAccount_NonExistantAccountGroup()
		{
			var accountName = "test account";
			var balance = 1.23M;
			var accountGroupId = -1;

			var result = _sut.AddAccount(accountName, balance, accountGroupId: accountGroupId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void AddAccountGroup_()
		{
			
		}

		[Fact]
		public void DeleteAccount_ExistingAccount()
		{
			
			var name = "test account";
			var balance = 1.23M;

			// add a test account
			var addResult = _sut.AddAccount(name, balance);
			Assert.False(addResult.HasErrors);

			// delete the test account
			var deletionResult = _sut.DeleteAccount(addResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test account does not exist
			var getResult = _sut.GetAccount(addResult.Result.Id);
			Assert.True(getResult.HasErrors);
			Assert.True(getResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void DeleteAccount_NonExistantAccount()
		{
			

			var result = _sut.DeleteAccount(-1);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void DeleteAccountGroup_ExistingAccountGroup()
		{
			
			var name = "test account group";

			// add a test account group
			var addResult = _sut.AddAccountGroup(name);
			Assert.False(addResult.HasErrors);

			// delete the test account group
			var deletionResult = _sut.DeleteAccountGroup(addResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test account group does not exist
			var getResult = _sut.GetAccount(addResult.Result.Id);
			Assert.True(getResult.HasErrors);
			Assert.True(getResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void DeleteAccountGroup_NonExistantAccountGroup()
		{
			

			var result = _sut.DeleteAccountGroup(-1);
			Assert.True(result.HasErrors);
		}

	}
}

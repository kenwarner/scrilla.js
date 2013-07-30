using DapperExtensions;
using DapperExtensions.Sql;
using Ploeh.AutoFixture;
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
	public class DelectAccountTests : BaseFixture
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

}

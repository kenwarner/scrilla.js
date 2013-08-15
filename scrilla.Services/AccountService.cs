using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using scrilla.Data;
using DapperExtensions;

namespace scrilla.Services
{
	public class AccountService : EntityService, IAccountService
	{
		private ICategoryService _categoryService;

		public AccountService(IDatabase database, ICategoryService categoryService)
			: base(database) 
		{
			_categoryService = categoryService;
		}

		public ServiceResult<Account> GetAccount(int accountId)
		{
			return base.GetEntity<Account>(accountId);
		}

		public ServiceResult<AccountGroup> GetAccountGroup(int accountGroupId)
		{
			return base.GetEntity<AccountGroup>(accountGroupId);
		}
		
		public ServiceResult<IEnumerable<Account>> GetAllAccounts()
		{
			return GetAllEntity<Account>();
		}


		public ServiceResult<Account> AddAccount(string name, decimal initialBalance = 0.0M, int? defaultCategoryId = null, int? accountGroupId = null)
		{
			var result = new ServiceResult<Account>();

			// does AccountGroup exist?
			if (accountGroupId.HasValue)
			{
				var accountGroupResult = GetAccountGroup(accountGroupId.Value);
				if (accountGroupResult.HasErrors)
				{
					result.AddErrors(accountGroupResult);
					return result;
				}
			}

			// does default Category exist?
			if (defaultCategoryId.HasValue)
			{
				var categoryResult = _categoryService.GetCategory(defaultCategoryId.Value);
				if (categoryResult.HasErrors)
				{
					result.AddErrors(categoryResult);
					return result;
				}
			}

			// create Account
			var account = new Account()
			{
				Name = name,
				InitialBalance = initialBalance,
				DefaultCategoryId = defaultCategoryId,
				Balance = initialBalance,
				BalanceTimestamp = DateTime.Now,
				AccountGroupId = accountGroupId
			};

			_db.Insert<Account>(account);

			result.Result = account;
			return result;
		}

		public ServiceResult<AccountGroup> AddAccountGroup(string name, int displayOrder = 0, bool isActive = true)
		{
			var result = new ServiceResult<AccountGroup>();

			// create AccountGroup
			var accountGroup = new AccountGroup()
			{
				Name = name,
				DisplayOrder = displayOrder,
				IsActive = isActive
			};

			_db.Insert<AccountGroup>(accountGroup);

			result.Result = accountGroup;
			return result;
		}


		public ServiceResult<bool> DeleteAccount(int accountId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<Account>(Predicates.Field<Account>(x => x.Id, Operator.Eq, accountId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Account {0} not found", accountId);
				return result;
			}

			result.Result = deletionResult;
			return result;
		}

		public ServiceResult<bool> DeleteAccountGroup(int accountGroupId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<AccountGroup>(Predicates.Field<AccountGroup>(x => x.Id, Operator.Eq, accountGroupId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "AccountGroup {0} not found", accountGroupId);
				return result;
			}

			result.Result = deletionResult;
			return result;
		}
	}
}

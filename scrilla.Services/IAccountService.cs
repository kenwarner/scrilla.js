using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data;
using scrilla.Services.Models;

namespace scrilla.Services
{
	public interface IAccountService
	{
		ServiceResult<Account> GetAccount(int accountId);

		ServiceResult<AccountGroup> GetAccountGroup(int accountGroupId);

		ServiceResult<IEnumerable<Account>> GetAllAccounts();

		ServiceResult<IEnumerable<AccountGroup>> GetAllAccountGroups();

		ServiceResult<IEnumerable<AccountNameMap>> GetAllAccountNameMaps();


		ServiceResult<Account> AddAccount(string name, decimal initialBalance = 0.0M, int? defaultCategoryId = null, int? accountGroupId = null);
		ServiceResult<AccountGroup> AddAccountGroup(string name, int displayOrder = 0, bool isActive = true);
		ServiceResult<AccountNameMap> AddAccountNameMap(int accountId, string name);


		ServiceResult<bool> DeleteAccount(int accountId);
		ServiceResult<bool> DeleteAccountGroup(int accountGroupId);

		ServiceResult<Account> UpdateAccount(int accountId, Filter<string> name = null, Filter<decimal> initialBalance = null, Filter<int?> defaultCategoryId = null, Filter<int?> accountGroupId = null);
		ServiceResult<bool> UpdateAccountBalances();


		ServiceResult<AccountsModel> GetAccounts(DateTime from, DateTime to);
	}
}

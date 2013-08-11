using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data;

namespace scrilla.Services
{
	public interface IAccountService
	{
		ServiceResult<Account> GetAccount(int accountId);

		ServiceResult<AccountGroup> GetAccountGroup(int accountGroupId);

		ServiceResult<IEnumerable<Account>> GetAllAccounts();


		ServiceResult<Account> AddAccount(string name, decimal initialBalance = 0.0M, int? defaultCategoryId = null, int? accountGroupId = null);
		ServiceResult<AccountGroup> AddAccountGroup(string name, int displayOrder = 0, bool isActive = true);


		ServiceResult<bool> DeleteAccount(int accountId);
		ServiceResult<bool> DeleteAccountGroup(int accountGroupId);
	}
}

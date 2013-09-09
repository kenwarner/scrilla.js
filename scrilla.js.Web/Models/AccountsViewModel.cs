using scrilla.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrilla.js.Web.Models
{
	public class AccountsViewModel
	{
		public DateRangeViewModel DateRange { get; set; }

		public AccountBalancesModel AccountsBalances { get; set; }

		public string AccountGroupClass(AccountBalancesModel.AccountGroupModel accountGroupModel)
		{
			return "account-group" + (accountGroupModel.IsActive ? "" : " inactive");
		}
	}
}
using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services.Models
{
	public class AccountBalancesModel : List<AccountBalancesModel.AccountGroupModel>
	{
		public class AccountGroupModel
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public bool IsActive { get; set; }
			public int DisplayOrder { get; set; }
			public IEnumerable<AccountModel> Accounts { get; set; }
		}

		public class AccountModel
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public decimal InitialBalance { get; set; }
			public decimal Balance { get; set; }
			public DateTime BalanceTimestamp { get; set; }
			public int UncategorizedTransactionCount { get; set; }
			public IEnumerable<AccountBalanceModel> AccountBalances { get; set; }
		}

		public class AccountBalanceModel
		{
			public int AccountId { get; set; }
			public DateTime Month { get; set; }
			public decimal MonthTransactionTotal { get; set; }
			public decimal MonthEndBalance { get; set; }
		}
	}
}

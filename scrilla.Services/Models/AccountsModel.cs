using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services.Models
{
	public class AccountsModel : List<AccountGroupBalance>
	{
	}

	public class AccountGroupBalance : List<AccountBalance>
	{
		public AccountGroup AccountGroup { get; set; }
	}

	public class AccountBalance : Account
	{
		public AccountBalance()
		{
			Balances = new Dictionary<DateTime, decimal>();
		}

		public int UncategorizedTransactionCount { get; set; }
		public Dictionary<DateTime, decimal> Balances { get; set; }
	}
}

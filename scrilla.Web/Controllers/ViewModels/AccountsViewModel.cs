using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Web.Controllers.ViewModels
{
	using scrilla.Data;
	using scrilla.Data.Domain;

	public class AccountsViewModel
	{
		public AccountsViewModel()
		{
			AccountGroupBalances = new List<AccountGroupBalance>();
		}

		public List<AccountGroupBalance> AccountGroupBalances { get; set; }

		public DateTime? UrlFrom { get; set; }
		public DateTime? UrlTo { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }

		public DateTime FromMonth
		{
			get
			{
				return new DateTime(From.Year, From.Month, 1);
			}
		}

		public DateTime ToMonth
		{
			get
			{
				return new DateTime(To.Year, To.Month, 1);
			}
		}
	}

	public class AccountBalance
	{
		public AccountBalance()
		{
			Balances = new Dictionary<DateTime, decimal>();
		}

		public Account Account { get; set; }
		public int UncategorizedTransactionCount { get; set;}
		public Dictionary<DateTime, decimal> Balances { get; set; }
	}

	public class AccountGroupBalance
	{
		public AccountGroupBalance()
		{
			AccountBalances = new List<AccountBalance>();
		}

		public AccountGroup AccountGroup { get; set; }
		public List<AccountBalance> AccountBalances { get; set; }
	}

}

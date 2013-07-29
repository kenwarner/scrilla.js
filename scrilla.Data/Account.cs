using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class Account
	{
		public int Id { get; set; }
		public int AccountGroupId { get; set; }
		public int? DefaultCategoryId { get; set; }
		public string Name { get; set; }
		public decimal InitialBalance { get; set; }
		public decimal Balance { get; set; }
		public DateTime BalanceTimestamp { get; set; }
	}

	public class AccountNameMap
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Name { get; set; }
	}
}

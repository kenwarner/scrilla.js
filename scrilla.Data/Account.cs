using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class Account
	{
		public Account()
		{
			this.Transactions = new List<Transaction>();
		}

		public int Id { get; set; }
		public int AccountGroupId { get; set; }
		public int? DefaultCategoryId { get; set; }
		public string Name { get; set; }
		public decimal InitialBalance { get; set; }
		public decimal Balance { get; set; }
		public DateTime BalanceTimestamp { get; set; }
		public virtual AccountGroup AccountGroup { get; set; }
		public virtual Category DefaultCategory { get; set; }

		public virtual ICollection<Transaction> Transactions { get; set; }
		public virtual ICollection<AccountNameMap> AccountNameMaps { get; set; }
	}

	public class AccountNameMap
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Name { get; set; }

		public virtual Account Account { get; set; }
	}
}

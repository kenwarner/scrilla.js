using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.Domain
{
	public class AccountGroup
	{
		public AccountGroup()
		{
			Accounts = new List<Account>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }

		public ICollection<Account> Accounts { get; set; }
	}
}

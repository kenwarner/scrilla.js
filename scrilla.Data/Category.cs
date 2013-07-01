using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class Category
	{
		public Category()
		{
			Subtransactions = new List<Subtransaction>();
			VendorDefaults = new List<Vendor>();
			BudgetCategories = new List<BudgetCategory>();
		}

		public int Id { get; set; }
		public int CategoryGroupId { get; set; }
		public string Name { get; set; }

		public virtual CategoryGroup CategoryGroup { get; set; }
		public virtual ICollection<Subtransaction> Subtransactions { get; set; }
		public virtual ICollection<Vendor> VendorDefaults { get; set; }
		public virtual ICollection<BudgetCategory> BudgetCategories { get; set; }
	}
}

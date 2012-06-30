using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.Domain
{
	public class Vendor
	{
		public Vendor()
		{
			this.Transactions = new List<Transaction>();
			this.ImportDescriptionVendorMaps = new List<ImportDescriptionVendorMap>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public Nullable<int> DefaultCategoryId { get; set; }

		public virtual Category DefaultCategory { get; set; }
		public virtual ICollection<Bill> Bills { get; set; }
		public virtual ICollection<Transaction> Transactions { get; set; }
		public virtual ICollection<ImportDescriptionVendorMap> ImportDescriptionVendorMaps { get; set; }
	}
}

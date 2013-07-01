using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class BillGroup
	{
		public BillGroup()
		{
			Bills = new List<Bill>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }

		public virtual ICollection<Bill> Bills { get; set; }
	}
}

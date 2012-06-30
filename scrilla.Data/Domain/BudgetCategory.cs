using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.Domain
{
	public class BudgetCategory
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public DateTime Month { get; set; }
		public decimal Amount { get; set; }
		public virtual Category Category { get; set; }
	}
}

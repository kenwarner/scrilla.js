using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class Subtransaction
	{
		public int Id { get; set; }
		public int TransactionId { get; set; }
		public Nullable<int> CategoryId { get; set; }
		public decimal Amount { get; set; }
		public string Memo { get; set; }  // From Bank Account
		public string Notes { get; set; }  // Personal Notes
		public bool IsTransfer { get; set; }
		public bool IsExcludedFromBudget { get; set; }

		public virtual Transaction Transaction { get; set; }
		public virtual Category Category { get; set; }
	}
}

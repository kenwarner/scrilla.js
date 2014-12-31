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
		public string CategoryName { get; set; }
		public decimal Amount { get; set; }
		/// <summary>
		/// Memo set from bank account
		/// </summary>
		public string Memo { get; set; }
		/// <summary>
		/// Personal notes
		/// </summary>
		public string Notes { get; set; }
		public bool IsTransfer { get; set; }
		public bool IsExcludedFromBudget { get; set; }
	}
}

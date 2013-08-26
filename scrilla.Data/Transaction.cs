using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class Transaction
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public DateTime Timestamp { get; set; }
		public DateTime OriginalTimestamp { get; set; }
		public decimal Amount { get; set; }
		//public decimal Balance { get; set; }
		public bool IsReconciled { get; set; }

		public Nullable<int> VendorId { get; set; }
		public Nullable<int> BillTransactionId { get; set; }

		public IEnumerable<Subtransaction> Subtransactions { get; set; }
	}
}

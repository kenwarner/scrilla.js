using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.Domain
{
	public class Transaction
	{
		public Transaction()
		{
			Subtransactions = new List<Subtransaction>();
		}

		public int Id { get; set; }
		public int AccountId { get; set; }
		public DateTime Timestamp { get; set; }
		public DateTime OriginalTimestamp { get; set; }
		public decimal Amount { get; set; }
		public decimal Balance { get; set; }
		public bool IsReconciled { get; set; }

		public Nullable<int> VendorId { get; set; }
		public Nullable<int> BillTransactionId { get; set; }
		public virtual Account Account { get; set; }
		public virtual BillTransaction BillTransaction { get; set; }
		public virtual ICollection<Subtransaction> Subtransactions { get; set; }
		public virtual Vendor Vendor { get; set; }
	}
}

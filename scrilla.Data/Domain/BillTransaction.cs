using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.Domain
{
	public class BillTransaction
	{
		public BillTransaction()
		{
			this.Transactions = new List<Transaction>();
			this.TransactionPredictions = new List<Tuple<Transaction, double>>();
		}

		public int Id { get; set; }
		public int BillId { get; set; }
		public decimal Amount { get; set; }
		public DateTime Timestamp { get; set; }
		public bool IsPaid { get; set; }
		public decimal OriginalAmount { get; set; }
		public DateTime OriginalTimestamp { get; set; }
		public Nullable<int> CategoryId { get; set; }
		public Nullable<int> VendorId { get; set; }
		public Nullable<int> OriginalCategoryId { get; set; }
		public Nullable<int> OriginalVendorId { get; set; }

		public virtual Bill Bill { get; set; }
		public virtual Category Category { get; set; }
		public virtual Category OriginalCategory { get; set; }
		public virtual Vendor Vendor { get; set; }
		//public virtual Vendor Vendor1 { get; set; }
		public virtual ICollection<Transaction> Transactions { get; set; }

		public List<Tuple<Transaction, double>> TransactionPredictions { get; set; }
	}
}

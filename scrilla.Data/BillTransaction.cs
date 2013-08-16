using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class BillTransaction
	{
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

		//public List<Tuple<Transaction, double>> TransactionPredictions { get; set; }
	}
}

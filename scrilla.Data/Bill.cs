using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class Bill
	{
		public Bill()
		{
			this.BillTransactions = new List<BillTransaction>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public int BillGroupId { get; set; }
		public Nullable<int> VendorId { get; set; }
		public Nullable<int> CategoryId { get; set; }
		public decimal Amount { get; set; }
		public DateTime StartDate { get; set; }
		public Nullable<DateTime> StartDate2 { get; set; }
		public DateTime EndDate { get; set; }
		public Nullable<DateTime> EndDate2 { get; set; }
		public int RecurrenceFrequency { get; set; }
		public virtual BillGroup BillGroup { get; set; }
		public virtual Category Category { get; set; }
		public virtual Vendor Vendor { get; set; }
		public virtual ICollection<BillTransaction> BillTransactions { get; set; }

		public BillTransaction DueNext
		{
			get
			{
				if (BillTransactions == null)
					return null;

				var upcoming = BillTransactions.Where(y => y.Timestamp > DateTime.Now);
				if (!upcoming.Any())
					return null;

				return upcoming.OrderBy(y => y.Timestamp).FirstOrDefault();
			}
		}

		public static Dictionary<int, string> AvailableFrequencies
		{
			get
			{
				var frequencies = new Dictionary<int, string>();
				frequencies.Add(0, "One Time");
				frequencies.Add(1, "Daily");
				frequencies.Add(7, "Weekly");
				frequencies.Add(14, "Bi-Weekly");
				frequencies.Add(-1, "Monthly");
				frequencies.Add(-3, "Quarterly");
				frequencies.Add(-12, "Yearly");
				return frequencies;
			}
		}
	}
}

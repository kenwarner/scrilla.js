using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class Bill
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Nullable<int> BillGroupId { get; set; }
		public Nullable<int> VendorId { get; set; }
		public Nullable<int> CategoryId { get; set; }
		public decimal Amount { get; set; }
		public DateTime StartDate { get; set; }
		public Nullable<DateTime> StartDate2 { get; set; }
		public DateTime EndDate { get; set; }
		public Nullable<DateTime> EndDate2 { get; set; }
		public BillFrequency RecurrenceFrequency { get; set; }

		//public BillTransaction DueNext
		//{
		//	get
		//	{
		//		if (BillTransactions == null)
		//			return null;

		//		var upcoming = BillTransactions.Where(y => y.Timestamp > DateTime.Now);
		//		if (!upcoming.Any())
		//			return null;

		//		return upcoming.OrderBy(y => y.Timestamp).FirstOrDefault();
		//	}
		//}
	}

	public enum BillFrequency : int
	{
		OneTime = 0,
		Daily = 1,
		Weekly = 7,
		BiWeekly = 14,
		Monthly = -1,
		Quarterly = -3,
		Yearly = -12
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using scrilla.Data;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class VendorsViewModel
	{
		public List<VendorTotal> VendorTotals { get; set; }

		public DateTime? UrlFrom { get; set; }
		public DateTime? UrlTo { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }

		public DateTime FromMonth
		{
			get
			{
				return new DateTime(From.Year, From.Month, 1);
			}
		}

		public DateTime ToMonth
		{
			get
			{
				return new DateTime(To.Year, To.Month, 1);
			}
		}
	}

	public class VendorTotal
	{
		public Vendor Vendor { get; set; }
		public Dictionary<DateTime, decimal> Totals { get; set; }

		public DateTime MostRecentTotal
		{
			get
			{
				if (Totals == null || Totals.Count(x => x.Value != 0M) == 0)
					return DateTime.MinValue;

				return Totals.Where(x => x.Value != 0M).Max(x => x.Key);
			}
		}

		public decimal MostRecentTotalAmount
		{
			get
			{
				decimal amount = 0M;
				Totals.TryGetValue(MostRecentTotal, out amount);
				return amount;
			}
		}

	}
}
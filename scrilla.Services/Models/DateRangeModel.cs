using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services.Models
{
	public class DateRangeModel
	{
		public DateRangeModel(DateTime? from = null, DateTime? to = null)
			: this(from.ToString(), to.ToString()) { }

		public DateRangeModel(string from = null, string to = null)
		{
			DateTime fromDate;
			if (DateTime.TryParse(from, out fromDate))
			{
				From = fromDate;
				UrlFrom = fromDate;
			}
			else
			{
				From = DefaultFrom();
				UrlFrom = null;
			}

			DateTime toDate;
			if (DateTime.TryParse(to, out toDate))
			{
				To = toDate;
				UrlTo = toDate;
			}
			else
			{
				To = DefaultTo();
				UrlTo = null;
			}
		}

		public DateTime? UrlFrom { get; private set; }
		public DateTime? UrlTo { get; private set; }
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

		private DateTime DefaultFrom()
		{
			return DefaultTo().AddDays(1).AddMonths(-6);
		}

		private DateTime DefaultTo()
		{
			var now = DateTime.Now;
			return new DateTime(now.Year, now.Month, 1).AddMonths(2).AddDays(-1);
		}

		public IEnumerable<DateTime> Months
		{
			get
			{
				var months = new List<DateTime>();
				for (var month = FromMonth; month <= ToMonth; month = month.AddMonths(1))
					months.Add(month);

				return months;
			}
		}

		public string RangeSummary
		{
			get { return this.ToString(); }
		}

		public override string ToString()
		{
			return From.ToString("MMMM yyyy") + " to " + To.ToString("MMMM yyyy");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using scrilla.Data;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class BillTransactionsViewModel
	{
		public BillTransactionsViewModel()
		{
			BillTransactions = new List<BillTransaction>();
		}

		public IEnumerable<BillTransaction> BillTransactions { get; set; }

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
}
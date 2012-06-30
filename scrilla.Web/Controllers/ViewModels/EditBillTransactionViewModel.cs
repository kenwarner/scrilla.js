using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace scrilla.Web.Controllers.ViewModels
{
	public class EditBillTransactionViewModel
	{
		public int BillTransactionId { get; set; }

		[DisplayFormat(DataFormatString = "{0:$###,##0.00}", ApplyFormatInEditMode = true)]
		public decimal Amount { get; set; }

		public bool? IsPaid { get; set; }

		public int? TransactionId { get; set; }

		public string Date { get; set; }

		public DateTime? ParsedDate
		{
			get
			{
				DateTime parsed;
				if (DateTime.TryParse(Date, out parsed))
					return (DateTime?)parsed;
				else
					return null;
			}
		}
	}
}
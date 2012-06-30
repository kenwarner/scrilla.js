using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class TransactionsViewModel
	{
		public bool ShowBalance { get; set; }
		public decimal InitialBalance { get; set; }
		public Account Account { get; set; }
		public Category Category { get; set; }
		public Vendor Vendor { get; set; }
		public IEnumerable<Transaction> Transactions { get; set; }
		public List<CategoryDto> AvailableCategories { get; set; }
		public List<VendorDto> AvailableVendors { get; set; }

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

	public class CategoryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class CategoryGroupDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class VendorDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class BillGroupDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
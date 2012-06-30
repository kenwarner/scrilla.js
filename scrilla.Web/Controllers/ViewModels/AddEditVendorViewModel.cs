using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace scrilla.Web.Controllers.ViewModels
{
	public class AddEditVendorViewModel
	{
		public AddEditVendorViewModel()
		{
			IsEditMode = false;
		}

		public bool IsEditMode { get; set; }
		public bool IsError { get; set; }

		public int VendorId { get; set; }

		[Required]
		public string Name { get; set; }

		[DisplayName("Default Category")]
		public int? DefaultCategory { get; set; }

		public List<CategoryDto> AvailableCategories { get; set; }

		[DisplayName("Update uncategorized transactions?")]
		public bool UpdateUncategorizedTransactions { get; set; }
	}
}
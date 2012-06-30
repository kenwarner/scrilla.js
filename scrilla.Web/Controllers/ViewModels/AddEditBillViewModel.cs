using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using scrilla.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class AddEditBillViewModel
	{
		public AddEditBillViewModel()
		{
			IsEditMode = false;
			StartDate = DateTime.Now.Date;
			EndDate = DateTime.Now.Date.AddYears(1);
			Amount = 100.0M;

			AvailableRecurrences = new SelectList(Bill.AvailableFrequencies, "Key", "Value");
		}

		public bool IsEditMode { get; set; }
		public bool IsError { get; set; }

		public int BillId { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[DisplayFormat(DataFormatString = "{0:$###,##0.00}", ApplyFormatInEditMode = true)]
		public decimal Amount { get; set; }

		[Required]
		[DisplayName("Start Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime StartDate { get; set; }

		[Required]
		[DisplayName("End Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime EndDate { get; set; }

		[DisplayName("Secondary Start Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "", ConvertEmptyStringToNull = true)]
		public DateTime? SecondaryStartDate { get; set; }

		[DisplayName("Secondary End Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "", ConvertEmptyStringToNull = true)]
		public DateTime? SecondaryEndDate { get; set; }

		[Required]
		[DisplayName("Bill Group")]
		public int BillGroupId { get; set; }

		[Required]
		[DisplayName("Category")]
		public int CategoryId { get; set; }

		[Required]
		[DisplayName("Vendor")]
		public int VendorId { get; set; }

		[Required]
		[DisplayName("Frequency")]
		public int Frequency { get; set; }

		[DisplayName("Update all existing?")]
		public bool UpdateExisting { get; set; }

		[DisplayName("Use secondary dates?")]
		public bool IncludeSecondaryDates { get; set; }

		public List<BillGroupDto> AvailableBillGroups { get; set; }
		public List<VendorDto> AvailableVendors { get; set; }
		public List<CategoryDto> AvailableCategories { get; set; }
		public SelectList AvailableRecurrences { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace scrilla.Web.Controllers.ViewModels
{
	public class AddEditCategoryViewModel
	{
		public AddEditCategoryViewModel()
		{
			IsEditMode = false;
		}

		public bool IsEditMode { get; set; }
		public bool IsError { get; set; }

		public int CategoryId { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[DisplayName("Category Group")]
		public int CategoryGroup { get; set; }

		public List<CategoryGroupDto> AvailableCategoryGroups { get; set; }
	}
}
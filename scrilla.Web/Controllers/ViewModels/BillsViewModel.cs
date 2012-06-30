using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using scrilla.Data;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class BillsViewModel
	{
		public BillsViewModel()
		{
			BillGroups = new List<BillGroup>();
		}

		public IEnumerable<BillGroup> BillGroups { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using scrilla.Data;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class BillViewModel
	{
		public Bill Bill { get; set; }
	}
}
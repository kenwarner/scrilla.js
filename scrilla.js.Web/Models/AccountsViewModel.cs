using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrilla.js.Web.Models
{
	public class AccountsViewModel : DateRangeViewModel
	{
		public AccountsViewModel(string from = null, string to = null)
			: base(from, to)
		{

		}
	}
}
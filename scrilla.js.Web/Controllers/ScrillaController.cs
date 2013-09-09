using AttributeRouting.Web.Mvc;
using scrilla.js.Web.Models;
using scrilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace scrilla.js.Web.Controllers
{
	public partial class ScrillaController : Controller
    {
		private readonly IAccountService _accountService;

		public ScrillaController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[GET("", ActionPrecedence = 1)]
		[GET("Accounts")]
		public virtual ActionResult Accounts(string from = null, string to = null)
        {
			var model = new AccountsViewModel();
			model.DateRange = new DateRangeViewModel(from, to);
			model.AccountsBalances = _accountService.GetAccountBalances(model.DateRange.From, model.DateRange.To).Result;
            return View(model);
        }

		[GET("Transactions")]
		public virtual ActionResult Transactions(int? accountId = null, string vendorId = "", string categoryId = "", string from = null, string to = null)
		{
			return View();
		}

		[GET("Categories")]
		public virtual ActionResult Categories(int? accountId = null, string from = null, string to = null, string transfers = "")
		{
			return View();
		}

		[GET("Category/{categoryId}")]
		public virtual ActionResult Category(int categoryId)
		{
			return View();
		}

		[GET("Vendors")]
		public virtual ActionResult Vendors(string from = null, string to = null)
		{
			return View();
		}

		[GET("Vendor/{vendorId}")]
		public virtual ActionResult Vendor(int vendorId)
		{
			return View();
		}

		[GET("Bills")]
		public virtual ActionResult Bills()
		{
			return View();
		}

		[GET("Bill/{billId}")]
		public virtual ActionResult Bill(int billId)
		{
			return View();
		}

		[GET("Budget")]
		public virtual ActionResult Budget(int? accountId = null, string month = null, string from = null, string to = null)
		{
			return View();
		}
    }
}

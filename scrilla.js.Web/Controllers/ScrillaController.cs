using scrilla.js.Web.Models;
using scrilla.Services;
using scrilla.Services.Models;
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

		[Route("")]
		[Route("Accounts")]
		public virtual ActionResult Accounts(string from = null, string to = null, string view = "ng")
		{
			view = view.Equals("ng") ? MVC.Scrilla.Views.Accounts_ng : MVC.Scrilla.Views.Accounts;

			var dateRange = new DateRangeModel(from, to);
			var accountBalancesModel = _accountService.GetAccountBalances(dateRange.From, dateRange.To).Result;
			var model = new AccountsViewModel() { AccountBalances = accountBalancesModel.AccountBalances, DateRange = accountBalancesModel.DateRange };

			return View(view, model);
		}

		[Route("Transactions")]
		public virtual ActionResult Transactions(int? accountId = null, string vendorId = "", string categoryId = "", string from = null, string to = null)
		{
			return View();
		}

		[Route("Categories")]
		public virtual ActionResult Categories(int? accountId = null, string from = null, string to = null, string transfers = "")
		{
			return View();
		}

		[Route("Category/{categoryId}")]
		public virtual ActionResult Category(int categoryId)
		{
			return View();
		}

		[Route("Vendors")]
		public virtual ActionResult Vendors(string from = null, string to = null)
		{
			return View();
		}

		[Route("Vendor/{vendorId}")]
		public virtual ActionResult Vendor(int vendorId)
		{
			return View();
		}

		[Route("Bills")]
		public virtual ActionResult Bills()
		{
			return View();
		}

		[Route("Bill/{billId}")]
		public virtual ActionResult Bill(int billId)
		{
			return View();
		}

		[Route("Budget")]
		public virtual ActionResult Budget(int? accountId = null, string month = null, string from = null, string to = null)
		{
			return View();
		}
	}
}

﻿using scrilla.js.Web.Models;
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
		[Route("accounts")]
		[Route("transactions")]
		[Route("vendors")]
		[Route("categories")]
		[Route("bills")]
		[Route("budget")]
		public virtual ActionResult App()
		{
			return View();
		}

		[Route("legacy/accounts")]
		public virtual ActionResult Accounts(string from = null, string to = null)
		{
			var dateRange = new DateRangeModel(from, to);
			var accountBalancesModel = _accountService.GetAccountBalances(dateRange.From, dateRange.To).Result;
			var model = new AccountsViewModel() { AccountBalances = accountBalancesModel.AccountBalances, DateRange = accountBalancesModel.DateRange };

			return View(MVC.Scrilla.Views.Accounts, model);
		}

		/*
		[Route("transactions")]
		public virtual ActionResult Transactions(int? accountId = null, string vendorId = "", string categoryId = "", string from = null, string to = null)
		{
			return View();
		}

		[Route("categories")]
		public virtual ActionResult Categories(int? accountId = null, string from = null, string to = null, string transfers = "")
		{
			return View();
		}

		[Route("category/{categoryId}")]
		public virtual ActionResult Category(int categoryId)
		{
			return View();
		}

		[Route("vendors")]
		public virtual ActionResult Vendors(string from = null, string to = null)
		{
			return View();
		}

		[Route("vendor/{vendorId}")]
		public virtual ActionResult Vendor(int vendorId)
		{
			return View();
		}

		[Route("Bills")]
		public virtual ActionResult Bills()
		{
			return View();
		}

		[Route("bill/{billId}")]
		public virtual ActionResult Bill(int billId)
		{
			return View();
		}

		[Route("budget")]
		public virtual ActionResult Budget(int? accountId = null, string month = null, string from = null, string to = null)
		{
			return View();
		}
		
		 */
	}
}

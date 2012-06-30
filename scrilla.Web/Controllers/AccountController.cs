using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using scrilla.Services;
using AttributeRouting;
using scrilla.Data;
using scrilla.Web.Controllers.ViewModels;
using scrilla.Web.Helpers;
using scrilla.Data.Domain;
using System.IO;
using AttributeRouting.Web.Mvc;

namespace scrilla.Web.Controllers
{
	[ModelStateToTempDataCustom()]
	public partial class AccountController : Controller
	{
		private IAccountService _accountService;
		private ITransactionImporter _transactionImporter;

		public AccountController(IAccountService accountService, ITransactionImporter transactionImporter)
		{
			_accountService = accountService;
			_transactionImporter = transactionImporter;
		}

		[GET("NotFound")]
		public virtual ActionResult NotFound()
		{
			HttpContext.Response.StatusCode = 404;
			return View();
		}

		[HandleError]
		[GET("Error")]
		public virtual ActionResult Error()
		{
			HttpContext.Response.StatusCode = 500;
			return View();
		}


		[GET("Accounts")]
		[GET("")]
		public virtual ActionResult Accounts(string from = null, string to = null)
		{
			var vm = new AccountsViewModel();

			// parse from/to
			DateTime fromDate;
			DateTime toDate;
			if (DateTime.TryParse(from, out fromDate))
			{
				vm.From = fromDate;
				vm.UrlFrom = fromDate;
			}
			else
			{
				vm.From = DefaultFrom();
				vm.UrlFrom = null;
			}

			if (DateTime.TryParse(to, out toDate))
			{
				vm.To = toDate;
				vm.UrlTo = toDate;
			}
			else
			{
				vm.To = DefaultTo();
				vm.UrlTo = null;
			}

			// get accounts
			var accounts = _accountService.GetAllAccounts().Result;

			// get uncategorized transaction counts and monthly balances
			foreach (var account in accounts)
			{
				// set the account group
				var accountGroupBalance = vm.AccountGroupBalances.SingleOrDefault(x => x.AccountGroup == account.AccountGroup);
				if (accountGroupBalance == null)
				{
					accountGroupBalance = new AccountGroupBalance() { AccountGroup = account.AccountGroup };
					vm.AccountGroupBalances.Add(accountGroupBalance);
				}

				// set the account group balance
				AccountBalance balance = new AccountBalance();
				balance.Account = account;
				accountGroupBalance.AccountBalances.Add(balance);

				// TODO should vm.FromMonth be vm.From???
				var initialBalance = account.InitialBalance + _accountService.GetTransactionsByAccount(account.Id, DateTime.MinValue, vm.FromMonth.AddDays(-1)).Result.Sum(x => x.Amount);
				var transactions = _accountService.GetTransactionsByAccount(account.Id, vm.From, vm.To).Result.ToList();
				balance.UncategorizedTransactionCount = _accountService.GetTransactions(account.Id, null).Result.Count();

				for (DateTime month = vm.FromMonth; month <= vm.ToMonth; month = month.AddMonths(1))
				{
					var monthBalance = initialBalance + transactions.Where(x => x.Timestamp < month).Sum(x => x.Amount);
					balance.Balances.Add(month, monthBalance);
				}
			}

			return View(vm);
		}

		[GET("Budget")]
		public virtual ActionResult Budget(int? accountId = null, string month = null, string from = null, string to = null)
		{
			var vm = new BudgetViewModel();

			// parse month
			DateTime monthDate;
			DateTime fromDate;
			DateTime toDate;

			if (DateTime.TryParse(month, out monthDate))
			{
				vm.From = monthDate;
				vm.To = monthDate.AddMonths(1).AddDays(-1);
				vm.UrlMonth = monthDate;
				vm.UrlFrom = null;
				vm.UrlTo = null;
			}
			else
			{
				if (DateTime.TryParse(from, out fromDate))
				{
					vm.ShowBudgetOnly = true;
					vm.From = fromDate;
					vm.UrlFrom = fromDate;
				}
				else
				{
					var now = DateTime.Now.AddMonths(-1);
					vm.From = new DateTime(now.Year, now.Month, 1);
					vm.UrlFrom = null;
				}

				if (DateTime.TryParse(to, out toDate))
				{
					vm.ShowBudgetOnly = true;
					vm.To = toDate;
					vm.UrlTo = toDate;
				}
				else
				{
					vm.To = vm.From.AddMonths(3).AddDays(-1);// DefaultTo();
					vm.UrlTo = null;
				}
			}

			// get accounts
			var accounts = new List<Account>();
			accounts.AddRange(_accountService.GetAllAccounts().Result.OrderBy(x => x.AccountGroup.DisplayOrder).ThenBy(x => x.Name));
			accounts.Insert(0, new Account() { Name = "All Accounts" });
			vm.AvailableAccounts = accounts;

			// get selected account
			if (accountId.HasValue)
			{
				vm.Account = _accountService.GetAccount(accountId.Value).Result;
			}

			// get categories
			var categories = new List<Category>();
			categories.AddRange(_accountService.GetAllCategories().Result.OrderBy(x => x.CategoryGroup.DisplayOrder).ThenBy(x => x.Name));
			categories.Add(new Category() { Name = "None", CategoryGroup = new CategoryGroup() { Name = "Uncategorized", DisplayOrder = Int32.MaxValue } });

			// get budget categories
			var budgetCategories = _accountService.GetBudgetCategories(vm.From, vm.To).Result;

			// get transactions
			var transactions = accountId.HasValue ?
				_accountService.GetTransactionsByAccount(accountId.Value, vm.From, vm.To).Result.ToList() :
				_accountService.GetAllTransactions(vm.From, vm.To).Result.ToList();

			// get projected bill transactions
			var billTransactions = _accountService.GetBillTransactions(null, vm.From, vm.To).Result;

			// get amounts
			var vmBudgetCategories = new List<BudgetCategoryAmount>();
			foreach (var category in categories)
			{
				for (DateTime curMonth = vm.FromMonth; curMonth <= vm.ToMonth; curMonth = curMonth.AddMonths(1))
				{
					var bc = new BudgetCategoryAmount() { Category = category, Month = curMonth };
					vmBudgetCategories.Add(bc);

					// budget amount
					var budgetCategory = budgetCategories.FirstOrDefault(x => x.CategoryId == category.Id && x.Month == curMonth);
					if (budgetCategory != null)
					{
						bc.ExtraBudgetAmount = budgetCategory.Amount;
					}

					// bills for projected amount TODO are the projections using non-budgeted amounts?
					int? categoryId = category.Id == 0 ? null : (int?)category.Id;
					bc.Bills = billTransactions.Where(x => x.CategoryId == categoryId && x.Timestamp >= curMonth && x.Timestamp < curMonth.AddMonths(1)).ToList();

					// actual amount
					bool includeTransfers = true; // TODO is this right?
					var trx = transactions
						.Where(x => x.Timestamp >= curMonth && x.Timestamp < curMonth.AddMonths(1))
						.SelectMany(x => x.Subtransactions)
						.Where(x => x.CategoryId == categoryId && (includeTransfers | !x.IsTransfer));

					bc.ActualAmount = trx.Sum(x => x.Amount);
					bc.NonBudgetAmount = trx.Where(x => x.IsExcludedFromBudget).Sum(x => x.Amount);
				}
			}
			vm.BudgetCategoryAmounts = vmBudgetCategories;

			return View(vm);
		}

		[POST("Budget")]
		public virtual ActionResult SetBudget(DateTime month, int categoryId, decimal amount)
		{
			var result = _accountService.SetBudget(month, categoryId, amount);
			if (result.HasErrors)
				return Json(null);

			return Json(new
			{
				total = (result.Result.BillsAmount + result.Result.ExtraAmount).ToCurrency(),
				extra = result.Result.ExtraAmount.ToCurrency(),
				hasExtra = result.Result.ExtraAmount != 0M
			});
		}

		private DateTime DefaultFrom()
		{
			return DefaultTo().AddDays(1).AddMonths(-6);
		}

		private DateTime DefaultTo()
		{
			var now = DateTime.Now;
			return new DateTime(now.Year, now.Month, 1).AddMonths(2).AddDays(-1);
		}

		#region Transactions

		[GET("Transactions")]
		public virtual ActionResult Transactions(int? accountId = null, string vendorId = "", string categoryId = "", string from = null, string to = null)
		{
			var vm = new TransactionsViewModel();

			// parse categoryId
			int? category = null;
			int tempCategory;
			var isInt = Int32.TryParse(categoryId, out tempCategory);
			if (isInt) category = tempCategory;

			// parse vendorId
			int? vendor = null;
			int tempVendor;
			isInt = Int32.TryParse(vendorId, out tempVendor);
			if (isInt) vendor = tempVendor;

			// parse from/to
			DateTime fromDate;
			DateTime toDate;
			if (DateTime.TryParse(from, out fromDate))
			{
				vm.From = fromDate;
				vm.UrlFrom = fromDate;
			}
			else
			{
				vm.From = DefaultFrom();
				vm.UrlFrom = null;
			}

			if (DateTime.TryParse(to, out toDate))
			{
				vm.To = toDate;
				vm.UrlTo = toDate;
			}
			else
			{
				vm.To = DefaultTo();
				vm.UrlTo = null;
			}

			// account
			if (accountId.HasValue)
				vm.Account = _accountService.GetAccount(accountId.Value).Result;

			// available categories
			var categories = _accountService.GetAllCategories().Result;
			vm.AvailableCategories = categories.OrderBy(x => x.CategoryGroup.DisplayOrder).ThenBy(x => x.Name).Select(x => new CategoryDto() { Id = x.Id, Name = x.CategoryGroup.Name + " : " + x.Name }).ToList();
			vm.AvailableCategories.Insert(0, new CategoryDto());

			// available vendors
			var vendors = _accountService.GetAllVendors().Result;
			vm.AvailableVendors = vendors.OrderBy(x => x.Name).Select(x => new VendorDto() { Id = x.Id, Name = x.Name }).ToList();
			vm.AvailableVendors.Insert(0, new VendorDto());

			// transactions
			IEnumerable<Transaction> transactions = null;

			// TODO what happens if ?categoryId=BREAKTHIS. it's basically the same as ?categoryID=none

			// 1) by account
			if (accountId.HasValue && String.IsNullOrEmpty(vendorId) && String.IsNullOrEmpty(categoryId))
			{
				transactions = _accountService.GetTransactionsByAccount(accountId.Value, vm.From, vm.To).Result;

				// balance (only makes sense if we are in a single account and not a specific category or vendor)
				vm.ShowBalance = true;
				decimal initialBalance = vm.Account.InitialBalance + _accountService.GetTransactionsByAccount(accountId.Value, DateTime.MinValue, vm.From.AddDays(-1)).Result.Sum(x => x.Amount);
				vm.InitialBalance = initialBalance;
				foreach (var trx in transactions.OrderBy(x => x.OriginalTimestamp).ThenByDescending(x => x.Id)) // in reverse order to compute balance
				{
					initialBalance += trx.Amount;
					trx.Balance = initialBalance;
				}
			}

			// 2) by vendor but not category
			else if (!accountId.HasValue && !String.IsNullOrEmpty(vendorId) && String.IsNullOrEmpty(categoryId))
			{
				vm.Vendor = vendors.FirstOrDefault(x => x.Id == vendor);
				transactions = _accountService.GetTransactionsByVendor(vendor, vm.From, vm.To).Result;
				if (vm.Vendor == null)
					vm.Vendor = new Vendor() { Name = "None" };
			}

			// 3) by category
			else if (!accountId.HasValue && !String.IsNullOrEmpty(categoryId))
			{
				vm.Category = categories.FirstOrDefault(x => x.Id == category);
				transactions = _accountService.GetTransactionsByCategory(category, vm.From, vm.To).Result;
				if (vm.Category == null)
					vm.Category = new Category() { Name = "None" };

				// 4) by category and vendor
				if (!String.IsNullOrEmpty(vendorId))
				{
					vm.Vendor = vendors.FirstOrDefault(x => x.Id == vendor);
					transactions = transactions.Where(x => x.VendorId == vendor.Value);
					if (vm.Vendor == null)
						vm.Vendor = new Vendor() { Name = "None" };
				}
			}

			// 5) by account and category (including none category)
			else if (accountId.HasValue && !String.IsNullOrEmpty(categoryId))
			{
				vm.Category = categories.FirstOrDefault(x => x.Id == category);
				transactions = _accountService.GetTransactions(accountId.Value, category, vm.From, vm.To).Result;
				if (vm.Category == null)
					vm.Category = new Category() { Name = "None" };

				// 6) by account and category and vendor
				if (!String.IsNullOrEmpty(vendorId))
				{
					vm.Vendor = vendors.FirstOrDefault(x => x.Id == vendor);
					transactions = transactions.Where(x => x.VendorId == vendor.Value);
					if (vm.Vendor == null)
						vm.Vendor = new Vendor() { Name = "None" };
				}
			}

			// 7) by account and vendor
			else if (accountId.HasValue && !String.IsNullOrEmpty(vendorId) && String.IsNullOrEmpty(categoryId))
			{
				vm.Vendor = _accountService.GetVendor(vendor.Value).Result;
				transactions = _accountService.GetTransactionsByAccount(accountId.Value, vm.From, vm.To).Result;
				transactions = transactions.Where(x => x.VendorId == vendor.Value);
				if (vm.Vendor == null)
					vm.Vendor = new Vendor() { Name = "None" };
			}

			// 8) all transactions
			else
			{
				transactions = _accountService.GetAllTransactions(vm.From, vm.To).Result;
			}

			vm.Transactions = transactions.OrderByDescending(x => x.OriginalTimestamp).ThenBy(x => x.Id); // in forward order to display

			if (ControllerContext.IsChildAction)
				return PartialView(vm);
			else
				return View(vm);
		}

		[POST("Transaction/Reconcile")]
		public virtual ActionResult Reconcile(int transactionId, bool isReconciled)
		{
			_accountService.SetReconciled(transactionId, isReconciled);
			return new EmptyResult();
		}

		[POST("Transaction/ChangeCategory")]
		public virtual ActionResult ChangeCategory(int transactionId, int categoryId)
		{
			_accountService.ChangeCategory(transactionId, categoryId);
			return new EmptyResult();
		}

		[POST("Transaction/ChangeVendor")]
		public virtual ActionResult ChangeVendor(int transactionId, int vendorId)
		{
			_accountService.ChangeVendor(transactionId, vendorId);
			return new EmptyResult();
		}

		[POST("Transaction/ChangeDate")]
		public virtual ActionResult ChangeDate(int transactionId, DateTime date)
		{
			_accountService.ChangeDate(transactionId, date);
			return new EmptyResult();
		}

		#endregion

		#region Categories

		[GET("Categories")]
		public virtual ActionResult Categories(int? accountId = null, string from = null, string to = null, string transfers = "")
		{
			var vm = new CategoriesViewModel();
			vm.CategoryTotals = new List<CategoryTotal>();
			vm.IncludeTransfers = (transfers == "excluded") ? false : true;

			// parse from/to
			DateTime fromDate;
			DateTime toDate;
			if (DateTime.TryParse(from, out fromDate))
			{
				vm.From = fromDate;
				vm.UrlFrom = fromDate;
			}
			else
			{
				vm.From = DefaultFrom();
				vm.UrlFrom = null;
			}

			if (DateTime.TryParse(to, out toDate))
			{
				vm.To = toDate;
				vm.UrlTo = toDate;
			}
			else
			{
				vm.To = DefaultTo();
				vm.UrlTo = null;
			}

			// get accounts
			var accounts = new List<Account>();
			accounts.AddRange(_accountService.GetAllAccounts().Result.OrderBy(x => x.AccountGroup.DisplayOrder).ThenBy(x => x.Name));
			accounts.Insert(0, new Account() { Name = "All Accounts" });
			vm.AvailableAccounts = accounts;

			// get account
			Account account = null;
			if (accountId.HasValue)
			{
				account = _accountService.GetAccount(accountId.Value).Result;
				vm.Account = account;
			}

			// get categories
			var categories = new List<Category>();
			categories.AddRange(_accountService.GetAllCategories().Result.OrderBy(x => x.CategoryGroup.DisplayOrder).ThenBy(x => x.Name));
			categories.Add(new Category() { Name = "None", CategoryGroup = new CategoryGroup() { Name = "Uncategorized", DisplayOrder = Int32.MaxValue } });

			// get category totals
			var transactions = accountId.HasValue ?
				_accountService.GetTransactionsByAccount(accountId.Value, vm.From, vm.To).Result.ToList() :
				_accountService.GetAllTransactions(vm.From, vm.To).Result.ToList();

			foreach (var category in categories)
			{
				var ct = new CategoryTotal() { Category = category, Totals = new Dictionary<DateTime, decimal>(), NonBudgetTotals = new Dictionary<DateTime, decimal>() };
				vm.CategoryTotals.Add(ct);
				for (DateTime month = vm.FromMonth; month <= vm.ToMonth; month = month.AddMonths(1))
				{
					int? categoryId = category.Id == 0 ? null : (int?)category.Id;
					var trx = transactions
						.Where(x => x.Timestamp >= month && x.Timestamp < month.AddMonths(1))
						.SelectMany(x => x.Subtransactions)
						.Where(x => x.CategoryId == categoryId && (vm.IncludeTransfers | !x.IsTransfer));
					var total = trx.Sum(x => x.Amount);
					var nonBudgetTotal = trx.Where(x => x.IsExcludedFromBudget).Sum(x => x.Amount);
					var budgetTotal = total - nonBudgetTotal;
					ct.Totals.Add(month, total);
					ct.NonBudgetTotals.Add(month, nonBudgetTotal);

					// income/expense totals
					if (budgetTotal > 0)
					{
						decimal income = 0M;
						vm.MonthlyIncome.TryGetValue(month, out income);
						vm.MonthlyIncome[month] = income + total - nonBudgetTotal;
					}
					else
					{
						decimal expense = 0M;
						vm.MonthlyExpenses.TryGetValue(month, out expense);
						vm.MonthlyExpenses[month] = expense + total - nonBudgetTotal;
					}
				}
			}

			// balances
			if (accountId.HasValue)
			{
				for (var cur = vm.FromMonth; cur <= vm.ToMonth; cur = cur.AddMonths(1))
				{
					var initialBalance = account.InitialBalance + account.Transactions.Where(x => x.Timestamp < cur).SelectMany(x => x.Subtransactions).Sum(x => x.Amount);
					var finalBalance = initialBalance + account.Transactions.Where(x => x.Timestamp >= cur && x.Timestamp < cur.AddMonths(1)).SelectMany(x => x.Subtransactions).Sum(x => x.Amount);
					vm.MonthlyInitialBalances.Add(cur, initialBalance);
					vm.MonthlyFinalBalances.Add(cur, finalBalance);
				}
			}

			return View(vm);
		}

		[GET("Category/{categoryId}")]
		public virtual ActionResult Category(int categoryId)
		{
			var vm = new CategoryViewModel();

			var category = _accountService.GetCategory(categoryId);
			if (category.HasErrors)
			{
				return new HttpNotFoundResult();
			}

			vm.Category = category.Result;

			return View(vm);
		}

		[ChildActionOnly]
		[GET("Category/Add")]
		public virtual ActionResult AddCategory()
		{
			AddEditCategoryViewModel vm = TempData["AddEditCategoryViewModel"] as AddEditCategoryViewModel ?? new AddEditCategoryViewModel();

			// available category groups
			var categoryGroups = _accountService.GetAllCategoryGroups().Result;
			vm.AvailableCategoryGroups = categoryGroups.OrderBy(x => x.Name).Select(x => new CategoryGroupDto() { Id = x.Id, Name = x.Name }).ToList();

			return PartialView("AddEditCategory", vm);
		}

		[POST("Category/Add")]
		public virtual ActionResult AddCategoryPost(AddEditCategoryViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				TempData["AddEditCategoryViewModel"] = viewModel;
				viewModel.IsError = true;
				return RedirectToAction(MVC.Account.Categories());
			}

			var result = _accountService.AddCategory(viewModel.Name, viewModel.CategoryGroup);

			return RedirectToAction(MVC.Account.Categories());
		}

		[ChildActionOnly]
		[GET("Category/Edit/{categoryId}")]
		public virtual ActionResult EditCategory(int categoryId)
		{
			AddEditCategoryViewModel vm = TempData["AddEditCategoryViewModel"] as AddEditCategoryViewModel;
			if (vm == null)
			{
				var category = _accountService.GetCategory(categoryId).Result;
				vm = new AddEditCategoryViewModel()
				{
					CategoryId = category.Id,
					CategoryGroup = category.CategoryGroupId,
					Name = category.Name
				};
			}
			vm.IsEditMode = true;

			// available category groups
			var categoryGroups = _accountService.GetAllCategoryGroups().Result;
			vm.AvailableCategoryGroups = categoryGroups.OrderBy(x => x.Name).Select(x => new CategoryGroupDto() { Id = x.Id, Name = x.Name }).ToList();

			return PartialView("AddEditCategory", vm);
		}

		[POST("Category/Edit/{categoryId}")]
		public virtual ActionResult EditCategoryPost(AddEditCategoryViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				TempData["AddEditCategoryViewModel"] = viewModel;
				viewModel.IsError = true;
				return RedirectToAction(MVC.Account.Category(viewModel.CategoryId));
			}

			_accountService.UpdateCategory(viewModel.CategoryId, viewModel.Name);

			return RedirectToAction(MVC.Account.Category(viewModel.CategoryId));
		}

		[POST("Category/Delete")]
		public virtual ActionResult DeleteCategory(int categoryId)
		{
			_accountService.DeleteCategory(categoryId);
			return RedirectToAction(MVC.Account.Categories());
		}

		#endregion

		#region Vendors

		[GET("Vendors")]
		public virtual ActionResult Vendors(string from = null, string to = null)
		{
			var vm = new VendorsViewModel();
			vm.VendorTotals = new List<VendorTotal>();

			// parse from/to
			DateTime fromDate;
			DateTime toDate;
			if (DateTime.TryParse(from, out fromDate))
			{
				vm.From = fromDate;
				vm.UrlFrom = fromDate;
			}
			else
			{
				vm.From = DefaultFrom();
				vm.UrlFrom = null;
			}

			if (DateTime.TryParse(to, out toDate))
			{
				vm.To = toDate;
				vm.UrlTo = toDate;
			}
			else
			{
				vm.To = DefaultTo();
				vm.UrlTo = null;
			}

			// get vendors
			var vendors = new List<Vendor>();
			vendors.AddRange(_accountService.GetAllVendors().Result);
			vendors.Add(new Vendor() { Name = "Uncategorized" });

			// get vendor totals
			var transactions = _accountService.GetAllTransactions(vm.From, vm.To).Result;
			foreach (var vendor in vendors)
			{
				var vt = new VendorTotal() { Vendor = vendor, Totals = new Dictionary<DateTime, decimal>() };
				vm.VendorTotals.Add(vt);
				for (DateTime month = vm.FromMonth; month <= vm.ToMonth; month = month.AddMonths(1))
				{
					int? vendorId = vendor.Id == 0 ? null : (int?)vendor.Id;
					var total = transactions.Where(x => x.VendorId == vendorId && x.Timestamp >= month && x.Timestamp < month.AddMonths(1)).Sum(x => x.Amount);
					vt.Totals.Add(month, total);
				}
			}

			vm.VendorTotals = vm.VendorTotals.OrderByDescending(x => x.MostRecentTotal).ThenBy(x => x.MostRecentTotalAmount).ToList();
			return View(vm);
		}

		[GET("Vendor/{vendorId}")]
		public virtual ActionResult Vendor(int vendorId)
		{
			var vm = new VendorViewModel();

			var vendor = _accountService.GetVendor(vendorId);
			if (vendor.HasErrors)
			{
				return new HttpNotFoundResult();
			}

			vm.Vendor = vendor.Result;

			return View(vm);
		}

		[ChildActionOnly]
		[GET("Vendor/Add")]
		public virtual ActionResult AddVendor()
		{
			AddEditVendorViewModel vm = TempData["AddEditVendorViewModel"] as AddEditVendorViewModel ?? new AddEditVendorViewModel();

			// available categories
			var categories = _accountService.GetAllCategories().Result;
			vm.AvailableCategories = categories.OrderBy(x => x.CategoryGroup.DisplayOrder).ThenBy(x => x.Name).Select(x => new CategoryDto() { Id = x.Id, Name = x.CategoryGroup.Name + " : " + x.Name }).ToList();
			vm.AvailableCategories.Insert(0, new CategoryDto());

			return PartialView("AddEditVendor", vm);
		}

		[POST("Vendor/Add")]
		public virtual ActionResult AddVendorPost(AddEditVendorViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				TempData["AddEditVendorViewModel"] = viewModel;
				viewModel.IsError = true;
				return RedirectToAction(MVC.Account.Vendors());
			}

			var result = _accountService.AddVendor(viewModel.Name, (int?)viewModel.DefaultCategory);

			return RedirectToAction(MVC.Account.Vendors());
		}

		[ChildActionOnly]
		[GET("Vendor/Edit/{vendorId}")]
		public virtual ActionResult EditVendor(int vendorId)
		{
			AddEditVendorViewModel vm = TempData["AddEditVendorViewModel"] as AddEditVendorViewModel;
			if (vm == null)
			{
				var vendor = _accountService.GetVendor(vendorId).Result;
				vm = new AddEditVendorViewModel()
				{
					VendorId = vendorId,
					Name = vendor.Name,
					DefaultCategory = vendor.DefaultCategoryId
				};
			}
			vm.IsEditMode = true;

			// available categories
			var categories = _accountService.GetAllCategories().Result;
			vm.AvailableCategories = categories.OrderBy(x => x.CategoryGroup.DisplayOrder).ThenBy(x => x.Name).Select(x => new CategoryDto() { Id = x.Id, Name = x.CategoryGroup.Name + " : " + x.Name }).ToList();
			vm.AvailableCategories.Insert(0, new CategoryDto());

			return PartialView("AddEditVendor", vm);
		}

		[POST("Vendor/Edit/{vendorId}")]
		public virtual ActionResult EditVendorPost(AddEditVendorViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				TempData["AddEditVendorViewModel"] = viewModel;
				viewModel.IsError = true;
				return RedirectToAction(MVC.Account.Vendor(viewModel.VendorId));
			}

			var vendorResult = _accountService.UpdateVendor(viewModel.VendorId, viewModel.Name, viewModel.DefaultCategory, viewModel.UpdateUncategorizedTransactions);

			return RedirectToAction(MVC.Account.Vendor(vendorResult.Result.Id));
		}

		[POST("Vendor/Delete")]
		public virtual ActionResult DeleteVendor(int vendorId)
		{
			_accountService.DeleteVendor(vendorId);
			return RedirectToAction(MVC.Account.Vendors());
		}

		[POST("Vendor/Mapping/Delete")]
		public virtual ActionResult DeleteVendorMap(int vendorMapId)
		{
			_accountService.DeleteVendorMap(vendorMapId);
			return RedirectToAction(MVC.Account.Vendors());
		}

		#endregion

		#region Bills

		[GET("Bills")]
		public virtual ActionResult Bills()
		{
			var vm = new BillsViewModel();

			vm.BillGroups = _accountService.GetAllBillGroups().Result.OrderBy(x => x.DisplayOrder);

			return View(vm);
		}

		[GET("Bill/{billId}")]
		public virtual ActionResult Bill(int billId)
		{
			var vm = new BillViewModel();

			var bill = _accountService.GetBill(billId);
			if (bill.HasErrors)
			{
				return new HttpNotFoundResult();
			}

			// predictions
			var predictionCutoff = DateTime.Now.AddDays(10);
			foreach (var billTransaction in bill.Result.BillTransactions)
			{
				if (billTransaction.Timestamp < predictionCutoff && (!billTransaction.IsPaid || !billTransaction.Transactions.Any()))
				{
					var billPredictionResult = _accountService.PredictBillTransactionMatch(billTransaction.Id);
					if (!billPredictionResult.HasErrors)
						billTransaction.TransactionPredictions = billPredictionResult.Result;
				}
			}

			vm.Bill = bill.Result;

			return View(vm);
		}

		[ChildActionOnly]
		[GET("BillTransactions")]
		public virtual ActionResult BillTransactions(int? billId, string from = null, string to = null)
		{
			BillTransactionsViewModel vm = new BillTransactionsViewModel();

			// parse from/to
			DateTime fromDate;
			DateTime toDate;
			if (DateTime.TryParse(from, out fromDate))
			{
				vm.From = fromDate;
				vm.UrlFrom = fromDate;
			}
			else
			{
				vm.From = DateTime.MinValue;
				vm.UrlFrom = null;
			}

			if (DateTime.TryParse(to, out toDate))
			{
				vm.To = toDate;
				vm.UrlTo = toDate;
			}
			else
			{
				vm.To = DateTime.MaxValue;
				vm.UrlTo = null;
			}

			// get bill transactions
			vm.BillTransactions = _accountService.GetBillTransactions(billId, vm.From, vm.To).Result;

			return PartialView(vm);
		}

		[ChildActionOnly]
		[GET("Bill/Add")]
		public virtual ActionResult AddBill()
		{
			AddEditBillViewModel vm = TempData["AddEditBillViewModel"] as AddEditBillViewModel ?? new AddEditBillViewModel();

			// available bill groups
			var billGroups = _accountService.GetAllBillGroups().Result;
			vm.AvailableBillGroups = billGroups.OrderBy(x => x.Name).Select(x => new BillGroupDto() { Id = x.Id, Name = x.Name }).ToList();

			// available categories
			var categories = _accountService.GetAllCategories().Result;
			vm.AvailableCategories = categories.OrderBy(x => x.CategoryGroup.DisplayOrder).ThenBy(x => x.Name).Select(x => new CategoryDto() { Id = x.Id, Name = x.CategoryGroup.Name + " : " + x.Name }).ToList();
			vm.AvailableCategories.Insert(0, new CategoryDto());

			// available vendors
			var vendors = _accountService.GetAllVendors().Result;
			vm.AvailableVendors = vendors.OrderBy(x => x.Name).Select(x => new VendorDto() { Id = x.Id, Name = x.Name }).ToList();
			vm.AvailableVendors.Insert(0, new VendorDto());

			return PartialView("AddEditBill", vm);
		}

		[POST("Bill/Add")]
		public virtual ActionResult AddBillPost(AddEditBillViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				TempData["AddEditBillViewModel"] = viewModel;
				viewModel.IsError = true;
				return RedirectToAction(MVC.Account.Bills());
			}

			var result = _accountService.AddBill(viewModel.Name, viewModel.Amount, viewModel.BillGroupId, viewModel.CategoryId, viewModel.VendorId, viewModel.StartDate, viewModel.EndDate, viewModel.Frequency, viewModel.SecondaryStartDate, viewModel.SecondaryEndDate);

			return RedirectToAction(MVC.Account.Bills());
		}

		[ChildActionOnly]
		[GET("Bill/Edit/{billId}")]
		public virtual ActionResult EditBill(int billId)
		{
			AddEditBillViewModel vm = TempData["AddEditBillViewModel"] as AddEditBillViewModel;
			if (vm == null)
			{
				var bill = _accountService.GetBill(billId).Result;
				vm = new AddEditBillViewModel()
				{
					BillId = billId,
					BillGroupId = bill.BillGroupId,
					Name = bill.Name,
					Amount = bill.Amount,
					StartDate = bill.StartDate,
					EndDate = bill.EndDate,
					CategoryId = bill.CategoryId.HasValue ? bill.CategoryId.Value : 0,
					VendorId = bill.VendorId.HasValue ? bill.VendorId.Value : 0,
					Frequency = bill.RecurrenceFrequency,
					SecondaryStartDate = bill.StartDate2,
					SecondaryEndDate = bill.EndDate2,
					IncludeSecondaryDates = bill.StartDate2.HasValue
				};
			}
			vm.IsEditMode = true;

			// available bill groups
			var billGroups = _accountService.GetAllBillGroups().Result;
			vm.AvailableBillGroups = billGroups.OrderBy(x => x.Name).Select(x => new BillGroupDto() { Id = x.Id, Name = x.Name }).ToList();

			// available categories
			var categories = _accountService.GetAllCategories().Result;
			vm.AvailableCategories = categories.OrderBy(x => x.CategoryGroup.DisplayOrder).ThenBy(x => x.Name).Select(x => new CategoryDto() { Id = x.Id, Name = x.CategoryGroup.Name + " : " + x.Name }).ToList();
			vm.AvailableCategories.Insert(0, new CategoryDto());

			// available vendors
			var vendors = _accountService.GetAllVendors().Result;
			vm.AvailableVendors = vendors.OrderBy(x => x.Name).Select(x => new VendorDto() { Id = x.Id, Name = x.Name }).ToList();
			vm.AvailableVendors.Insert(0, new VendorDto());

			return PartialView("AddEditBill", vm);
		}

		[POST("Bill/Edit/{billId}")]
		public virtual ActionResult EditBillPost(AddEditBillViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				TempData["AddEditBillViewModel"] = viewModel;
				viewModel.IsError = true;
				return RedirectToAction(MVC.Account.Bill(viewModel.BillId));
			}

			_accountService.UpdateBill(viewModel.BillId, viewModel.Name, viewModel.Amount, viewModel.BillGroupId, viewModel.CategoryId, viewModel.VendorId, viewModel.StartDate, viewModel.EndDate, viewModel.Frequency, viewModel.UpdateExisting, viewModel.IncludeSecondaryDates ? viewModel.SecondaryStartDate : null, viewModel.IncludeSecondaryDates ? viewModel.SecondaryEndDate : null);

			return RedirectToAction(MVC.Account.Bill(viewModel.BillId));
		}

		[POST("Bill/Delete")]
		public virtual ActionResult DeleteBill(int billId)
		{
			_accountService.DeleteBill(billId);
			return RedirectToAction(MVC.Account.Bills());
		}

		[POST("BillTransaction/Paid")]
		public virtual ActionResult EditBillTransactionPaidStatus(int billTransactionId, bool isPaid, int? transactionId = null)
		{
			_accountService.UpdateBillTransaction(billTransactionId, null, null, isPaid, transactionId);
			return new EmptyResult();
		}

		[POST("BillTransaction")]
		public virtual ActionResult EditBillTransactionPost(EditBillTransactionViewModel viewModel)
		{
			_accountService.UpdateBillTransaction(viewModel.BillTransactionId, viewModel.Amount == 0M ? null : (decimal?)viewModel.Amount, viewModel.ParsedDate, viewModel.IsPaid, viewModel.TransactionId);
			return new EmptyResult();
		}

		#endregion

		public virtual ActionResult UploadTransactions(HttpPostedFileBase file)
		{
			if (file != null && file.ContentLength > 0)
			{
				try
				{
					var fileName = Path.GetFileName(file.FileName);
					var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
					file.SaveAs(path);

					// TODO async!
					_transactionImporter.Import(path);
				}
				catch (Exception ex)
				{
					throw;
				}
			}

			return RedirectToAction(MVC.Account.Accounts());
		}
	}
}

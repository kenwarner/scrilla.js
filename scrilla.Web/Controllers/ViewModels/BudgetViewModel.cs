using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using scrilla.Data;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class BudgetViewModel
	{
		public BudgetViewModel()
		{
			AvailableAccounts = new List<Account>();
			BudgetCategoryAmounts = new List<BudgetCategoryAmount>();
		}

		public Account Account { get; set; }
		public IEnumerable<Account> AvailableAccounts { get; set; }
		public IEnumerable<BudgetCategoryAmount> BudgetCategoryAmounts { get; set; }

		public bool ShowBudgetOnly { get; set; }

		public DateTime? UrlMonth { get; set; }
		public DateTime? UrlFrom { get; set; }
		public DateTime? UrlTo { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }

		public DateTime FromMonth
		{
			get
			{
				return new DateTime(From.Year, From.Month, 1);
			}
		}

		public DateTime ToMonth
		{
			get
			{
				return new DateTime(To.Year, To.Month, 1);
			}
		}
	}

	public class BudgetCategoryAmount
	{
		public DateTime Month { get; set; }
		public Category Category { get; set; }
		public IEnumerable<BillTransaction> Bills { get; set; }
		public decimal NonBudgetAmount { get; set; }
		public decimal ExtraBudgetAmount { get; set; }
		public decimal ActualAmount { get; set; }

		public decimal BillsBudgetAmount
		{
			get
			{
				decimal billsAmount = 0M;
				if (Bills != null)
				{
					billsAmount = Bills.Sum(x => x.Amount);
				}

				return billsAmount;
			}
		}

		public decimal TotalBudgetAmount
		{
			get
			{
				return BillsBudgetAmount + ExtraBudgetAmount;
			}
		}

		public decimal ProjectedAmount
		{
			get
			{
				decimal unpaidBillsAmount = 0M;
				if (Bills != null)
				{
					unpaidBillsAmount = Bills.Where(x => !x.IsPaid).Sum(x => x.Amount);
				}

				return ActualAmount - NonBudgetAmount + unpaidBillsAmount;
			}
		}
	}

}
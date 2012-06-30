using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using scrilla.Data;
using scrilla.Data.Domain;

namespace scrilla.Web.Controllers.ViewModels
{
	public class CategoriesViewModel
	{
		public CategoriesViewModel()
		{
			MonthlyInitialBalances = new Dictionary<DateTime, decimal>();
			MonthlyIncome = new Dictionary<DateTime, decimal>();
			MonthlyExpenses = new Dictionary<DateTime, decimal>();
			MonthlyFinalBalances = new Dictionary<DateTime, decimal>();
		}

		public bool IncludeTransfers { get; set; }
		public Account Account { get; set; }
		public IEnumerable<Account> AvailableAccounts { get; set; }
		public List<CategoryTotal> CategoryTotals { get; set; }
		public Dictionary<DateTime, decimal> MonthlyInitialBalances { get; set; }
		public Dictionary<DateTime, decimal> MonthlyIncome { get; set; }
		public Dictionary<DateTime, decimal> MonthlyExpenses { get; set; }
		public Dictionary<DateTime, decimal> MonthlyFinalBalances { get; set; }

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

	public class CategoryTotal
	{
		public Category Category { get; set; }
		public Dictionary<DateTime, decimal> Totals { get; set; }
		public Dictionary<DateTime, decimal> NonBudgetTotals { get; set; }
	}
}
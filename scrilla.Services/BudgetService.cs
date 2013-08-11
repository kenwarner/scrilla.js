using DapperExtensions;
using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public class BudgetService : IBudgetService
	{
		private readonly IDatabase _db;

		public BudgetService(IDatabase database)
		{
			_db = database;
		}

		public ServiceResult<IEnumerable<BudgetCategory>> GetBudgetCategories(DateTime? from = null, DateTime? to = null)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<IEnumerable<BudgetCategory>>();

			//var budgetCategories = _budgetCategoryRepository.GetAll();

			//if (from.HasValue) budgetCategories = budgetCategories.Where(x => x.Month >= from.Value);
			//if (to.HasValue) budgetCategories = budgetCategories.Where(x => x.Month <= to.Value);

			//result.Result = budgetCategories.ToList();
			return result;
		}

		public ServiceResult<BudgetAmountInfo> UpdateBudget(DateTime month, int categoryId, decimal amount)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<BudgetAmountInfo>();

			//var budget = _budgetCategoryRepository.Get(x => x.Month == month && x.CategoryId == categoryId);
			//if (budget == null)
			//{
			//	budget = new BudgetCategory() { Month = month, CategoryId = categoryId };
			//	_budgetCategoryRepository.Add(budget);
			//}

			//var nextMonth = month.AddMonths(1);
			//var bills = _billTransactionRepository.GetMany(x => x.CategoryId == categoryId && x.Timestamp >= month && x.Timestamp < nextMonth); // TODO paid vs !paid
			//var sumBills = bills.Any() ? bills.Sum(x => x.Amount) : 0M;

			//budget.Amount = amount;
			//_unitOfWork.Commit();

			//result.Result = new BudgetAmountInfo() { Month = month, CategoryId = categoryId, ExtraAmount = amount, BillsAmount = sumBills };

			return result;
		}


	}

	public class BudgetAmountInfo
	{
		public DateTime Month { get; set; }
		public int CategoryId { get; set; }
		public decimal BillsAmount { get; set; }
		public decimal ExtraAmount { get; set; }
	}
}

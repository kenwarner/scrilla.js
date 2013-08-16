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
		private ICategoryService _categoryService;

		public BudgetService(IDatabase database, ICategoryService categoryService)
		{
			_db = database;
			_categoryService = categoryService;
		}

		public ServiceResult<IEnumerable<BudgetCategory>> GetBudgetCategories(DateTime? from = null, DateTime? to = null)
		{
			var result = new ServiceResult<IEnumerable<BudgetCategory>>();

			var predicates = new List<IPredicate>();

			if (from.HasValue)
				predicates.Add(Predicates.Field<BudgetCategory>(x => x.Month, Operator.Ge, from.Value));

			if (to.HasValue)
				predicates.Add(Predicates.Field<BudgetCategory>(x => x.Month, Operator.Le, to.Value));

			object predicate = !predicates.Any() ?
				null : (predicates.Count == 1 ?
					predicates.First() :
					new PredicateGroup { Operator = GroupOperator.And, Predicates = predicates });

			result.Result = _db.GetList<BudgetCategory>(predicate);
			return result;
		}

		public ServiceResult<bool> DeleteBudgetCategory(int categoryId, DateTime month)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();
			bool deletionResult = false;

			// does category exist?
			var categoryResult = _categoryService.GetCategory(categoryId);
			if (categoryResult.HasErrors)
			{
				result.AddErrors(categoryResult);
				return result;
			}

			// does budget category exist?
			var predicates = new List<IPredicate>();
			predicates.Add(Predicates.Field<BudgetCategory>(x => x.Month, Operator.Eq, month));
			predicates.Add(Predicates.Field<BudgetCategory>(x => x.CategoryId, Operator.Eq, categoryId));
			var predicate = new PredicateGroup { Operator = GroupOperator.And, Predicates = predicates };
			var budgetCategory = _db.GetList<BudgetCategory>(predicate);

			// are there multiple budget categories with the same month?
			if (budgetCategory.Count() > 1)
			{
				result.AddError(ErrorType.Generic, "Multiple Budget Categories for month {0} exist", month.ToShortDateString());
				return result;
			}

			// is this an existing budget category?
			else if (budgetCategory.Count() == 1)
			{
				var existingBudgetCategory = budgetCategory.First();
				deletionResult = _db.Delete<BudgetCategory>(existingBudgetCategory);
			}

			result.Result = deletionResult;
			return result;
		}

		public ServiceResult<BudgetAmountInfo> UpdateBudget(int categoryId, DateTime month, decimal amount)
		{
			var result = new ServiceResult<BudgetAmountInfo>();
			BudgetAmountInfo budgetAmountInfo = new BudgetAmountInfo() { CategoryId = categoryId, Month = month, ExtraAmount = amount };

			// does category exist?
			var categoryResult = _categoryService.GetCategory(categoryId);
			if (categoryResult.HasErrors)
			{
				result.AddErrors(categoryResult);
				return result;
			}

			// does budget category exist?
			var predicates = new List<IPredicate>();
			predicates.Add(Predicates.Field<BudgetCategory>(x => x.Month, Operator.Eq, month));
			predicates.Add(Predicates.Field<BudgetCategory>(x => x.CategoryId, Operator.Eq, categoryId));
			var predicate = new PredicateGroup { Operator = GroupOperator.And, Predicates = predicates };
			var budgetCategory = _db.GetList<BudgetCategory>(predicate);

			// are there multiple budget categories with the same month?
			if (budgetCategory.Count() > 1)
			{
				result.AddError(ErrorType.Generic, "Multiple Budget Categories for month {0} exist", month.ToShortDateString());
				return result;
			}

			// is this an existing budget category?
			else if (budgetCategory.Count() == 1)
			{
				var existingBudgetCategory = budgetCategory.First();
				existingBudgetCategory.Amount = amount;
				_db.Update<BudgetCategory>(existingBudgetCategory);
			}

			// is this a new budget category?
			else
			{
				var newBudgetCategory = new BudgetCategory()
				{
					CategoryId = categoryId,
					Month = month,
					Amount = amount
				};

				_db.Insert<BudgetCategory>(newBudgetCategory);
			}

			// what is the bills amount for this month?
			var nextMonth = month.AddMonths(1);
			predicates = new List<IPredicate>();
			predicates.Add(Predicates.Field<BillTransaction>(x => x.CategoryId, Operator.Eq, categoryId));
			predicates.Add(Predicates.Field<BillTransaction>(x => x.Timestamp, Operator.Ge, month));
			predicates.Add(Predicates.Field<BillTransaction>(x => x.Timestamp, Operator.Lt, nextMonth));
			predicate = new PredicateGroup { Operator = GroupOperator.And, Predicates = predicates };
			var billTransactions = _db.GetList<BillTransaction>(predicate);
			var billsAmount = billTransactions.Any() ? billTransactions.Sum(x => x.Amount) : 0M;
			budgetAmountInfo.BillsAmount = billsAmount;

			result.Result = budgetAmountInfo;
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

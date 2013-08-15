using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public interface IBudgetService
	{
		ServiceResult<IEnumerable<BudgetCategory>> GetBudgetCategories(DateTime? from = null, DateTime? to = null);
		ServiceResult<BudgetCategory> DeleteBudgetCategory(DateTime month, int categoryId);
		ServiceResult<BudgetAmountInfo> UpdateBudget(DateTime month, int categoryId, decimal amount);
	}
}

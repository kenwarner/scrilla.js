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
		ServiceResult<bool> DeleteBudgetCategory(int categoryId, DateTime month);
		ServiceResult<BudgetAmountInfo> UpdateBudget(int categoryId, DateTime month, decimal amount);
	}
}

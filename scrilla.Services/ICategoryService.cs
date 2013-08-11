using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public interface ICategoryService
	{
		ServiceResult<Category> GetCategory(int categoryId);

		ServiceResult<CategoryGroup> GetCategoryGroup(int categoryGroupId);

		ServiceResult<IEnumerable<Category>> GetAllCategories();
		ServiceResult<IEnumerable<CategoryGroup>> GetAllCategoryGroups();


		ServiceResult<Category> AddCategory(string name, int? categoryGroupId = null);
		ServiceResult<CategoryGroup> AddCategoryGroup(string name, int displayOrder = 0);

		ServiceResult<bool> DeleteCategory(int categoryId);
		ServiceResult<bool> DeleteCategoryGroup(int categoryGroupId);

		// TODO need a categoryGroupId param on UpdateCategory
		ServiceResult<Category> UpdateCategoryName(int categoryId, string name);
	}
}

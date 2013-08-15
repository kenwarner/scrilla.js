using DapperExtensions;
using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public class CategoryService : EntityService, ICategoryService
	{
		public CategoryService(IDatabase database)
			: base(database) { }

		public ServiceResult<Category> GetCategory(int categoryId)
		{
			return base.GetEntity<Category>(categoryId);
		}

		public ServiceResult<CategoryGroup> GetCategoryGroup(int categoryGroupId)
		{
			return base.GetEntity<CategoryGroup>(categoryGroupId);
		}

		public ServiceResult<IEnumerable<Category>> GetAllCategories()
		{
			return base.GetAllEntity<Category>();
		}

		public ServiceResult<IEnumerable<CategoryGroup>> GetAllCategoryGroups()
		{
			return base.GetAllEntity<CategoryGroup>();
		}


		public ServiceResult<Category> AddCategory(string name, int? categoryGroupId = null)
		{
			var result = new ServiceResult<Category>();

			// does Category Group exist?
			if (categoryGroupId.HasValue)
			{
				var categoryGroupResult = GetCategoryGroup(categoryGroupId.Value);
				if (categoryGroupResult.HasErrors)
				{
					result.AddErrors(categoryGroupResult);
					return result;
				}
			}

			// create Category
			var category = new Category()
			{
				Name = name,
				CategoryGroupId = categoryGroupId,
			};

			_db.Insert<Category>(category);

			result.Result = category;
			return result;
		}

		public ServiceResult<CategoryGroup> AddCategoryGroup(string name, int displayOrder = 0)
		{
			var result = new ServiceResult<CategoryGroup>();

			// create Category Group
			var categoryGroup = new CategoryGroup()
			{
				Name = name,
				DisplayOrder = displayOrder
				// TOOD we need a better way to reorder things
			};

			_db.Insert<CategoryGroup>(categoryGroup);

			result.Result = categoryGroup;
			return result;
		}

		public ServiceResult<bool> DeleteCategory(int categoryId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<Category>(Predicates.Field<Category>(x => x.Id, Operator.Eq, categoryId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Category {0} not found", categoryId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		public ServiceResult<bool> DeleteCategoryGroup(int categoryGroupId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<CategoryGroup>(Predicates.Field<CategoryGroup>(x => x.Id, Operator.Eq, categoryGroupId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "CategoryGroup {0} not found", categoryGroupId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		public ServiceResult<Category> UpdateCategoryName(int categoryId, string name)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<Category>();

			//var category = _categoryRepository.GetById(categoryId);
			//if (category == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Category {0} not found", categoryId);
			//	return result;
			//}

			//category.Name = name;
			//_unitOfWork.Commit();

			//result.Result = category;
			return result;
		}


	}
}

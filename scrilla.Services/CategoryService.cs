using Dapper;
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

		public ServiceResult<Category> GetCategory(string name)
		{
			var result = new ServiceResult<Category>();

			var category = _db.Connection.Query<Category>("SELECT * FROM Category WHERE Name = @name", new { name });
			if (category == null || !category.Any())
			{
				result.AddError(ErrorType.NotFound, "Category {0} not found", name);
				return result;
			}

			// are there multiple categories with the same name?
			if (category.Count() > 1)
			{
				result.AddError(ErrorType.Generic, "Multiple Categories with name {0} exist", name);
				return result;
			}

			result.Result = category.First();
			return result;
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
			var result = new ServiceResult<bool>();

			var categoryResult = GetCategory(categoryId);
			if (categoryResult.HasErrors)
			{
				result.AddErrors(categoryResult);
				return result;
			}

			// unassign other entities that use this category
			_db.Connection.Execute("UPDATE Account SET DefaultCategoryId = NULL WHERE DefaultCategoryId = @categoryId", new { categoryId });
			_db.Connection.Execute("UPDATE Bill SET CategoryId = NULL WHERE CategoryId = @categoryId", new { categoryId });
			_db.Connection.Execute("UPDATE BillTransaction SET CategoryId = NULL WHERE CategoryId = @categoryId", new { categoryId });
			_db.Connection.Execute("UPDATE BillTransaction SET OriginalCategoryId = NULL WHERE OriginalCategoryId = @categoryId", new { categoryId });
			_db.Connection.Execute("UPDATE BudgetCategory SET CategoryId = NULL WHERE CategoryId = @categoryId", new { categoryId });
			_db.Connection.Execute("UPDATE Subtransaction SET CategoryId = NULL WHERE CategoryId = @categoryId", new { categoryId });
			_db.Connection.Execute("UPDATE Vendor SET DefaultCategoryId = NULL WHERE DefaultCategoryId = @categoryId", new { categoryId });

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
			var result = new ServiceResult<Category>();
			Category resultCategory;

			// does category exist?
			var categoryResult = GetCategory(categoryId);
			if (categoryResult.HasErrors)
			{
				result.AddErrors(categoryResult);
				return result;
			}

			// does a category with this name already exist?
			var existingCategoryResult = GetCategory(name);
			if (existingCategoryResult.Result == null)
			{
				// just rename the original category
				categoryResult.Result.Name = name;
				_db.Update<Category>(categoryResult.Result);
				resultCategory = categoryResult.Result;
			}
			else
			{
				// Bill, BillTransaction, BudgetCategory, Subtransaction, Account, Vendor
				_db.Connection.Execute("UPDATE Bill SET CategoryId = @existingCategoryId WHERE CategoryId = @categoryId", new { existingCategoryId = existingCategoryResult.Result.Id, categoryId = categoryResult.Result.Id });
				_db.Connection.Execute("UPDATE BillTransaction SET CategoryId = @existingCategoryId WHERE CategoryId = @categoryId", new { existingCategoryId = existingCategoryResult.Result.Id, categoryId = categoryResult.Result.Id });
				_db.Connection.Execute("UPDATE BillTransaction SET OriginalCategoryId = @existingCategoryId WHERE OriginalCategoryId = @categoryId", new { existingCategoryId = existingCategoryResult.Result.Id, categoryId = categoryResult.Result.Id });
				_db.Connection.Execute("UPDATE BudgetCategory SET CategoryId = @existingCategoryId WHERE CategoryId = @categoryId", new { existingCategoryId = existingCategoryResult.Result.Id, categoryId = categoryResult.Result.Id });
				_db.Connection.Execute("UPDATE Subtransaction SET CategoryId = @existingCategoryId WHERE CategoryId = @categoryId", new { existingCategoryId = existingCategoryResult.Result.Id, categoryId = categoryResult.Result.Id });
				_db.Connection.Execute("UPDATE [Account] SET DefaultCategoryId = @existingCategoryId WHERE DefaultCategoryId = @categoryId", new { existingCategoryId = existingCategoryResult.Result.Id, categoryId = categoryResult.Result.Id });
				_db.Connection.Execute("UPDATE Vendor SET DefaultCategoryId = @existingCategoryId WHERE DefaultCategoryId = @categoryId", new { existingCategoryId = existingCategoryResult.Result.Id, categoryId = categoryResult.Result.Id });

				// delete the original category
				var deleteCategoryResult = DeleteCategory(categoryResult.Result.Id);
				if (deleteCategoryResult.HasErrors)
				{
					result.AddErrors(deleteCategoryResult);
					return result;
				}

				resultCategory = existingCategoryResult.Result;
			}

			result.Result = resultCategory;
			return result;
		}
	}
}

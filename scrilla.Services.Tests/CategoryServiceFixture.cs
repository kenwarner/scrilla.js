using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace scrilla.Services.Tests
{
	public class CategoryServiceFixture : BaseFixture<CategoryService>
	{
		[Fact]
		public void GetCategory_ExistingCategory_WithCategoryGroup()
		{
			var categoryName = "test category";
			var categoryGroupName = "test category group";

			// create test category group
			var addCategoryGroupResult = _sut.AddCategoryGroup(categoryGroupName);
			Assert.False(addCategoryGroupResult.HasErrors);
			Assert.Equal(categoryGroupName, addCategoryGroupResult.Result.Name);

			// create test category
			var addCategoryResult = _sut.AddCategory(categoryName, addCategoryGroupResult.Result.Id);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);
			Assert.Equal(addCategoryGroupResult.Result.Id, addCategoryResult.Result.CategoryGroupId);

			// act
			var result = _sut.GetCategory(addCategoryResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(categoryName, result.Result.Name);
			Assert.Equal(addCategoryGroupResult.Result.Id, result.Result.CategoryGroupId);

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
			_sut.DeleteCategoryGroup(addCategoryGroupResult.Result.Id);
		}

		[Fact]
		public void GetCategory_ExistingCategory_WithNullCategoryGroup()
		{
			var categoryName = "test category";
			int? categoryGroupId = null;

			// create test category
			var addCategoryResult = _sut.AddCategory(categoryName, categoryGroupId);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);
			Assert.Equal(categoryGroupId, addCategoryResult.Result.CategoryGroupId);

			// act
			var result = _sut.GetCategory(addCategoryResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(categoryName, result.Result.Name);
			Assert.Equal(categoryGroupId, result.Result.CategoryGroupId);

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetCategory_NonExistantCategory()
		{
			var nonExistantCategoryId = -1;

			// act
			var result = _sut.GetCategory(nonExistantCategoryId);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetCategory_ExistingName()
		{
			var categoryName = "test category";

			// create test category
			var addCategoryResult = _sut.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// act
			var result = _sut.GetCategory(categoryName);
			Assert.False(result.HasErrors);
			Assert.Equal(categoryName, result.Result.Name);

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetCategory_NonExistantName()
		{
			var nonExistantCategory = "nonexistant category";

			// act
			var result = _sut.GetCategory(nonExistantCategory);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetCategoryGroup_ExistingName()
		{
			var categoryGroupName = "test category group";

			// create test category group
			var addCategoryGroupResult = _sut.AddCategoryGroup(categoryGroupName);
			Assert.False(addCategoryGroupResult.HasErrors);

			// act
			var result = _sut.GetCategoryGroup(addCategoryGroupResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(categoryGroupName, result.Result.Name);

			// cleanup
			_sut.DeleteCategoryGroup(result.Result.Id);
		}

		[Fact]
		public void GetCategoryGroup_NonExistantName()
		{
			var nonExistantCategoryGroup = -1;

			// act
			var result = _sut.GetCategoryGroup(nonExistantCategoryGroup);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetAllCategories_OneCategoryExistsAfterAddingACategory()
		{
			var categoriesResult = _sut.GetAllCategories();
			Assert.False(categoriesResult.HasErrors);
			Assert.Empty(categoriesResult.Result);

			// create test category
			var categoryName = "test category";
			var addCategoryResult = _sut.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// act
			categoriesResult = _sut.GetAllCategories();
			Assert.False(categoriesResult.HasErrors);
			Assert.Equal(1, categoriesResult.Result.Count());

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetAllCategoryGroups_NotImplemented()
		{
			var categoryGroupsResult = _sut.GetAllCategoryGroups();
			Assert.False(categoryGroupsResult.HasErrors);
			Assert.Empty(categoryGroupsResult.Result);

			// create test category group
			var categoryGroupName = "test category group";
			var addCategoryGroupResult = _sut.AddCategoryGroup(categoryGroupName);
			Assert.False(addCategoryGroupResult.HasErrors);

			// act
			categoryGroupsResult = _sut.GetAllCategoryGroups();
			Assert.False(categoryGroupsResult.HasErrors);
			Assert.Equal(1, categoryGroupsResult.Result.Count());

			// cleanup
			_sut.DeleteCategoryGroup(addCategoryGroupResult.Result.Id);
		}

		[Fact]
		public void AddCategory_NullCategoryGroupId()
		{
			var categoryName = "test category";
			int? categoryGroupId = null;

			// act
			var addCategoryResult = _sut.AddCategory(categoryName, categoryGroupId);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);
			Assert.Equal(categoryGroupId, addCategoryResult.Result.CategoryGroupId);

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void AddCategory_NonExistantCategoryGroupId()
		{
			var categoryName = "test category";
			int? nonExistantCategoryGroupId = -1;

			// act
			var addCategoryResult = _sut.AddCategory(categoryName, nonExistantCategoryGroupId);
			Assert.True(addCategoryResult.HasErrors);
		}


		[Fact]
		public void AddCategory_ExistingCategoryGroupId()
		{
			var categoryName = "test category";
			var categoryGroupName = "test category group";

			// add test category group
			var addCategoryGroupResult = _sut.AddCategoryGroup(categoryGroupName);
			Assert.False(addCategoryGroupResult.HasErrors);

			// act
			var addCategoryResult = _sut.AddCategory(categoryName, addCategoryGroupResult.Result.Id);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);
			Assert.Equal(addCategoryGroupResult.Result.Id, addCategoryResult.Result.CategoryGroupId);

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
			_sut.DeleteCategoryGroup(addCategoryGroupResult.Result.Id);
		}

		[Fact]
		public void AddCategoryGroup_NotImplemented()
		{
			throw new NotImplementedException();
		}


		[Fact]
		public void DeleteCategory_NotImplemented()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void DeleteCategoryGroup_NotImplemented()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void UpdateCategoryName_ExistingCategory_WithDistinctNewName()
		{
			var categoryName = "test category";
			var newCategoryName = "new test category";

			// add test category
			var addCategoryResult = _sut.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// act
			var updateCategoryNameResult = _sut.UpdateCategoryName(addCategoryResult.Result.Id, newCategoryName);
			Assert.False(updateCategoryNameResult.HasErrors);

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void UpdateCategoryName_ExistingCategory_WithExistingNewName()
		{
			// TODO make sure all the associations are cleaned up

			var existingCategoryName = "existing test category";
			var categoryName = "test category";

			// add test category
			var addCategoryResult = _sut.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// add existing test category
			var addExistingCategoryResult = _sut.AddCategory(existingCategoryName);
			Assert.False(addExistingCategoryResult.HasErrors);

			// act
			var updateCategoryNameResult = _sut.UpdateCategoryName(addCategoryResult.Result.Id, existingCategoryName);
			Assert.False(updateCategoryNameResult.HasErrors);

			var allCategoriesResult = _sut.GetAllCategories();
			Assert.False(allCategoriesResult.HasErrors);
			Assert.Equal(1, allCategoriesResult.Result.Count());

			// cleanup
			_sut.DeleteCategory(addCategoryResult.Result.Id);
			_sut.DeleteCategory(addExistingCategoryResult.Result.Id);
		}
		
		[Fact]
		public void UpdateCategoryName_NonExistantCategory()
		{
			var nonExistantCategoryId = -1;
			var newCategoryName = "new category";

			// act
			var result = _sut.UpdateCategoryName(nonExistantCategoryId, newCategoryName);
			Assert.True(result.HasErrors);
		}
	}
}

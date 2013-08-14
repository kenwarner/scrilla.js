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
		public CategoryServiceFixture()
		{
			_sut = _fixture.Create<CategoryService>();
		}

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
	}
}

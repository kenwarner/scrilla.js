using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace scrilla.Services.Tests
{
	public class BudgetServiceFixture : BaseFixture<BudgetService>
	{
		[Fact]
		public void GetBudgetCategories_BetweenFromAndToIncluded()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var categoryName = "test category";
			var fromMonth = new DateTime(2000, 1, 1);
			var amount = 10.0M;

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// create test budget category
			var setBudgetResult = _sut.UpdateBudget(addCategoryResult.Result.Id, fromMonth, amount);
			Assert.False(setBudgetResult.HasErrors);

			// act
			var result = _sut.GetBudgetCategories(fromMonth);
			Assert.False(result.HasErrors);
			Assert.Equal(1, result.Result.Count());

			// cleanup
			_sut.DeleteBudgetCategory(addCategoryResult.Result.Id, fromMonth);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetBudgetCategories_BeforeFromNotIncluded()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var categoryName = "test category";
			var fromMonth = new DateTime(2000, 1, 1);
			var updateMonth = fromMonth.AddDays(-1);
			var amount = 10.0M;

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// create test budget category
			var setBudgetResult = _sut.UpdateBudget(addCategoryResult.Result.Id, updateMonth, amount);
			Assert.False(setBudgetResult.HasErrors);

			// act
			var result = _sut.GetBudgetCategories(fromMonth);
			Assert.False(result.HasErrors);
			Assert.Equal(0, result.Result.Count());

			// cleanup
			_sut.DeleteBudgetCategory(addCategoryResult.Result.Id, updateMonth);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetBudgetCategories_AfterToNotIncluded()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var categoryName = "test category";
			var fromMonth = new DateTime(2000, 1, 1);
			var toMonth = new DateTime(2000, 2, 1);
			var updateMonth = toMonth.AddDays(1);
			var amount = 10.0M;

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// create test budget category
			var setBudgetResult = _sut.UpdateBudget(addCategoryResult.Result.Id, updateMonth, amount);
			Assert.False(setBudgetResult.HasErrors);

			// act
			var result = _sut.GetBudgetCategories(fromMonth, toMonth);
			Assert.False(result.HasErrors);
			Assert.Equal(0, result.Result.Count());

			// cleanup
			_sut.DeleteBudgetCategory(addCategoryResult.Result.Id, updateMonth);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void DeleteBudgetCategory_ExistingBudgetCategory()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var categoryName = "test category";
			var fromMonth = new DateTime(2000, 1, 1);
			var amount = 10.0M;

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// create test budget category
			var setBudgetResult = _sut.UpdateBudget(addCategoryResult.Result.Id, fromMonth, amount);
			Assert.False(setBudgetResult.HasErrors);

			// act
			var result = _sut.DeleteBudgetCategory(addCategoryResult.Result.Id, fromMonth);
			Assert.False(result.HasErrors);
			Assert.True(result.Result);

			// make sure the test account does not exist
			var getResult = _sut.GetBudgetCategories();
			Assert.False(getResult.HasErrors);
			Assert.Equal(0, getResult.Result.Count());

			// cleanup
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void DeleteBudgetCategory_NonexistantBudgetCategory()
		{
			var nonExistantBudgetCategoryId = -1;
			var fromMonth = new DateTime(2000, 1, 1);

			// act
			var result = _sut.DeleteBudgetCategory(nonExistantBudgetCategoryId, fromMonth);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void UpdateBudget_ExistingBudgetCategory_NotImplemented()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void UpdateBudget_NewBudgetCategory_NotImplemented()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void UpdateBudget_NoBillTransactions_NotImplemented()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void UpdateBudget_IncludesBillTransactions_NotImplemented()
		{
			throw new NotImplementedException();
		}
	}
}

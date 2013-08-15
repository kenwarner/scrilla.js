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
			Assert.Equal(categoryName, addCategoryResult.Result.Name);

			// create test budget category
			var setBudgetResult = _sut.UpdateBudget(fromMonth, addCategoryResult.Result.Id, amount);
			Assert.False(setBudgetResult.HasErrors);

			// act
			var result = _sut.GetBudgetCategories(fromMonth);
			Assert.False(result.HasErrors);

			// cleanup
			_sut.DeleteBudgetCategory(fromMonth, addCategoryResult.Result.Id);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetBudgetCategories_BeforeFromNotIncluded()
		{

		}

		[Fact]
		public void GetBudgetCategories_AfterToNotIncluded()
		{

		}

		[Fact]
		public void DeleteBudgetCategory_NotImplemented()
		{
			throw new NotImplementedException();
		}


		[Fact]
		public void UpdateBudget_NotImplemented()
		{
			throw new NotImplementedException();
		}
	}
}

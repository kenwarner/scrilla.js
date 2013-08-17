using Ploeh.AutoFixture;
using scrilla.Data;
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
		public void DeleteBudgetCategory_NonExistantBudgetCategory()
		{
			var nonExistantBudgetCategoryId = -1;
			var fromMonth = new DateTime(2000, 1, 1);

			// act
			var result = _sut.DeleteBudgetCategory(nonExistantBudgetCategoryId, fromMonth);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void UpdateBudget_NonExistantCategory()
		{
			var nonExistantCategoryId = -1;
			var month = new DateTime(2000, 1, 1);
			var amount = 10M;

			// act
			var result = _sut.UpdateBudget(nonExistantCategoryId, month, amount);
			Assert.True(result.HasErrors);
		}
		
		[Fact]
		public void UpdateBudget_ExistingCategory_NewBudgetCategory()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var categoryName = "test category";
			var month = new DateTime(2000, 1, 1);
			var amount = 10M;

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// act
			var result = _sut.UpdateBudget(addCategoryResult.Result.Id, month, amount);
			Assert.False(result.HasErrors);
			Assert.Equal(month, result.Result.Month);
			Assert.Equal(amount, result.Result.ExtraAmount);
			Assert.Equal(0, result.Result.BillsAmount);

			// cleanup
			_sut.DeleteBudgetCategory(addCategoryResult.Result.Id, month);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void UpdateBudget_ExistingBudgetCategory()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var categoryName = "test category";
			var month = new DateTime(2000, 1, 1);
			var amount = 10M;
			var newAmount = 20M;

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// create existing budget category
			var createExistingBudgetCategoryResult = _sut.UpdateBudget(addCategoryResult.Result.Id, month, amount);
			Assert.False(createExistingBudgetCategoryResult.HasErrors);

			// act
			var result = _sut.UpdateBudget(addCategoryResult.Result.Id, month, newAmount);
			Assert.False(result.HasErrors);
			Assert.Equal(month, result.Result.Month);
			Assert.Equal(newAmount, result.Result.ExtraAmount);
			Assert.Equal(0, result.Result.BillsAmount);

			// cleanup
			_sut.DeleteBudgetCategory(addCategoryResult.Result.Id, month);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void UpdateBudget_ExistingCategory_NewBudgetCategory_WithBillTransactions()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var billService = _fixture.Create<BillService>();
			var categoryName = "test category";
			var month = new DateTime(2000, 1, 1);
			var amount = 10M;
			var billName = "test bill";
			var billAmount = 20M;
			var billStartDate = new DateTime(2000, 1, 1);
			var billEndDate = billStartDate.AddMonths(1);

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// create test bill transactions
			var addBillResult = billService.AddBill(billName, billAmount, BillFrequency.Monthly, billStartDate, billEndDate, categoryId: addCategoryResult.Result.Id);
			Assert.False(addBillResult.HasErrors);

			// act
			var result = _sut.UpdateBudget(addCategoryResult.Result.Id, month, amount);
			Assert.False(result.HasErrors);
			Assert.Equal(month, result.Result.Month);
			Assert.Equal(amount, result.Result.ExtraAmount);
			Assert.Equal(billAmount * 2, result.Result.BillsAmount);

			// cleanup
			_sut.DeleteBudgetCategory(addCategoryResult.Result.Id, month);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}
	}
}

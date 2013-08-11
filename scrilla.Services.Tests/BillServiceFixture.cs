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
	public class BillServiceFixture : BaseFixture
	{
		public BillServiceFixture()
		{
			_fixture.Register<ICategoryService>(() => _fixture.Create<CategoryService>());
			_fixture.Register<IVendorService>(() => _fixture.Create<VendorService>());
		}

		[Fact]
		public void GetBill_ExistingBill_WithNullBillGroup_AndNullCategory_AndNullVendor_AndNullSecondaryDates()
		{
			var sut = _fixture.Create<BillService>();
			var billName = "test bill";
			var amount = 1.23M;
			int? billGroupId = null;
			int? categoryId = null;
			int? vendorId = null;
			DateTime startDate = new DateTime(2013, 7, 30);
			DateTime endDate = new DateTime(2013, 10, 15);
			BillFrequency frequency = BillFrequency.Daily;
			DateTime? secondaryStartDate = null;
			DateTime? secondaryEndDate = null;

			// create test bill
			var addBillResult = sut.AddBill(billName, amount, billGroupId, categoryId, vendorId, startDate, endDate, frequency, secondaryStartDate, secondaryEndDate);
			Assert.False(addBillResult.HasErrors);
			Assert.Equal(billName, addBillResult.Result.Name);
			Assert.Equal(amount, addBillResult.Result.Amount);
			Assert.Equal(billGroupId, addBillResult.Result.BillGroupId);
			Assert.Equal(categoryId, addBillResult.Result.CategoryId);
			Assert.Equal(vendorId, addBillResult.Result.VendorId);
			Assert.Equal(startDate, addBillResult.Result.StartDate);
			Assert.Equal(endDate, addBillResult.Result.EndDate);
			Assert.Equal(frequency, addBillResult.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, addBillResult.Result.StartDate2);
			Assert.Equal(secondaryEndDate, addBillResult.Result.EndDate2);

			// act
			var result = sut.GetBill(addBillResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(billName, result.Result.Name);
			Assert.Equal(amount, result.Result.Amount);
			Assert.Equal(billGroupId, result.Result.BillGroupId);
			Assert.Equal(categoryId, result.Result.CategoryId);
			Assert.Equal(vendorId, result.Result.VendorId);
			Assert.Equal(startDate, result.Result.StartDate);
			Assert.Equal(endDate, result.Result.EndDate);
			Assert.Equal(frequency, result.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, result.Result.StartDate2);
			Assert.Equal(secondaryEndDate, result.Result.EndDate2);

			// cleanup
			sut.DeleteBill(addBillResult.Result.Id);
		}

		[Fact]
		public void GetBill_ExistingBill_WithBillGroup_AndCategory_AndVendor_AndSecondaryDates()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var vendorService = _fixture.Create<VendorService>();
			var sut = _fixture.Create<BillService>();
			var billName = "test bill";
			var amount = 1.23M;
			DateTime startDate = new DateTime(2013, 7, 2);
			DateTime endDate = new DateTime(2013, 10, 2);
			BillFrequency frequency = BillFrequency.Daily;
			DateTime? secondaryStartDate = new DateTime(2013, 7, 16);
			DateTime? secondaryEndDate = new DateTime(2013, 10, 16);

			// create test bill group
			var billGroupName = "test bill group";
			var addBillGroupResult = sut.AddBillGroup(billGroupName);
			Assert.False(addBillGroupResult.HasErrors);
			Assert.Equal(billGroupName, addBillGroupResult.Result.Name);

			// create test category
			var categoryName = "test category";
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);

			// create test vendor
			var vendorName = "test vendor";
			var addVendorResult = vendorService.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(vendorName, addVendorResult.Result.Name);

			// create test bill
			var addBillResult = sut.AddBill(billName, amount, addBillGroupResult.Result.Id, addCategoryResult.Result.Id, addVendorResult.Result.Id
				, startDate, endDate, frequency, secondaryStartDate, secondaryEndDate);
			Assert.False(addBillResult.HasErrors);
			Assert.Equal(billName, addBillResult.Result.Name);
			Assert.Equal(amount, addBillResult.Result.Amount);
			Assert.Equal(addBillGroupResult.Result.Id, addBillResult.Result.BillGroupId);
			Assert.Equal(addCategoryResult.Result.Id, addBillResult.Result.CategoryId);
			Assert.Equal(addVendorResult.Result.Id, addBillResult.Result.VendorId);
			Assert.Equal(startDate, addBillResult.Result.StartDate);
			Assert.Equal(endDate, addBillResult.Result.EndDate);
			Assert.Equal(frequency, addBillResult.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, addBillResult.Result.StartDate2);
			Assert.Equal(secondaryEndDate, addBillResult.Result.EndDate2);

			// act
			var result = sut.GetBill(addBillResult.Result.Id);
			Assert.False(addBillResult.HasErrors);
			Assert.Equal(billName, result.Result.Name);
			Assert.Equal(billName, result.Result.Name);
			Assert.Equal(amount, result.Result.Amount);
			Assert.Equal(addBillGroupResult.Result.Id, result.Result.BillGroupId);
			Assert.Equal(addCategoryResult.Result.Id, result.Result.CategoryId);
			Assert.Equal(addVendorResult.Result.Id, result.Result.VendorId);
			Assert.Equal(startDate, result.Result.StartDate);
			Assert.Equal(endDate, result.Result.EndDate);
			Assert.Equal(frequency, result.Result.RecurrenceFrequency);
			Assert.Equal(secondaryStartDate, result.Result.StartDate2);
			Assert.Equal(secondaryEndDate, result.Result.EndDate2);

			// cleanup
			sut.DeleteBill(addBillResult.Result.Id);
			sut.DeleteBillGroup(addBillGroupResult.Result.Id);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
			vendorService.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void GetBill_NonExistantBill()
		{
			var sut = _fixture.Create<BillService>();
			var nonExistantBillId = -1;

			// act
			var result = sut.GetBill(nonExistantBillId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetBillGroup_ExistingBillGroup()
		{
			var sut = _fixture.Create<BillService>();
			var billGroupName = "test bill group";
			var displayOrder = 1;
			var isActive = true;

			// create test bill group
			var addBillGroupResult = sut.AddBillGroup(billGroupName, displayOrder, isActive);
			Assert.False(addBillGroupResult.HasErrors);
			Assert.Equal(billGroupName, addBillGroupResult.Result.Name);
			Assert.Equal(displayOrder, addBillGroupResult.Result.DisplayOrder);
			Assert.Equal(isActive, addBillGroupResult.Result.IsActive);

			// act
			var result = sut.GetBillGroup(addBillGroupResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(billGroupName, result.Result.Name);
			Assert.Equal(displayOrder, result.Result.DisplayOrder);
			Assert.Equal(isActive, result.Result.IsActive);

			// cleanup
			sut.DeleteBillGroup(addBillGroupResult.Result.Id);
		}

		[Fact]
		public void GetBillGroup_NonExistantBillGroup()
		{
			var sut = _fixture.Create<BillService>();
			var nonExistantBillGroupId = -1;

			// act
			var result = sut.GetBillGroup(nonExistantBillGroupId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}

}

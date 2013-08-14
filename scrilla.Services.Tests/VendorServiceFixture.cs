﻿using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace scrilla.Services.Tests
{
	public class VendorServiceFixture : BaseFixture<VendorService>
	{
		public VendorServiceFixture()
		{
			_fixture.Register<ICategoryService>(() => _fixture.Create<CategoryService>());
			_sut = _fixture.Create<VendorService>();
		}

		[Fact]
		public void GetVendor_ExistingVendor_WithNullDefaultCategoryId()
		{
			var vendorName = "test vendor";
			int? defaultCategoryId = null;

			// create test vendor
			var addVendorResult =  _sut.AddVendor(vendorName, defaultCategoryId);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(vendorName, addVendorResult.Result.Name);
			Assert.Equal(defaultCategoryId, addVendorResult.Result.DefaultCategoryId);

			// act
			var result =  _sut.GetVendor(addVendorResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(vendorName, result.Result.Name);
			Assert.Equal(defaultCategoryId, result.Result.DefaultCategoryId);

			// cleanup
			 _sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void GetVendor_ExistingVendor_WithDefaultCategoryId()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var vendorName = "test vendor";
			var categoryName = "test category";

			// create test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);
			Assert.Equal(categoryName, addCategoryResult.Result.Name);

			// create test vendor
			var addVendorResult =  _sut.AddVendor(vendorName, addCategoryResult.Result.Id);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(vendorName, addVendorResult.Result.Name);
			Assert.Equal(addCategoryResult.Result.Id, addVendorResult.Result.DefaultCategoryId);

			// act
			var result =  _sut.GetVendor(addVendorResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(vendorName, result.Result.Name);
			Assert.Equal(addCategoryResult.Result.Id, result.Result.DefaultCategoryId);

			// cleanup
			 _sut.DeleteVendor(addVendorResult.Result.Id);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void GetVendor_NonExistantVendor()
		{
			var nonExistantVendorId = -1;

			// act
			var result =  _sut.GetVendor(nonExistantVendorId);

			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}
	}
}

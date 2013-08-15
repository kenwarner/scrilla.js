using Ploeh.AutoFixture;
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
			_fixture.Register<ITransactionService>(() => _fixture.Create<TransactionService>());
			_fixture.Register<ICategoryService>(() => _fixture.Create<CategoryService>());
			_fixture.Register<IAccountService>(() => _fixture.Create<AccountService>());
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

		[Fact]
		public void GetVendor_ExistingName()
		{
			var vendorName = "test vendor";

			// create test vendor
			var addVendorResult = _sut.AddVendor(vendorName, null);
			Assert.False(addVendorResult.HasErrors);

			// act
			var result = _sut.GetVendor(vendorName);
			Assert.False(result.HasErrors);
			Assert.Equal(vendorName, result.Result.Name);

			// cleanup
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void GetVendor_NonExistantName()
		{
			var nonExistantVendor = "nonexistant vendor";

			// act
			var result = _sut.GetVendor(nonExistantVendor);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetVendorMap_ExistingVendorMap()
		{
			var vendorName = "test vendor";
			var description = "test vendor map";

			// add a test vendor
			var addVendorResult = _sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);

			// add a test vendor map
			var addVendorMapResult = _sut.AddVendorMap(addVendorResult.Result.Id, description);
			Assert.False(addVendorMapResult.HasErrors);

			// act
			var result = _sut.GetVendorMap(addVendorResult.Result.Id);
			Assert.False(result.HasErrors);
			Assert.Equal(description, result.Result.Description);
			Assert.Equal(addVendorResult.Result.Id, result.Result.VendorId);

			// cleanup
			_sut.DeleteVendorMap(addVendorMapResult.Result.Id);
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void GetVendorMap_NonExistantVendorMap()
		{
			var nonExistantVendorMapId = -1;

			// act
			var result = _sut.GetVendorMap(nonExistantVendorMapId);
			Assert.True(result.HasErrors);
			Assert.True(result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void GetAllVendors_OneVendorExistsAfterAddingAVendor()
		{
			var vendorResult = _sut.GetAllVendors();
			Assert.False(vendorResult.HasErrors);
			Assert.Empty(vendorResult.Result);

			// create test account
			var name = "test vendor";
			var addVendorResult = _sut.AddVendor(name);
			Assert.False(addVendorResult.HasErrors);

			// act
			vendorResult = _sut.GetAllVendors();
			Assert.False(vendorResult.HasErrors);
			Assert.Equal(1, vendorResult.Result.Count());

			// cleanup
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void AddVendor_NullDefaultCategoryId()
		{
			var name = "test vendor";
			int? defaultCategoryId = null;

			// act
			var addVendorResult = _sut.AddVendor(name, defaultCategoryId);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(name, addVendorResult.Result.Name);
			Assert.Equal(defaultCategoryId, addVendorResult.Result.DefaultCategoryId);

			// cleanup
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void AddVendor_NonNullDefaultCategoryId()
		{
			var categoryService = _fixture.Create<CategoryService>();
			var name = "test vendor";

			// get a default category
			var categoryName = "test category";
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// act
			var addVendorResult = _sut.AddVendor(name, addCategoryResult.Result.Id);
			Assert.False(addVendorResult.HasErrors);
			Assert.Equal(name, addVendorResult.Result.Name);
			Assert.Equal(addCategoryResult.Result.Id, addVendorResult.Result.DefaultCategoryId);

			// cleanup
			_sut.DeleteVendor(addVendorResult.Result.Id);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void AddVendorMap_ExistingVendor()
		{
			var vendorName = "test vendor";
			var description = "test vendor map";

			// add a test vendor
			var addVendorResult = _sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);

			// act
			var addVendorMapResult = _sut.AddVendorMap(addVendorResult.Result.Id, description);
			Assert.False(addVendorMapResult.HasErrors);
			Assert.Equal(description, addVendorMapResult.Result.Description);
			Assert.Equal(addVendorResult.Result.Id, addVendorMapResult.Result.VendorId);

			// cleanup
			_sut.DeleteVendorMap(addVendorMapResult.Result.Id);
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void AddVendorMap_NonExistantVendor()
		{
			var description = "test vendor map";
			var nonExistantVendorId = -1;

			// act
			var result = _sut.AddVendorMap(nonExistantVendorId, description);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void DeleteVendor_ExistingVendor()
		{
			var name = "test vendor";

			// add a test vendor
			var addResult = _sut.AddVendor(name);
			Assert.False(addResult.HasErrors);

			// delete the test vendor
			var deletionResult = _sut.DeleteVendor(addResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test vendor does not exist
			var getResult = _sut.GetVendor(addResult.Result.Id);
			Assert.True(getResult.HasErrors);
			Assert.True(getResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void DeleteVendor_ExistingVendor_With_VendorMap()
		{
			var name = "test vendor";
			var vendorMapDescription = "test vendor description";

			// add a test vendor
			var addVendorResult = _sut.AddVendor(name);
			Assert.False(addVendorResult.HasErrors);

			// add a vendor map
			var addVendorMapResult = _sut.AddVendorMap(addVendorResult.Result.Id, vendorMapDescription);
			Assert.False(addVendorMapResult.HasErrors);

			// delete the test vendor
			var deletionResult = _sut.DeleteVendor(addVendorResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test vendor does not exist
			var getVendorResult = _sut.GetVendor(addVendorResult.Result.Id);
			Assert.True(getVendorResult.HasErrors);
			Assert.True(getVendorResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));

			// make sure the test vendor map does not exist
			var getVendorMapResult = _sut.GetVendorMap(addVendorMapResult.Result.Id);
			Assert.True(getVendorMapResult.HasErrors);
			Assert.True(getVendorMapResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));
		}

		[Fact]
		public void DeleteVendor_NonExistantVendor()
		{
			var nonExistantVendorId = -1;

			// act
			var result = _sut.DeleteVendor(nonExistantVendorId);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void DeleteVendorMap_ExistingVendorMap()
		{
			var vendorName = "test vendor";
			var description = "test vendor map";

			// add a test vendor
			var addVendorResult = _sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);

			// add a test vendor map
			var addVendorMapResult = _sut.AddVendorMap(addVendorResult.Result.Id, description);
			Assert.False(addVendorMapResult.HasErrors);

			// delete the test vendor map
			var deletionResult = _sut.DeleteVendorMap(addVendorMapResult.Result.Id);
			Assert.False(deletionResult.HasErrors);

			// make sure the test vendor map does not exist
			var getResult = _sut.GetVendorMap(addVendorMapResult.Result.Id);
			Assert.True(getResult.HasErrors);
			Assert.True(getResult.ErrorMessages.Any(x => x.Key == ErrorType.NotFound));

			// cleanup
			_sut.DeleteVendorMap(addVendorMapResult.Result.Id);
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void DeleteVendorMap_NonExistantVendorMap()
		{
			var nonExistantVendorMapId = -1;

			// act
			var result = _sut.DeleteVendorMap(nonExistantVendorMapId);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void UpdateVendorName_ExistingVendor_WithDistinctNewName()
		{
			var vendorName = "test vendor";
			var newVendorName = "new test vendor";

			// add test vendor
			var addVendorResult = _sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);

			// act
			var updateVendorNameResult = _sut.UpdateVendorName(addVendorResult.Result.Id, newVendorName);
			Assert.False(updateVendorNameResult.HasErrors);

			// cleanup
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void UpdateVendorName_ExistingVendor_WithExistingNewName()
		{
			// TODO make sure all the associations are cleaned up

			var existingVendorName = "existing test vendor";
			var vendorName = "test vendor";

			// add test vendor
			var addVendorResult = _sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);

			// add existing test vendor
			var addExistingVendorResult = _sut.AddVendor(existingVendorName);
			Assert.False(addExistingVendorResult.HasErrors);

			// act
			var updateVendorNameResult = _sut.UpdateVendorName(addVendorResult.Result.Id, existingVendorName);
			Assert.False(updateVendorNameResult.HasErrors);

			var allVendorsResult = _sut.GetAllVendors();
			Assert.False(allVendorsResult.HasErrors);
			Assert.Equal(1, allVendorsResult.Result.Count());

			// cleanup
			_sut.DeleteVendor(addVendorResult.Result.Id);
			_sut.DeleteVendor(addExistingVendorResult.Result.Id);
		}

		[Fact]
		public void UpdateVendorName_NonExistantVendor()
		{
			var nonExistantVendorId = -1;
			var newVendorName = "new vendor";

			// act
			var result = _sut.UpdateVendorName(nonExistantVendorId, newVendorName);
			Assert.True(result.HasErrors);
		}

		[Fact]
		public void UpdateVendorDefaultCategory_ExistingVendor_And_ExistingCategory_With_RecategorizationEnabled()
		{
			var transactionService = _fixture.Create<TransactionService>();
			var accountService = _fixture.Create<AccountService>();
			var categoryService = _fixture.Create<CategoryService>();
			var vendorName = "test vendor";
			var categoryName = "test category";
			var accountName = "test account";

			// add test vendor
			var addVendorResult = _sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);

			// add test category
			var addCategoryResult = categoryService.AddCategory(categoryName);
			Assert.False(addCategoryResult.HasErrors);

			// add test account
			var addAccountResult = accountService.AddAccount(accountName);
			Assert.False(addAccountResult.HasErrors);

			// add test transactions
			var addTransactionResult = transactionService.AddTransaction(addAccountResult.Result.Id, DateTime.Now, 0.0M);
			Assert.False(addTransactionResult.HasErrors);

			// act
			var result = _sut.UpdateVendorDefaultCategory(addVendorResult.Result.Id, addCategoryResult.Result.Id, true);
			Assert.False(result.HasErrors);
			Assert.Equal(addCategoryResult.Result.Id, result.Result.DefaultCategoryId);
			var transactionResult = transactionService.GetTransactionsByAccount(addAccountResult.Result.Id);
			Assert.False(transactionResult.HasErrors);
			Assert.Equal(1, transactionResult.Result.Count());
			Assert.Equal(addCategoryResult.Result.Id, transactionResult.Result.First().Id);

			// cleanup
			transactionService.DeleteTransaction(addTransactionResult.Result.Id);
			accountService.DeleteAccount(addAccountResult.Result.Id);
			_sut.DeleteVendor(addVendorResult.Result.Id);
			categoryService.DeleteCategory(addCategoryResult.Result.Id);
		}

		[Fact]
		public void UpdateVendorDefaultCategory_ExistingVendor_And_NonExistantCategory()
		{
			var vendorName = "test vendor";
			var nonExistantCategoryId = -1;

			// add test vendor
			var addVendorResult = _sut.AddVendor(vendorName);
			Assert.False(addVendorResult.HasErrors);

			// act
			var result = _sut.UpdateVendorDefaultCategory(addVendorResult.Result.Id, nonExistantCategoryId);
			Assert.True(result.HasErrors);

			// cleanup
			_sut.DeleteVendor(addVendorResult.Result.Id);
		}

		[Fact]
		public void UpdateVendorDefaultCategory_NonExistantVendor()
		{
			var nonExistantVendorId = -1;

			// act
			var result = _sut.UpdateVendorDefaultCategory(nonExistantVendorId, null);
			Assert.True(result.HasErrors);
		}
	}
}

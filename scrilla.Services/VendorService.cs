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
	public class VendorService : EntityService, IVendorService
	{
		private ICategoryService _categoryService;

		public VendorService(IDatabase database, ICategoryService categoryService)
			: base(database) 
		{
			_categoryService = categoryService;
		}

		public ServiceResult<Vendor> GetVendor(int vendorId)
		{
			return base.GetEntity<Vendor>(vendorId);
		}

		public ServiceResult<Vendor> GetVendor(string name)
		{
			var result = new ServiceResult<Vendor>();

			var vendor = _db.Connection.Query<Vendor>("SELECT * FROM Vendor WHERE Name = @name", new { name });
			if (vendor == null || !vendor.Any())
			{
				result.AddError(ErrorType.NotFound, "Vendor {0} not found", name);
				return result;
			}

			// are there multiple vendors with the same name?
			if (vendor.Count() > 1)
			{
				result.AddError(ErrorType.Generic, "Multiple Vendors with name {0} exist", name);
				return result;
			}

			result.Result = vendor.First();
			return result;
		}

		public ServiceResult<ImportDescriptionVendorMap> GetVendorMap(int vendorMapId)
		{
			return base.GetEntity<ImportDescriptionVendorMap>(vendorMapId);
		}

		public ServiceResult<IEnumerable<Vendor>> GetAllVendors()
		{
			return base.GetAllEntity<Vendor>();
		}

		public ServiceResult<IEnumerable<ImportDescriptionVendorMap>> GetAllVendorMaps()
		{
			return base.GetAllEntity<ImportDescriptionVendorMap>();
		}

		public ServiceResult<Vendor> AddVendor(string name, int? defaultCategoryId = null)
		{
			var result = new ServiceResult<Vendor>();

			// TODO do we need to handle a case where defaultCategoryId = 0
			//if (defaultCategoryId.HasValue && defaultCategoryId.Value == 0)
			//	defaultCategoryId = null;


			// does category exist?
			if (defaultCategoryId.HasValue)
			{
				var categoryResult = _categoryService.GetCategory(defaultCategoryId.Value);
				if (categoryResult.HasErrors)
				{
					result.AddErrors(categoryResult);
					return result;
				}
			}

			// create Vendor
			var vendor = new Vendor()
			{
				Name = name,
				DefaultCategoryId = defaultCategoryId
			};

			_db.Insert<Vendor>(vendor);

			result.Result = vendor;
			return result;
		}

		public ServiceResult<ImportDescriptionVendorMap> AddVendorMap(int vendorId, string description)
		{
			var result = new ServiceResult<ImportDescriptionVendorMap>();

			var vendorResult = GetVendor(vendorId);
			if (vendorResult.HasErrors)
			{
				result.AddErrors(vendorResult);
				return result;
			}

			var vendorMap = new ImportDescriptionVendorMap()
			{
				VendorId = vendorId,
				Description = description
			};

			_db.Insert<ImportDescriptionVendorMap>(vendorMap);

			result.Result = vendorMap;
			return result;
		}


		public ServiceResult<bool> DeleteVendor(int vendorId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			// delete the vendor maps
			var vendorMaps = _db.Connection.Delete<ImportDescriptionVendorMap>(Predicates.Field<ImportDescriptionVendorMap>(x => x.VendorId, Operator.Eq, vendorId));

			// delete the vendor
			var deletionResult = _db.Delete<Vendor>(Predicates.Field<Vendor>(x => x.Id, Operator.Eq, vendorId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Vendor {0} not found", vendorId);
				return result;
			}

			result.Result = deletionResult;
			return result;
		}

		public ServiceResult<bool> DeleteVendorMap(int vendorMapId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<ImportDescriptionVendorMap>(Predicates.Field<ImportDescriptionVendorMap>(x => x.Id, Operator.Eq, vendorMapId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Vendor Mapping {0} not found", vendorMapId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		public ServiceResult<Vendor> UpdateVendorName(int vendorId, string name)
		{
			var result = new ServiceResult<Vendor>();
			Vendor resultVendor;

			// does vendor exist?
			var vendorResult = GetVendor(vendorId);
			if (vendorResult.HasErrors)
			{
				result.AddErrors(vendorResult);
				return result;
			}

			// does a vendor with this name already exist?
			var existingVendorResult = GetVendor(name);
			if (existingVendorResult.Result == null)
			{
				// just rename the original vendor
				vendorResult.Result.Name = name;
				_db.Update<Vendor>(vendorResult.Result);
				resultVendor = vendorResult.Result;
			}
			else
			{
				// move this vendor's transactions to the existing vendor
				_db.Connection.Execute("UPDATE [Transaction] SET VendorId = @existingVendorId WHERE VendorId = @vendorId", new { existingVendorId = existingVendorResult.Result.Id, vendorId = vendorResult.Result.Id });

				// move this vendor's bills and bill transactions to the existing vendor
				_db.Connection.Execute("UPDATE Bill SET VendorId = @existingVendorId WHERE VendorId = @vendorId", new { existingVendorId = existingVendorResult.Result.Id, vendorId = vendorResult.Result.Id });
				_db.Connection.Execute("UPDATE BillTransaction SET VendorId = @existingVendorId WHERE VendorId = @vendorId", new { existingVendorId = existingVendorResult.Result.Id, vendorId = vendorResult.Result.Id });
				_db.Connection.Execute("UPDATE BillTransaction SET OriginalVendorId = @existingVendorId WHERE OriginalVendorId = @vendorId", new { existingVendorId = existingVendorResult.Result.Id, vendorId = vendorResult.Result.Id });

				// move this vendor's import description maps to the existing vendor
				_db.Connection.Execute("UPDATE ImportDescriptionVendorMap SET VendorId = @existingVendorId WHERE VendorId = @vendorId", new { existingVendorId = existingVendorResult.Result.Id, vendorId = vendorResult.Result.Id });

				// delete the original vendor
				var deleteVendorResult = DeleteVendor(vendorResult.Result.Id);
				if (deleteVendorResult.HasErrors)
				{
					result.AddErrors(deleteVendorResult);
					return result;
				}

				resultVendor = existingVendorResult.Result;
			}

			// keep track of the rename in the mappings table
			var vendorMapResult = AddVendorMap(resultVendor.Id, name);
			if (vendorMapResult.HasErrors)
			{
				result.AddErrors(vendorMapResult);
				return result;
			}

			result.Result = resultVendor;
			return result;
		}

		public ServiceResult<Vendor> UpdateVendorDefaultCategory(int vendorId, int? defaultCategoryId, bool updateUncategorizedTransactions = false)
		{
			var result = new ServiceResult<Vendor>();

			// does vendor exist?
			var vendorResult = GetVendor(vendorId);
			if (vendorResult.HasErrors)
			{
				result.AddErrors(vendorResult);
				return result;
			}

			// does category exist?
			if (defaultCategoryId.HasValue)
			{
				var categoryResult = _categoryService.GetCategory(defaultCategoryId.Value);
				if (categoryResult.HasErrors)
				{
					result.AddErrors(categoryResult);
					return result;
				}
			}

			vendorResult.Result.DefaultCategoryId = defaultCategoryId;
			_db.Update<Vendor>(vendorResult.Result);

			// update any uncategorized transactions
			if (updateUncategorizedTransactions && defaultCategoryId.HasValue)
			{
				_db.Connection.Execute("UPDATE s SET s.CategoryId = @categoryId FROM Subtransaction s JOIN [Transaction] t ON t.Id = s.TransactionId AND t.VendorId = @vendorId", new { categoryId = defaultCategoryId.Value, vendorId = vendorId });
			}

			result.Result = vendorResult.Result;
			return result;
		}
	}
}

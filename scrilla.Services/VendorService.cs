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


		public ServiceResult<ImportDescriptionVendorMap> GetVendorMap(int vendorMapId)
		{
			return base.GetEntity<ImportDescriptionVendorMap>(vendorMapId);
		}

		public ServiceResult<IEnumerable<Vendor>> GetAllVendors()
		{
			return base.GetAllEntity<Vendor>();
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
					result.AddErrors(categoryResult.ErrorMessages);
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
				result.AddErrors(vendorResult.ErrorMessages);
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


		public ServiceResult<Vendor> UpdateVendor(int vendorId, string name, int? defaultCategoryId, bool updateUncategorizedTransactions)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<Vendor>();

			//// does this vendor even exist?
			//var vendor = _vendorRepository.GetById(vendorId);
			//if (vendor == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Vendor with Id {0} not found", vendorId);
			//	return result;
			//}

			//// see if a vendor with this name already exists
			//var existingVendor = _vendorRepository.Get(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
			//if (existingVendor != null && existingVendor != vendor)
			//{
			//	var renameResult = RenameVendor(vendorId, name);
			//	if (renameResult.HasErrors)
			//	{
			//		result.AddErrors(renameResult.ErrorMessages);
			//		return result;
			//	}

			//	vendor = renameResult.Result;
			//}

			//if (defaultCategoryId.HasValue && defaultCategoryId.Value == 0)
			//	defaultCategoryId = null;

			//// update uncategorized transactions
			//if (updateUncategorizedTransactions && defaultCategoryId.HasValue)
			//{
			//	var uncategorizedTransactions = _transactionRepository.GetMany(x => x.VendorId == vendorId && x.Subtransactions.Any(y => y.CategoryId == null)).ToList();
			//	if (uncategorizedTransactions != null)
			//	{
			//		foreach (var trx in uncategorizedTransactions)
			//		{
			//			var subtrx = trx.Subtransactions.Where(x => x.CategoryId == null);
			//			if (subtrx != null)
			//			{
			//				foreach (var sub in subtrx)
			//				{
			//					sub.CategoryId = defaultCategoryId.Value;
			//				}
			//			}
			//		}
			//	}
			//}

			//// keep track of the rename in the mappings table
			//if (!vendor.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
			//{
			//	vendor.ImportDescriptionVendorMaps.Add(new ImportDescriptionVendorMap() { Description = vendor.Name });
			//}

			//vendor.Name = name;
			//vendor.DefaultCategoryId = defaultCategoryId;
			//_unitOfWork.Commit();

			//result.Result = vendor;
			return result;
		}

		public ServiceResult<Vendor> UpdateVendorName(int vendorId, string name)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<Vendor>();

			//// does this vendor even exist?
			//var vendor = _vendorRepository.GetById(vendorId);
			//if (vendor == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Vendor with Id {0} not found", vendorId);
			//	return result;
			//}

			//// see if a vendor with this name already exists
			//var existingVendor = _vendorRepository.Get(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
			//if (existingVendor != null && existingVendor != vendor)
			//{
			//	foreach (var trx in vendor.Transactions)
			//	{
			//		trx.VendorId = existingVendor.Id;
			//	}

			//	foreach (var bill in vendor.Bills)
			//	{
			//		foreach (var billTrx in bill.BillTransactions)
			//		{
			//			if (billTrx.OriginalVendorId == vendorId)
			//			{
			//				billTrx.OriginalVendorId = existingVendor.Id;
			//			}

			//			if (billTrx.VendorId == vendorId)
			//			{
			//				billTrx.VendorId = existingVendor.Id;
			//			}
			//		}

			//		bill.VendorId = existingVendor.Id;
			//	}

			//	_vendorRepository.Delete(vendor);

			//	// keep track of the rename in the mappings table
			//	existingVendor.ImportDescriptionVendorMaps.Add(new ImportDescriptionVendorMap() { Description = vendor.Name });

			//	result.Result = existingVendor;
			//}
			//else
			//{
			//	// if there's not an existing vendor with this name, just rename the one we have
			//	// keep track of the rename in the mappings table
			//	vendor.ImportDescriptionVendorMaps.Add(new ImportDescriptionVendorMap() { Description = vendor.Name });
			//	vendor.Name = name;
			//	result.Result = vendor;
			//}

			//_unitOfWork.Commit();

			return result;
		}

		public ServiceResult<Vendor> UpdateVendorDefaultCategory(int vendorId, int? defaultCategoryId)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<Vendor>();

			//if (defaultCategoryId.HasValue && defaultCategoryId.Value == 0)
			//	defaultCategoryId = null;

			//// does this vendor even exist?
			//var vendor = _vendorRepository.GetById(vendorId);
			//if (vendor == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Vendor with Id {0} not found", vendorId);
			//	return result;
			//}

			//vendor.DefaultCategoryId = defaultCategoryId;
			//result.Result = vendor;

			//_unitOfWork.Commit();

			return result;
		}
	}
}

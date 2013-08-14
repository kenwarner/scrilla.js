using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public interface IVendorService
	{
		ServiceResult<Vendor> GetVendor(int vendorId);
		ServiceResult<ImportDescriptionVendorMap> GetVendorMap(int vendorMapId);

		ServiceResult<IEnumerable<Vendor>> GetAllVendors();

		ServiceResult<Vendor> AddVendor(string name, int? defaultCategoryId = null);

		ServiceResult<ImportDescriptionVendorMap> AddVendorMap(int vendorId, string description);

		ServiceResult<bool> DeleteVendor(int vendorId);
		ServiceResult<bool> DeleteVendorMap(int vendorMapId);

		ServiceResult<Vendor> UpdateVendor(int vendorId, string name, int? defaultCategoryId, bool updateUncategorizedTransactions);
		ServiceResult<Vendor> UpdateVendorName(int vendorId, string name);
		ServiceResult<Vendor> UpdateVendorDefaultCategory(int vendorId, int? defaultCategoryId);
	}
}

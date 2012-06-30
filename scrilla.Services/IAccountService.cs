using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data;
using scrilla.Data.Domain;

namespace scrilla.Services
{
	public interface IAccountService
	{
		ServiceResult<Account> GetAccount(int accountId);
		ServiceResult<Category> GetCategory(int categoryId);
		ServiceResult<Vendor> GetVendor(int vendorId);
		ServiceResult<Bill> GetBill(int billId);

		ServiceResult<IEnumerable<Account>> GetAllAccounts();
		ServiceResult<IEnumerable<Category>> GetAllCategories();
		ServiceResult<IEnumerable<CategoryGroup>> GetAllCategoryGroups();
		ServiceResult<IEnumerable<Vendor>> GetAllVendors();
		ServiceResult<IEnumerable<Bill>> GetAllBills();
		ServiceResult<IEnumerable<BillGroup>> GetAllBillGroups();

		ServiceResult<IEnumerable<Transaction>> GetAllTransactions(DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactions(int accountId, int? categoryId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByVendor(int? vendorId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByCategory(int? categoryId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<BudgetCategory>> GetBudgetCategories(DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<BillTransaction>> GetBillTransactions(int? billId, DateTime? from, DateTime? to);

		ServiceResult<Category> AddCategory(string name, int categoryGroupId);
		ServiceResult<Vendor> AddVendor(string name, int? defaultCategoryId = null);
		ServiceResult<Bill> AddBill(string name, decimal amount, int billGroupId, int? categoryId, int? vendorId, DateTime startDate, DateTime endDate, int frequency, DateTime? secondaryStartDate, DateTime? secondaryEndDate);

		// TODO need a categoryGroupId param on UpdateCategory
		ServiceResult<Category> UpdateCategory(int categoryId, string name);
		ServiceResult<Vendor> RenameVendor(int vendorId, string name);
		ServiceResult<Vendor> RecategorizeVendor(int vendorId, int? defaultCategoryId);
		ServiceResult<Vendor> UpdateVendor(int vendorId, string name, int? defaultCategoryId, bool updateUncategorizedTransactions);
		ServiceResult<Bill> UpdateBill(int billId, string name, decimal amount, int billGroupId, int? categoryId, int? vendorId, DateTime startDate, DateTime endDate, int frequency, bool updateExisting, DateTime? secondaryStartDate, DateTime? secondaryEndDate);
		ServiceResult<BillTransaction> UpdateBillTransaction(int billTransactionId, decimal? amount, DateTime? date, bool? isPaid, int? transactionId);
		ServiceResult<List<Tuple<Transaction, double>>> PredictBillTransactionMatch(int billTransactionId);

		ServiceResult<bool> ChangeCategory(int transactionId, int? categoryId);
		ServiceResult<bool> ChangeVendor(int transactionId, int? vendorId);
		ServiceResult<bool> ChangeDate(int transactionId, DateTime date);
		ServiceResult<bool> SetReconciled(int transactionId, bool isReconciled);
		ServiceResult<BudgetAmountInfo> SetBudget(DateTime month, int categoryId, decimal amount);

		ServiceResult<bool> DeleteCategory(int categoryId);
		ServiceResult<bool> DeleteVendor(int vendorId);
		ServiceResult<bool> DeleteVendorMap(int vendorMapId);
		ServiceResult<bool> DeleteBill(int billId);
	}

	public class BudgetAmountInfo
	{
		public DateTime Month { get; set; }
		public int CategoryId { get; set; }
		public decimal BillsAmount { get; set; }
		public decimal ExtraAmount { get; set; }
	}
}

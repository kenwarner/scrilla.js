using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using scrilla.Data;
using DapperExtensions;

namespace scrilla.Services
{
	public class AccountService : IAccountService
	{
		private readonly IDatabase _db;

		public AccountService(IDatabase database)
		{
			_db = database;
		}

		#region GetById

		private ServiceResult<T> GetEntity<T>(int id) where T : class
		{
			var result = new ServiceResult<T>();

			result.Result = _db.Get<T>(id);
			if (result.Result == null)
			{
				result.AddError(ErrorType.NotFound, "{0} {1} not found", (typeof(T)).Name, id);
			}

			return result;
		}

		public ServiceResult<Account> GetAccount(int accountId)
		{
			return GetEntity<Account>(accountId);
		}

		public ServiceResult<AccountGroup> GetAccountGroup(int accountGroupId)
		{
			return GetEntity<AccountGroup>(accountGroupId);
		}

		public ServiceResult<Category> GetCategory(int categoryId)
		{
			return GetEntity<Category>(categoryId);
		}

		public ServiceResult<Vendor> GetVendor(int vendorId)
		{
			return GetEntity<Vendor>(vendorId);
		}

		public ServiceResult<Bill> GetBill(int billId)
		{
			return GetEntity<Bill>(billId);
		}

		public ServiceResult<BillGroup> GetBillGroup(int billGroupId)
		{
			return GetEntity<BillGroup>(billGroupId);
		}

		public ServiceResult<CategoryGroup> GetCategoryGroup(int categoryGroupId)
		{
			return GetEntity<CategoryGroup>(categoryGroupId);
		}

		#endregion

		#region GetAll

		private ServiceResult<IEnumerable<T>> GetAllEntity<T>() where T : class
		{
			var result = new ServiceResult<IEnumerable<T>>();
			result.Result = _db.GetList<T>();
			return result;
		}

		public ServiceResult<IEnumerable<Account>> GetAllAccounts()
		{
			return GetAllEntity<Account>();
		}

		public ServiceResult<IEnumerable<Category>> GetAllCategories()
		{
			return GetAllEntity<Category>();
		}

		public ServiceResult<IEnumerable<CategoryGroup>> GetAllCategoryGroups()
		{
			return GetAllEntity<CategoryGroup>();
		}

		public ServiceResult<IEnumerable<Vendor>> GetAllVendors()
		{
			return GetAllEntity<Vendor>();
		}

		public ServiceResult<IEnumerable<Bill>> GetAllBills()
		{
			return GetAllEntity<Bill>();
		}

		public ServiceResult<IEnumerable<BillGroup>> GetAllBillGroups()
		{
			return GetAllEntity<BillGroup>();
		}

		public ServiceResult<IEnumerable<Transaction>> GetAllTransactions(DateTime? from = null, DateTime? to = null)
		{
			var result = new ServiceResult<IEnumerable<Transaction>>();

			var predicates = new List<IPredicate>();

			if (from.HasValue)
				predicates.Add(Predicates.Field<Transaction>(x => x.Timestamp, Operator.Ge, from.Value));

			if (to.HasValue)
				predicates.Add(Predicates.Field<Transaction>(x => x.Timestamp, Operator.Le, to.Value));
				
			object predicate = !predicates.Any() ?
				null : (predicates.Count == 1 ? 
					predicates.First() : 
					new PredicateGroup { Operator = GroupOperator.And, Predicates = predicates });

			result.Result = _db.GetList<Transaction>(predicate);
			return result;
		}

		public ServiceResult<IEnumerable<Transaction>> GetTransactions(int accountId, int? categoryId, DateTime? from = null, DateTime? to = null)
		{
			var result = new ServiceResult<IEnumerable<Transaction>>();

			//if (categoryId.HasValue && categoryId.Value == 0)
			//	categoryId = null;

			//var transactions = categoryId.HasValue ?
			//	_transactionRepository.GetMany(x => x.AccountId == accountId && x.Subtransactions.Any(y => y.CategoryId == categoryId)) :
			//	_transactionRepository.GetMany(x => x.AccountId == accountId && x.Subtransactions.Any(y => y.CategoryId == null));

			//if (from.HasValue) transactions = transactions.Where(x => x.Timestamp >= from.Value);
			//if (to.HasValue) transactions = transactions.Where(x => x.Timestamp <= to.Value);

			var predicates = new List<IPredicate>();
			predicates.Add(Predicates.Field<Transaction>(x => x.AccountId, Operator.Eq, accountId));

			if (from.HasValue)
				predicates.Add(Predicates.Field<Transaction>(x => x.Timestamp, Operator.Ge, from.Value));

			if (to.HasValue)
				predicates.Add(Predicates.Field<Transaction>(x => x.Timestamp, Operator.Le, to.Value));

			// TODO we need a better way to send back categoryId = 0
			//if (!categoryId.HasValue)
			//	predicates.Add(new ComparePredicate())

			object predicate = !predicates.Any() ?
				null : (predicates.Count == 1 ?
					predicates.First() :
					new PredicateGroup { Operator = GroupOperator.And, Predicates = predicates });

			throw new NotImplementedException();
			//result.Result = _db.GetList<Transaction>(predicate,);
			return result;
		}

		public ServiceResult<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, DateTime? from = null, DateTime? to = null)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<IEnumerable<Transaction>>();
			//var transactions = _transactionRepository.GetMany(x => x.AccountId == accountId);

			//if (from.HasValue) transactions = transactions.Where(x => x.Timestamp >= from.Value);
			//if (to.HasValue) transactions = transactions.Where(x => x.Timestamp <= to.Value);

			//result.Result = transactions;//.ToList();
			return result;
		}

		public ServiceResult<IEnumerable<Transaction>> GetTransactionsByVendor(int? vendorId, DateTime? from = null, DateTime? to = null)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<IEnumerable<Transaction>>();

			//if (vendorId.HasValue && vendorId.Value == 0)
			//	vendorId = null;

			//var transactions = vendorId.HasValue ?
			//	_transactionRepository.GetMany(x => x.VendorId == vendorId) :
			//	_transactionRepository.GetMany(x => x.VendorId == null);

			//if (from.HasValue) transactions = transactions.Where(x => x.Timestamp >= from.Value);
			//if (to.HasValue) transactions = transactions.Where(x => x.Timestamp <= to.Value);

			//result.Result = transactions;//.ToList();
			return result;
		}

		public ServiceResult<IEnumerable<Transaction>> GetTransactionsByCategory(int? categoryId, DateTime? from = null, DateTime? to = null)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<IEnumerable<Transaction>>();

			//if (categoryId.HasValue && categoryId.Value == 0)
			//	categoryId = null;

			//var transactions = categoryId.HasValue ?
			//	_transactionRepository.GetMany(x => x.Subtransactions.Any(y => y.CategoryId == categoryId)) :
			//	_transactionRepository.GetMany(x => x.Subtransactions.Any(y => y.CategoryId == null));

			//if (from.HasValue) transactions = transactions.Where(x => x.Timestamp >= from.Value);
			//if (to.HasValue) transactions = transactions.Where(x => x.Timestamp <= to.Value);

			//result.Result = transactions.ToList();
			return result;
		}

		public ServiceResult<IEnumerable<BudgetCategory>> GetBudgetCategories(DateTime? from = null, DateTime? to = null)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<IEnumerable<BudgetCategory>>();

			//var budgetCategories = _budgetCategoryRepository.GetAll();

			//if (from.HasValue) budgetCategories = budgetCategories.Where(x => x.Month >= from.Value);
			//if (to.HasValue) budgetCategories = budgetCategories.Where(x => x.Month <= to.Value);

			//result.Result = budgetCategories.ToList();
			return result;
		}

		/// <summary>
		/// Returns a list of bill transactions scheduled for this bill during this timeframe
		/// </summary>
		/// <param name="billId">Get all bill transactions for a value of null</param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public ServiceResult<IEnumerable<BillTransaction>> GetBillTransactions(int? billId, DateTime? from = null, DateTime? to = null)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<IEnumerable<BillTransaction>>();

			//var billTransactions = billId.HasValue ?
			//	_billTransactionRepository.GetMany(x => x.BillId == billId) :
			//	_billTransactionRepository.GetAll();

			//if (from.HasValue) billTransactions = billTransactions.Where(x => x.Timestamp >= from.Value);
			//if (to.HasValue) billTransactions = billTransactions.Where(x => x.Timestamp <= to.Value);

			//result.Result = billTransactions.ToList();
			return result;
		}

		#endregion

		#region Add

		public ServiceResult<Account> AddAccount(string name, decimal initialBalance = 0.0M, int? defaultCategoryId = null, int? accountGroupId = null)
		{
			var result = new ServiceResult<Account>();

			// does AccountGroup exist?
			if (accountGroupId.HasValue)
			{
				var accountGroupResult = GetAccountGroup(accountGroupId.Value);
				if (accountGroupResult.HasErrors)
				{
					result.AddErrors(accountGroupResult.ErrorMessages);
					return result;
				}
			}

			// does default Category exist?
			if (defaultCategoryId.HasValue)
			{
				var categoryResult = GetCategory(defaultCategoryId.Value);
				if (categoryResult.HasErrors)
				{
					result.AddErrors(categoryResult.ErrorMessages);
					return result;
				}
			}

			// create Account
			var account = new Account()
			{
				Name = name,
				InitialBalance = initialBalance,
				DefaultCategoryId = defaultCategoryId,
				Balance = initialBalance,
				BalanceTimestamp = DateTime.Now,
				AccountGroupId = accountGroupId
			};

			_db.Insert<Account>(account);

			result.Result = account;
			return result;
		}

		public ServiceResult<AccountGroup> AddAccountGroup(string name, int displayOrder = 0, bool isActive = true)
		{
			var result = new ServiceResult<AccountGroup>();

			// create AccountGroup
			var accountGroup = new AccountGroup()
			{
				Name = name,
				DisplayOrder = displayOrder,
				IsActive = isActive
			};

			_db.Insert<AccountGroup>(accountGroup);

			result.Result = accountGroup;
			return result;
		}

		public ServiceResult<Category> AddCategory(string name, int? categoryGroupId = null)
		{
			var result = new ServiceResult<Category>();

			// does Category Group exist?
			if (categoryGroupId.HasValue)
			{
				var categoryGroupResult = GetCategoryGroup(categoryGroupId.Value);
				if (categoryGroupResult.HasErrors)
				{
					result.AddErrors(categoryGroupResult.ErrorMessages);
					return result;
				}
			}

			// create Category
			var category = new Category()
			{
				Name = name,
				CategoryGroupId = categoryGroupId,
			};

			_db.Insert<Category>(category);

			result.Result = category;
			return result;
		}

		public ServiceResult<CategoryGroup> AddCategoryGroup(string name, int displayOrder = 0)
		{
			var result = new ServiceResult<CategoryGroup>();

			// create Category Group
			var categoryGroup = new CategoryGroup()
			{
				Name = name,
				DisplayOrder = displayOrder
				// TOOD we need a better way to reorder things
			};

			_db.Insert<CategoryGroup>(categoryGroup);

			result.Result = categoryGroup;
			return result;
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
				var categoryResult = GetCategory(defaultCategoryId.Value);
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

		public ServiceResult<Bill> AddBill(string name, decimal amount, int? billGroupId, int? categoryId, int? vendorId, DateTime startDate, DateTime endDate, BillFrequency frequency, DateTime? secondaryStartDate, DateTime? secondaryEndDate)
		{
			var result = new ServiceResult<Bill>();

			// TODO do we need to handle a case where billGroupId = 0
			//if (billGroupId.HasValue && billGroupId.Value == 0)
			//	billGroupId = null;
			//if (categoryId.HasValue && categoryId.Value == 0)
			//	categoryId = null;
			//if (vendorId.HasValue && vendorId.Value == 0)
			//	vendorId = null;

			// does the bill group exist?
			if (billGroupId.HasValue)
			{
				var billGroupResult = GetBillGroup(billGroupId.Value);
				if (billGroupResult.HasErrors)
				{
					result.AddErrors(billGroupResult.ErrorMessages);
				}
			}

			// does the category exist?
			if (categoryId.HasValue)
			{
				var categoryResult = GetCategory(categoryId.Value);
				if (categoryResult.HasErrors)
				{
					result.AddErrors(categoryResult.ErrorMessages);
				}
			}

			// does the vendor exist?
			if (vendorId.HasValue)
			{
				var vendorResult = GetVendor(vendorId.Value);
				if (vendorResult.HasErrors)
				{
					result.AddErrors(vendorResult.ErrorMessages);
				}
			}

			// does the startDate come before the endDate?
			if (endDate < startDate)
				result.AddError(ErrorType.Generic, "Start date {0} must come before End date {1}", startDate.ToString(), endDate.ToString());

			// TODO does the secondary dates both have to be null or non-null
			// TODO does the secondary start date come before the secondary end date

			// any errors thus far?
			if (result.HasErrors)
				return result;

			// create bill
			var bill = new Bill()
			{
				Name = name,
				Amount = amount,
				BillGroupId = billGroupId,
				CategoryId = categoryId,
				VendorId = vendorId,
				StartDate = startDate,
				EndDate = endDate,
				RecurrenceFrequency = frequency,
				StartDate2 = secondaryStartDate,
				EndDate2 = secondaryEndDate
			};

			_db.Insert<Bill>(bill);

			// create bill transactions
			//// create transactions
			//int count = 0;
			//DateTime cur = new DateTime(startDate.Year, startDate.Month, startDate.Day);

			//while (cur <= endDate)
			//{
			//	BillTransaction trx = new BillTransaction()
			//	{
			//		Amount = amount,
			//		OriginalAmount = amount,
			//		CategoryId = categoryId,
			//		OriginalCategoryId = categoryId,
			//		VendorId = vendorId,
			//		OriginalVendorId = vendorId,
			//		Timestamp = cur,
			//		OriginalTimestamp = cur
			//	};
			//	bill.BillTransactions.Add(trx);

			//	count++;
			//	if (frequency == 0)
			//		cur = endDate.AddDays(1);
			//	else if (frequency > 0)
			//		cur = startDate.AddDays(count * frequency);
			//	else
			//		cur = startDate.AddMonths(count * -1 * frequency);
			//}

			//if (secondaryStartDate.HasValue)
			//{
			//	if (secondaryEndDate.HasValue)
			//		endDate = secondaryEndDate.Value;

			//	count = 0;
			//	cur = new DateTime(secondaryStartDate.Value.Year, secondaryStartDate.Value.Month, secondaryStartDate.Value.Day);

			//	while (cur <= endDate)
			//	{
			//		BillTransaction trx = new BillTransaction()
			//		{
			//			Amount = amount,
			//			OriginalAmount = amount,
			//			CategoryId = categoryId,
			//			OriginalCategoryId = categoryId,
			//			VendorId = vendorId,
			//			OriginalVendorId = vendorId,
			//			Timestamp = cur,
			//			OriginalTimestamp = cur
			//		};
			//		bill.BillTransactions.Add(trx);

			//		count++;
			//		if (frequency == 0)
			//			cur = endDate.AddDays(1);
			//		else if (frequency > 0)
			//			cur = secondaryStartDate.Value.AddDays(count * frequency);
			//		else
			//			cur = secondaryStartDate.Value.AddMonths(count * -1 * frequency);
			//	}
			//}

			result.Result = bill;
			return result;
		}

		public ServiceResult<BillGroup> AddBillGroup(string name, int displayOrder = 0, bool isActive = true)
		{
			var result = new ServiceResult<BillGroup>();

			// create Bill Group
			var billGroup = new BillGroup()
			{
				Name = name,
				DisplayOrder = displayOrder,
				IsActive = isActive
			};

			_db.Insert<BillGroup>(billGroup);

			result.Result = billGroup;
			return result;
		}

		#endregion

		#region Delete

		public ServiceResult<bool> DeleteAccount(int accountId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<Account>(Predicates.Field<Account>(x => x.Id, Operator.Eq, accountId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Account {0} not found", accountId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		public ServiceResult<bool> DeleteAccountGroup(int accountGroupId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<AccountGroup>(Predicates.Field<AccountGroup>(x => x.Id, Operator.Eq, accountGroupId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "AccountGroup {0} not found", accountGroupId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		public ServiceResult<bool> DeleteCategory(int categoryId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<Category>(Predicates.Field<Category>(x => x.Id, Operator.Eq, categoryId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Category {0} not found", categoryId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		public ServiceResult<bool> DeleteCategoryGroup(int categoryGroupId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<CategoryGroup>(Predicates.Field<CategoryGroup>(x => x.Id, Operator.Eq, categoryGroupId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "CategoryGroup {0} not found", categoryGroupId);
				return result;
			}

			result.Result = deletionResult;

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

		public ServiceResult<bool> DeleteBill(int billId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<Bill>(Predicates.Field<Bill>(x => x.Id, Operator.Eq, billId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Bill {0} not found", billId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		public ServiceResult<bool> DeleteBillGroup(int billGroupId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			var deletionResult = _db.Delete<BillGroup>(Predicates.Field<BillGroup>(x => x.Id, Operator.Eq, billGroupId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Bill Group {0} not found", billGroupId);
				return result;
			}

			result.Result = deletionResult;

			return result;
		}

		#endregion



		public ServiceResult<Category> UpdateCategory(int categoryId, string name)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<Category>();

			//var category = _categoryRepository.GetById(categoryId);
			//if (category == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Category {0} not found", categoryId);
			//	return result;
			//}

			//category.Name = name;
			//_unitOfWork.Commit();

			//result.Result = category;
			return result;
		}

		public ServiceResult<Vendor> RecategorizeVendor(int vendorId, int? defaultCategoryId)
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

		public ServiceResult<Vendor> RenameVendor(int vendorId, string name)
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

		public ServiceResult<Bill> UpdateBill(int billId, string name, decimal amount, int billGroupId, int? categoryId, int? vendorId, DateTime startDate, DateTime endDate, int frequency, bool updateExisting, DateTime? secondaryStartDate, DateTime? secondaryEndDate)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<Bill>();

			//var bill = _billRepository.GetById(billId);
			//if (bill == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Bill {0} not found", billId);
			//	return result;
			//}

			//// TODO do we need to do exist checks for billGroupId, categoryId, vendorId?

			//if (categoryId.HasValue && categoryId.Value == 0)
			//	categoryId = null;
			//if (vendorId.HasValue && vendorId.Value == 0)
			//	vendorId = null;

			//if (updateExisting)
			//{
			//	if (bill.StartDate != startDate || bill.EndDate != endDate || bill.RecurrenceFrequency != frequency || bill.StartDate2 != secondaryStartDate || bill.EndDate2 != secondaryEndDate)
			//	{
			//		List<BillTransaction> existing = _billTransactionRepository.GetMany(x => x.BillId == billId).ToList();
			//		List<BillTransaction> expected = new List<BillTransaction>();

			//		#region Generate expected transactions

			//		int count = 0;
			//		DateTime cur = new DateTime(startDate.Year, startDate.Month, startDate.Day);
			//		while (cur <= endDate)
			//		{
			//			BillTransaction trx = new BillTransaction()
			//			{
			//				Amount = amount,
			//				OriginalAmount = amount,
			//				CategoryId = categoryId,
			//				OriginalCategoryId = categoryId,
			//				VendorId = vendorId,
			//				OriginalVendorId = vendorId,
			//				Timestamp = cur,
			//				OriginalTimestamp = cur
			//			};

			//			expected.Add(trx);

			//			count++;
			//			if (frequency == 0)
			//				cur = endDate.AddDays(1);
			//			else if (frequency > 0)
			//				cur = startDate.AddDays(count * frequency);
			//			else
			//				cur = startDate.AddMonths(count * -1 * frequency);
			//		}

			//		if (secondaryStartDate.HasValue)
			//		{
			//			if (secondaryEndDate.HasValue)
			//				endDate = secondaryEndDate.Value;

			//			count = 0;
			//			cur = new DateTime(secondaryStartDate.Value.Year, secondaryStartDate.Value.Month, secondaryStartDate.Value.Day);

			//			while (cur <= endDate)
			//			{
			//				BillTransaction trx = new BillTransaction()
			//				{
			//					Amount = amount,
			//					OriginalAmount = amount,
			//					CategoryId = categoryId,
			//					OriginalCategoryId = categoryId,
			//					VendorId = vendorId,
			//					OriginalVendorId = vendorId,
			//					Timestamp = cur,
			//					OriginalTimestamp = cur
			//				};

			//				expected.Add(trx);

			//				count++;
			//				if (frequency == 0)
			//					cur = endDate.AddDays(1);
			//				else if (frequency > 0)
			//					cur = secondaryStartDate.Value.AddDays(count * frequency);
			//				else
			//					cur = secondaryStartDate.Value.AddMonths(count * -1 * frequency);
			//			}
			//		}

			//		#endregion

			//		List<BillTransaction> reused = new List<BillTransaction>();

			//		while (existing.Any() && expected.Any())
			//		{
			//			var existingProjections = existing.Select(e => new
			//			{
			//				Item = e,
			//				Comparisons = expected.Select(x => new
			//				{
			//					Item = x,
			//					Days = Math.Abs((x.Timestamp - e.Timestamp).TotalDays)
			//				})
			//			});

			//			var bestExisting = existingProjections.OrderBy(x => x.Comparisons.Min(y => y.Days)).FirstOrDefault();
			//			if (bestExisting != null)
			//			{
			//				// shift existing record's timestamp to closest match in expected
			//				var bestMatch = bestExisting.Comparisons.OrderBy(x => x.Days).FirstOrDefault().Item;
			//				bestExisting.Item.Timestamp = bestMatch.Timestamp;
			//				bestExisting.Item.OriginalTimestamp = bestMatch.OriginalTimestamp;
			//				expected.Remove(bestMatch);
			//				existing.Remove(bestExisting.Item);
			//				reused.Add(bestExisting.Item);
			//			}
			//		}

			//		// delete unused transactions
			//		var complete = reused.Union(expected).Select(x => x.Id);
			//		_billTransactionRepository.Delete(x => x.BillId == billId && !complete.Contains(x.Id));

			//		//reused.ForEach(x => bill.BillTransactions.Add(x));
			//		expected.ForEach(x => bill.BillTransactions.Add(x));
			//	}

			//	if (bill.Amount != amount || bill.CategoryId != categoryId || bill.VendorId != vendorId)
			//	{
			//		var billTransasctions = _billTransactionRepository.GetMany(x => x.BillId == billId);
			//		if (billTransasctions != null)
			//		{
			//			foreach (var trx in billTransasctions)
			//			{
			//				if (bill.Amount != amount)
			//				{
			//					// only update a transaction amount if it hadn't been edited from it's original value (ie don't change modified amounts)
			//					if (trx.Amount == trx.OriginalAmount)
			//						trx.Amount = amount;
			//					trx.OriginalAmount = amount;
			//				}

			//				if (bill.CategoryId != categoryId)
			//					trx.CategoryId = categoryId;

			//				if (bill.VendorId != vendorId)
			//					trx.VendorId = vendorId;
			//			}
			//		}
			//	}
			//}

			//bill.Name = name;
			//bill.Amount = amount;
			//bill.BillGroupId = billGroupId;
			//bill.CategoryId = categoryId;
			//bill.VendorId = vendorId;
			//bill.StartDate = startDate;
			//bill.EndDate = endDate;
			//bill.StartDate2 = secondaryStartDate;
			//bill.EndDate2 = secondaryEndDate;

			//bill.RecurrenceFrequency = frequency;

			//_unitOfWork.Commit();

			//result.Result = bill;
			return result;
		}

		public ServiceResult<BillTransaction> UpdateBillTransaction(int billTransactionId, decimal? amount, DateTime? date, bool? isPaid, int? transactionId)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<BillTransaction>();

			//var billTransaction = _billTransactionRepository.GetById(billTransactionId);
			//if (billTransaction == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Bill transaction {0} not found", billTransactionId);
			//	return result;
			//}

			//if (amount.HasValue)
			//	billTransaction.Amount = amount.Value;
			//if (date.HasValue)
			//	billTransaction.Timestamp = date.Value;
			//if (isPaid.HasValue)
			//	billTransaction.IsPaid = isPaid.Value;
			//if (transactionId.HasValue)
			//{
			//	var transaction = _transactionRepository.GetById(transactionId.Value);
			//	if (transaction != null)
			//		transaction.BillTransactionId = billTransactionId;
			//}

			//_unitOfWork.Commit();

			return result;
		}

		public ServiceResult<List<Tuple<Transaction, double>>> PredictBillTransactionMatch(int billTransactionId)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<List<Tuple<Transaction, double>>>();

			//var billTransaction = _billTransactionRepository.GetById(billTransactionId);
			//if (billTransaction == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Bill transaction {0} not found", billTransactionId);
			//	return result;
			//}

			//// find predictions if it's not paid, or there are no associated transactions to indicate the paid status
			//if (!billTransaction.IsPaid || !billTransaction.Transactions.Any())
			//{
			//	var timestampLower = billTransaction.Timestamp.AddMonths(-2);
			//	var timestampUpper = billTransaction.Timestamp.AddMonths(2);
			//	var amountLower = billTransaction.Amount / 5.0M;
			//	var amountUpper = billTransaction.Amount * 5.0M;
			//	var amount = amountUpper;
			//	amountUpper = Math.Max(amountLower, amountUpper);
			//	amountLower = Math.Min(amount, amountLower);

			//	// find reasonable predictions
			//	var predictions = _transactionRepository.GetMany(x =>
			//		(x.Amount > amountLower && x.Amount < amountUpper)
			//		&& (x.Timestamp > timestampLower && x.Timestamp < timestampUpper)).ToList();

			//	// calculate confidence level
			//	var billTransactionVendorName = billTransaction.Vendor == null ? "" : billTransaction.Vendor.Name;
			//	var confidences = predictions.Select(x => new Tuple<Transaction, double>(x,
			//		(.1 * Math.Exp(-1 * Math.Pow((double)((x.Amount - billTransaction.Amount) / billTransaction.Amount), 2.0) / 2.0)
			//		+ .2 * Math.Exp(-1 * Math.Pow(((x.Timestamp - billTransaction.Timestamp).TotalDays / 60.0), 2.0))
			//		+ .2 * (x.Timestamp.Month == billTransaction.Timestamp.Month ? 1 : 0)
			//		+ .2 * ((x.VendorId.HasValue && (x.VendorId == billTransaction.VendorId)) ? 1 : 0)
			//		+ .3 * Math.Exp(-1 * Math.Pow(LevenshteinDistance.Compute(x.Vendor == null ? "" : x.Vendor.Name, billTransactionVendorName) / 20.0, 2.0)))
			//		* (x.BillTransaction == null ? 1.0 : 0.0)
			//		));

			//	// debug
			//	//Debug.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", "", billTransaction.Amount, billTransaction.Timestamp, billTransaction.VendorId, billTransactionVendorName, LevenshteinDistance.Compute(billTransactionVendorName, billTransactionVendorName));
			//	//predictions.ForEach(p =>
			//	//{
			//	//	var vendorName = p.Vendor == null ? "" : p.Vendor.Name;
			//	//	Debug.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", p.Id, p.Amount, p.Timestamp, p.VendorId, vendorName, LevenshteinDistance.Compute(vendorName, billTransactionVendorName));
			//	//});

			//	// order by confidence
			//	result.Result = confidences
			//		.OrderByDescending(x => x.Item2)
			//		.Take(5)
			//		.ToList();

			//}

			return result;
		}



		public ServiceResult<bool> SetReconciled(int transactionId, bool isReconciled)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<bool>();

			//var trx = _transactionRepository.GetById(transactionId);
			//if (trx == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Transaction {0} not found", transactionId);
			//	return result;
			//}

			//trx.IsReconciled = isReconciled;
			//_unitOfWork.Commit();
			//result.Result = true;

			return result;
		}

		public ServiceResult<bool> ChangeCategory(int transactionId, int? categoryId)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<bool>();

			//if (categoryId.HasValue)
			//{
			//	if (categoryId.Value == 0)
			//	{
			//		categoryId = null;
			//	}
			//	else
			//	{
			//		var category = _categoryRepository.GetById(categoryId.Value);
			//		if (category == null)
			//		{
			//			result.AddError(ErrorType.NotFound, "Category {0} not found", categoryId.Value);
			//			return result;
			//		}
			//	}
			//}

			//var trx = _transactionRepository.GetById(transactionId);
			//if (trx == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Transaction {0} not found", transactionId);
			//	return result;
			//}

			//if (trx.Subtransactions == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Transaction {0} has no transaction data", transactionId);
			//	return result;
			//}

			//// TODO eventually this needs to have subtransaction granualirty
			//foreach (var subtrx in trx.Subtransactions)
			//{
			//	subtrx.CategoryId = categoryId;
			//}
			//_unitOfWork.Commit();
			//result.Result = true;

			return result;
		}

		public ServiceResult<bool> ChangeVendor(int transactionId, int? vendorId)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<bool>();

			//if (vendorId.HasValue)
			//{
			//	if (vendorId.Value == 0)
			//	{
			//		vendorId = null;
			//	}
			//	else
			//	{
			//		var vendor = _vendorRepository.GetById(vendorId.Value);
			//		if (vendor == null)
			//		{
			//			result.AddError(ErrorType.NotFound, "Vendor {0} not found", vendorId.Value);
			//			return result;
			//		}
			//	}
			//}

			//var trx = _transactionRepository.GetById(transactionId);
			//if (trx == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Transaction {0} not found", transactionId);
			//	return result;
			//}

			//trx.VendorId = vendorId;
			//_unitOfWork.Commit();
			//result.Result = true;

			return result;
		}

		public ServiceResult<bool> ChangeDate(int transactionId, DateTime date)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<bool>();

			//var trx = _transactionRepository.GetById(transactionId);
			//if (trx == null)
			//{
			//	result.AddError(ErrorType.NotFound, "Transaction {0} not found", transactionId);
			//	return result;
			//}

			//trx.Timestamp = date;
			//_unitOfWork.Commit();
			//result.Result = true;

			return result;
		}

		public ServiceResult<BudgetAmountInfo> SetBudget(DateTime month, int categoryId, decimal amount)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<BudgetAmountInfo>();

			//var budget = _budgetCategoryRepository.Get(x => x.Month == month && x.CategoryId == categoryId);
			//if (budget == null)
			//{
			//	budget = new BudgetCategory() { Month = month, CategoryId = categoryId };
			//	_budgetCategoryRepository.Add(budget);
			//}

			//var nextMonth = month.AddMonths(1);
			//var bills = _billTransactionRepository.GetMany(x => x.CategoryId == categoryId && x.Timestamp >= month && x.Timestamp < nextMonth); // TODO paid vs !paid
			//var sumBills = bills.Any() ? bills.Sum(x => x.Amount) : 0M;

			//budget.Amount = amount;
			//_unitOfWork.Commit();

			//result.Result = new BudgetAmountInfo() { Month = month, CategoryId = categoryId, ExtraAmount = amount, BillsAmount = sumBills };

			return result;
		}


	}

	/// <summary>
	/// Contains approximate string matching
	/// </summary>
	static class LevenshteinDistance
	{
		/// <summary>
		/// Compute the distance between two strings.
		/// </summary>
		public static int Compute(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			// Step 1
			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			// Step 2
			for (int i = 0; i <= n; d[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; d[0, j] = j++)
			{
			}

			// Step 3
			for (int i = 1; i <= n; i++)
			{
				//Step 4
				for (int j = 1; j <= m; j++)
				{
					// Step 5
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					// Step 6
					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			// Step 7
			return d[n, m];
		}
	}
}

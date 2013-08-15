using DapperExtensions;
using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public class BillService : EntityService, IBillService
	{
		private ICategoryService _categoryService;
		private IVendorService _vendorService;

		public BillService(IDatabase database, ICategoryService categoryService, IVendorService vendorService)
			: base(database)
		{
			_categoryService = categoryService;
			_vendorService = vendorService;
		}

		public ServiceResult<Bill> GetBill(int billId)
		{
			return base.GetEntity<Bill>(billId);
		}

		public ServiceResult<BillGroup> GetBillGroup(int billGroupId)
		{
			return base.GetEntity<BillGroup>(billGroupId);
		}

		public ServiceResult<IEnumerable<Bill>> GetAllBills()
		{
			return base.GetAllEntity<Bill>();
		}

		public ServiceResult<IEnumerable<BillGroup>> GetAllBillGroups()
		{
			return base.GetAllEntity<BillGroup>();
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
					result.AddErrors(billGroupResult);
				}
			}

			// does the category exist?
			if (categoryId.HasValue)
			{
				var categoryResult = _categoryService.GetCategory(categoryId.Value);
				if (categoryResult.HasErrors)
				{
					result.AddErrors(categoryResult);
				}
			}

			// does the vendor exist?
			if (vendorId.HasValue)
			{
				var vendorResult = _vendorService.GetVendor(vendorId.Value);
				if (vendorResult.HasErrors)
				{
					result.AddErrors(vendorResult);
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

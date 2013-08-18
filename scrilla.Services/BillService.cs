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
	public class BillService : EntityService, IBillService
	{
		private ICategoryService _categoryService;
		private ITransactionService _transactionService;
		private IVendorService _vendorService;

		public BillService(IDatabase database, ICategoryService categoryService, ITransactionService transactionService, IVendorService vendorService)
			: base(database)
		{
			_categoryService = categoryService;
			_transactionService = transactionService;
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

		public ServiceResult<BillTransaction> GetBillTransaction(int billTransactionId)
		{
			return base.GetEntity<BillTransaction>(billTransactionId);
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
			var result = new ServiceResult<IEnumerable<BillTransaction>>();

			var predicates = new List<IPredicate>();

			if (billId.HasValue)
				predicates.Add(Predicates.Field<BillTransaction>(x => x.BillId, Operator.Eq, billId.Value));
			if (from.HasValue)
				predicates.Add(Predicates.Field<BillTransaction>(x => x.Timestamp, Operator.Ge, from.Value));
			if (to.HasValue)
				predicates.Add(Predicates.Field<BillTransaction>(x => x.Timestamp, Operator.Le, to.Value));

			var predicate = new PredicateGroup { Operator = GroupOperator.And, Predicates = predicates };
			var billTransactions = _db.GetList<BillTransaction>(predicate);

			result.Result = billTransactions;
			return result;
		}

		public ServiceResult<Bill> AddBill(string name, decimal amount, BillFrequency frequency, DateTime startDate, DateTime endDate, int? billGroupId = null, int? categoryId = null, int? vendorId = null, DateTime? secondaryStartDate = null, DateTime? secondaryEndDate = null)
		{
			var result = new ServiceResult<Bill>();

			// TODO do we need to handle a case where billGroupId = 0
			if (billGroupId.HasValue && billGroupId.Value == 0)
				billGroupId = null;
			if (categoryId.HasValue && categoryId.Value == 0)
				categoryId = null;
			if (vendorId.HasValue && vendorId.Value == 0)
				vendorId = null;

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

			// does the secondary startDate come before the secondary endDate?
			if (secondaryStartDate.HasValue && secondaryEndDate.HasValue)
			{
				if (secondaryEndDate.Value < secondaryStartDate.Value)
				{
					result.AddError(ErrorType.Generic, "Secondary Start date {0} must come before Secondary End date {1}", secondaryStartDate.ToString(), secondaryEndDate.ToString());
				}
			}

			// are both secondary startDate and secondary endDate provided?
			if ((secondaryStartDate.HasValue && !secondaryEndDate.HasValue) || (!secondaryStartDate.HasValue && secondaryEndDate.HasValue))
			{
				result.AddError(ErrorType.Generic, "Both Secondary Start date and Secondary End date should be provided: {0} & {1}", secondaryStartDate.ToString(), secondaryEndDate.ToString());
			}

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
			int count = 0;
			var billTransactions = new List<BillTransaction>();
			DateTime cur = new DateTime(startDate.Year, startDate.Month, startDate.Day);

			while (cur <= endDate)
			{
				BillTransaction trx = new BillTransaction()
				{
					BillId = bill.Id,
					Amount = amount,
					OriginalAmount = amount,
					CategoryId = categoryId,
					OriginalCategoryId = categoryId,
					VendorId = vendorId,
					OriginalVendorId = vendorId,
					Timestamp = cur,
					OriginalTimestamp = cur
				};
				billTransactions.Add(trx);

				count++;
				if (frequency == 0)
					cur = endDate.AddDays(1);
				else if (frequency > 0)
					cur = startDate.AddDays(count * (int)frequency);
				else
					cur = startDate.AddMonths(count * -1 * (int)frequency);
			}

			if (secondaryStartDate.HasValue)
			{
				if (secondaryEndDate.HasValue)
					endDate = secondaryEndDate.Value;

				count = 0;
				cur = new DateTime(secondaryStartDate.Value.Year, secondaryStartDate.Value.Month, secondaryStartDate.Value.Day);

				while (cur <= endDate)
				{
					BillTransaction trx = new BillTransaction()
					{
						BillId = bill.Id,
						Amount = amount,
						OriginalAmount = amount,
						CategoryId = categoryId,
						OriginalCategoryId = categoryId,
						VendorId = vendorId,
						OriginalVendorId = vendorId,
						Timestamp = cur,
						OriginalTimestamp = cur
					};
					billTransactions.Add(trx);

					count++;
					if (frequency == 0)
						cur = endDate.AddDays(1);
					else if (frequency > 0)
						cur = secondaryStartDate.Value.AddDays(count * (int)frequency);
					else
						cur = secondaryStartDate.Value.AddMonths(count * -1 * (int)frequency);
				}
			}

			_db.Insert<BillTransaction>(billTransactions);

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

			// delete bill transactions for this bill
			var deleteBillTransactionsResult = _db.Delete<BillTransaction>(Predicates.Field<BillTransaction>(x => x.BillId, Operator.Eq, billId));
			if (!deleteBillTransactionsResult)
			{
				result.AddError(ErrorType.Generic, "Error deleting Bill Transactions with BillId {0}", billId);
				return result;
			}

			// delete the bill
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

			// delete the bill group
			var deletionResult = _db.Delete<BillGroup>(Predicates.Field<BillGroup>(x => x.Id, Operator.Eq, billGroupId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Bill Group {0} not found", billGroupId);
				return result;
			}

			result.Result = deletionResult;
			return result;
		}

		public ServiceResult<bool> DeleteBillTransaction(int billTransactionId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			// delete the bill transaction
			var deletionResult = _db.Delete<BillTransaction>(Predicates.Field<BillTransaction>(x => x.Id, Operator.Eq, billTransactionId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Bill Transaction {0} not found", billTransactionId);
				return result;
			}

			result.Result = deletionResult;
			return result;
		}

		public ServiceResult<Bill> UpdateBill(int billId, string name, decimal amount, BillFrequency frequency, DateTime startDate, DateTime endDate, int? billGroupId = null, int? categoryId = null, int? vendorId = null, DateTime? secondaryStartDate = null, DateTime? secondaryEndDate = null, bool updateExisting = false)
		{
			var result = new ServiceResult<Bill>();

			var billResult = GetBill(billId);
			if (billResult.HasErrors)
			{
				result.AddErrors(billResult);
				return result;
			}

			// if the new value is 0, that means set it to null
			if (billGroupId.HasValue && billGroupId.Value == 0)
				billGroupId = null;
			if (categoryId.HasValue && categoryId.Value == 0)
				categoryId = null;
			if (vendorId.HasValue && vendorId.Value == 0)
				vendorId = null;

			// do the new BillGroup, Category and Vendor exist?
			if (billGroupId.HasValue)
			{
				var billGroupResult = GetBillGroup(billGroupId.Value);
				if (billGroupResult.HasErrors)
				{
					result.AddErrors(billGroupResult);
					return result;
				}
			}
			if (categoryId.HasValue)
			{
				var categoryResult = _categoryService.GetCategory(categoryId.Value);
				if (categoryResult.HasErrors)
				{
					result.AddErrors(categoryResult);
					return result;
				}
			}
			if (vendorId.HasValue)
			{
				var vendorResult = _vendorService.GetVendor(vendorId.Value);
				if (vendorResult.HasErrors)
				{
					result.AddErrors(vendorResult);
					return result;
				}
			}

			// TODO handle updateExisting
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

			billResult.Result.Name = name;
			billResult.Result.Amount = amount;
			billResult.Result.RecurrenceFrequency = frequency;
			billResult.Result.StartDate = startDate;
			billResult.Result.EndDate = endDate;
			billResult.Result.BillGroupId = billGroupId;
			billResult.Result.CategoryId = categoryId;
			billResult.Result.VendorId = vendorId;
			billResult.Result.StartDate2 = secondaryStartDate;
			billResult.Result.EndDate2 = secondaryEndDate;
			_db.Update<Bill>(billResult.Result);

			result.Result = billResult.Result;
			return result;
		}

		public ServiceResult<BillTransaction> UpdateBillTransaction(int billTransactionId, decimal? amount, DateTime? date, bool? isPaid, int? transactionId)
		{
			var result = new ServiceResult<BillTransaction>();

			// does the bill transaction exist?
			var billTransactionResult = GetBillTransaction(billTransactionId);
			if (billTransactionResult.HasErrors)
			{
				result.AddErrors(billTransactionResult);
				return result;
			}

			// does the transaction exist?
			if (transactionId.HasValue)
			{
				var transactionResult = _transactionService.GetTransaction(transactionId.Value);
				if (transactionResult.HasErrors)
				{
					result.AddErrors(transactionResult);
					return result;
				}

				// TODO remove any other transactions that have this billTransactionId

				// only update the bill transaction if it's different from the current one
				if (transactionResult.Result.BillTransactionId != billTransactionId)
				{
					transactionResult.Result.BillTransactionId = billTransactionId;
					_transactionService.UpdateTransaction(transactionResult.Result.Id, billTransactionId: billTransactionId);
				}
			}

			if (amount.HasValue)
				billTransactionResult.Result.Amount = amount.Value;
			if (date.HasValue)
				billTransactionResult.Result.Timestamp = date.Value;
			if (isPaid.HasValue)
				billTransactionResult.Result.IsPaid = isPaid.Value;

			_db.Update<BillTransaction>(billTransactionResult.Result);

			result.Result = billTransactionResult.Result;
			return result;
		}

		public ServiceResult<List<BillTransactionPrediction>> PredictBillTransactionMatch(int billTransactionId)
		{
			var result = new ServiceResult<List<BillTransactionPrediction>>();

			// get the bill transaction
			var billTransactionResult = GetBillTransaction(billTransactionId);
			if (billTransactionResult.HasErrors)
			{
				result.AddErrors(billTransactionResult);
				return result;
			}

			// get the vendor name
			string billTransactionVendorName = "";
			if (billTransactionResult.Result.VendorId.HasValue)
			{
				var vendorResult = _vendorService.GetVendor(billTransactionResult.Result.VendorId.Value);
				if (vendorResult.HasErrors)
				{
					result.AddErrors(vendorResult);
					return result;
				}

				billTransactionVendorName = vendorResult.Result.Name;
			}

			// get all transactions
			var transactions = _db.Connection.Query(@"
SELECT t.Id, t.Amount, t.Timestamp, v.Name
FROM [Transaction] t
LEFT JOIN Vendor v on v.Id = t.VendorId
");

			// calculate confidences
			var confidences = transactions.Select(t => new BillTransactionPrediction()
				{
					BillTransactionId = billTransactionId,
					TransactionId = t.Id,
					Amount = t.Amount,
					Timestamp = t.Timestamp,
					VendorName = t.Name,

					AmountConfidence = (decimal)Math.Exp(-1 * Math.Pow((double)((t.Amount - billTransactionResult.Result.Amount) / billTransactionResult.Result.Amount), 2.0) / 2.0),
					TimestampConfidence = (decimal)Math.Exp(-1 * Math.Pow(((t.Timestamp - billTransactionResult.Result.Timestamp).TotalDays / 60.0), 2.0)),
					VendorNameConfidence = (decimal)Math.Exp(-1 * Math.Pow(LevenshteinDistance.Compute(t.Name == null ? "" : t.Name, billTransactionVendorName) / 20.0, 2.0))
				});
			
			// order by confidence
			result.Result = confidences
				.OrderByDescending(x => x.Confidence)
				.Take(5)
				.ToList();

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

using Dapper;
using DapperExtensions;
using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public class TransactionService : EntityService, ITransactionService
	{
		private IAccountService _accountService;
		private ICategoryService _categoryService;
		private IVendorService _vendorService;
		private IBillService _billService;

		public TransactionService(IDatabase database, IAccountService accountService, ICategoryService categoryService, IVendorService vendorService, IBillService billService)
			: base(database)
		{
			_accountService = accountService;
			_categoryService = categoryService;
			_vendorService = vendorService;
			_billService = billService;
		}

		private ServiceResult<IEnumerable<Transaction>> GetTransactions(Filter<int> transactionId = null, Filter<int?> accountId = null, Filter<int?> categoryId = null, Filter<int?> vendorId = null, DateTime? from = null, DateTime? to = null)
		{
			var result = new ServiceResult<IEnumerable<Transaction>>();
			var parameters = new List<dynamic>();

			var builder = new StringBuilder();
			builder.Append(@"
FROM [Transaction] t
JOIN Subtransaction st ON st.TransactionId = t.Id
LEFT JOIN Account a on a.Id = t.AccountId
LEFT JOIN Vendor v on v.Id = t.VendorId
LEFT JOIN Category c on c.Id = st.CategoryId
");
			var whereClauses = new List<string>();

			// transaction clause
			if (transactionId != null)
			{
				// make sure transaction exists
				var transactionResult = base.GetEntity<Transaction>(transactionId.Object);
				if (transactionResult.HasErrors)
				{
					result.AddErrors(transactionResult);
					return result;
				}
				whereClauses.Add("t.Id = @transactionId");
				parameters.Add(new { transactionId = transactionId.Object });
			}

			// account clause
			if (accountId != null)
			{
				// make sure account exists
				if (accountId.Object.HasValue)
				{
					var accountResult = _accountService.GetAccount(accountId.Object.Value);
					if (accountResult.HasErrors)
					{
						result.AddErrors(accountResult);
						return result;
					}
				}
				whereClauses.Add("t.AccountId = @accountId");
				parameters.Add(new { accountId = accountId.Object });
			}

			// category clause
			if (categoryId != null)
			{
				// make sure category exists
				if (categoryId.Object.HasValue)
				{
					var categoryResult = _categoryService.GetCategory(categoryId.Object.Value);
					if (categoryResult.HasErrors)
					{
						result.AddErrors(categoryResult);
						return result;
					}
				}
				whereClauses.Add("st.CategoryId = @categoryId");
				parameters.Add(new { categoryId = categoryId.Object });
			}

			// vendor clause
			if (vendorId != null)
			{
				// make sure vendor exists
				if (vendorId.Object.HasValue)
				{
					var vendorResult = _vendorService.GetVendor(vendorId.Object.Value);
					if (vendorResult.HasErrors)
					{
						result.AddErrors(vendorResult);
						return result;
					}
				}
				whereClauses.Add("t.VendorId = @vendorId");
				parameters.Add(new { vendorId = vendorId.Object });
			}

			// start/end clauses
			if (from.HasValue)
			{
				whereClauses.Add("t.Timestamp >= @from");
				parameters.Add(new { from = new DateTime(Math.Max(SqlDateTime.MinValue.Value.Ticks, from.Value.Ticks)) });
			}
				

			if (to.HasValue)
			{
				whereClauses.Add("t.Timestamp <= @to");
				parameters.Add(new { to = new DateTime(Math.Min(SqlDateTime.MaxValue.Value.Ticks, to.Value.Ticks)) });
			}
				
			if (whereClauses.Any())
			{
				builder.Append("WHERE ");
				builder.Append(String.Join(" AND ", whereClauses));
			}

			// get transactions and subtransactions separately
			var sql = builder.ToString();
			object mergedParameters = MergeParameters(parameters);
			var transactions = _db.Connection.Query<Transaction>("SELECT t.*, a.Name as AccountName, v.Name as VendorName " + sql, mergedParameters);
            var subtransactions = _db.Connection.Query<Subtransaction>("SELECT st.*, c.Name as CategoryName " + sql, mergedParameters);

			// stitch the subtransactions into the transactions
			foreach (var t in transactions)
			{
				t.Subtransactions = subtransactions.Where(x => x.TransactionId == t.Id);
			}

			result.Result = transactions;
			return result;
		}

		public ServiceResult<Transaction> GetTransaction(int transactionId)
		{
			var result = new ServiceResult<Transaction>();

			var transactionsResult = GetTransactions(new Filter<int>(transactionId));
			if (transactionsResult.HasErrors)
			{
				result.AddErrors(transactionsResult);
				return result;
			}

			if (transactionsResult.Result.Count() > 1)
			{
				result.AddError(ErrorType.Generic, "More than one transaction with Id {0} found", transactionId);
				return result;
			}

			result.Result = transactionsResult.Result.First();
			return result;
		}

		public ServiceResult<IEnumerable<Transaction>> GetTransactions(Filter<int?> accountId = null, Filter<int?> categoryId = null, Filter<int?> vendorId = null, DateTime? from = null, DateTime? to = null)
		{
			return GetTransactions(transactionId: null, accountId: accountId, categoryId: categoryId, vendorId: vendorId, from: from, to: to);
		}

		public ServiceResult<Transaction> AddTransaction(int accountId, DateTime timestamp, decimal amount, string memo = null, string notes = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false, int? categoryId = null, int? vendorId = null, int? billTransactionId = null)
		{
			var result = new ServiceResult<Transaction>();

			// does account exist?
			var accountResult = _accountService.GetAccount(accountId);
			if (accountResult.HasErrors)
			{
				result.AddErrors(accountResult);
				return result;
			}

			// add transaction
			var transaction = new Transaction()
			{
				AccountId = accountId,
				Timestamp = timestamp,
				OriginalTimestamp = timestamp,
				Amount = amount,
				IsReconciled = isReconciled,
				VendorId = vendorId,
				BillTransactionId = billTransactionId
			};

			_db.Insert<Transaction>(transaction);

			// add subtransaction
			var subTransaction = new Subtransaction()
			{
				TransactionId = transaction.Id,
				Amount = amount,
				CategoryId = categoryId,
				Memo = memo,
				Notes = notes,
				IsTransfer = isTransfer,
				IsExcludedFromBudget = isExcludedFromBudget,
			};

			_db.Insert<Subtransaction>(subTransaction);
			transaction.Subtransactions = new List<Subtransaction>() { subTransaction };

			_accountService.UpdateAccountBalances();

			result.Result = transaction;
			return result;
		}

		public ServiceResult<bool> DeleteTransaction(int transactionId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

			// delete subtransactions
			_db.Delete<Subtransaction>(Predicates.Field<Subtransaction>(x => x.TransactionId, Operator.Eq, transactionId));

			var deletionResult = _db.Delete<Transaction>(Predicates.Field<Transaction>(x => x.Id, Operator.Eq, transactionId));
			if (!deletionResult)
			{
				result.AddError(ErrorType.NotFound, "Transaction {0} not found", transactionId);
				return result;
			}

			_accountService.UpdateAccountBalances();

			result.Result = deletionResult;
			return result;
		}

		public ServiceResult<Transaction> UpdateTransaction(int transactionId, Filter<int> accountId = null, DateTime? timestamp = null, decimal? amount = null, string memo = null, string notes = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false, Filter<int?> categoryId = null, Filter<int?> vendorId = null, Filter<int?> billTransactionId = null)
		{
			var result = new ServiceResult<Transaction>();

			var transactionResult = GetTransaction(transactionId);
			if (transactionResult.HasErrors)
			{
				result.AddErrors(transactionResult);
				return result;
			}

			// account update
			if (accountId != null)
			{
				// make sure account exists
				var accountResult = _accountService.GetAccount(accountId.Object);
				if (accountResult.HasErrors)
				{
					result.AddErrors(accountResult);
					return result;
				}

				transactionResult.Result.AccountId = accountId.Object;
			}

			// category update
			if (categoryId != null)
			{
				// make sure category exists
				if (categoryId.Object.HasValue)
				{
					var categoryResult = _categoryService.GetCategory(categoryId.Object.Value);
					if (categoryResult.HasErrors)
					{
						result.AddErrors(categoryResult);
						return result;
					}
				}

				foreach (var s in transactionResult.Result.Subtransactions)
				{
					s.CategoryId = categoryId.Object;
				}
			}

			// vendor update
			if (vendorId != null)
			{
				// make sure vendor exists
				if (vendorId.Object.HasValue)
				{
					var vendorResult = _vendorService.GetVendor(vendorId.Object.Value);
					if (vendorResult.HasErrors)
					{
						result.AddErrors(vendorResult);
						return result;
					}
				}

				transactionResult.Result.VendorId = vendorId.Object;
			}

			// bill transaction update
			if (billTransactionId != null)
			{
				// make sure bill transaction exists
				if (billTransactionId.Object.HasValue)
				{
					var billTransactionResult = _billService.GetBillTransaction(billTransactionId.Object.Value);
					if (billTransactionResult.HasErrors)
					{
						result.AddErrors(billTransactionResult);
						return result;
					}
				}

				transactionResult.Result.BillTransactionId = billTransactionId.Object;
			}

			if (timestamp.HasValue)
				transactionResult.Result.Timestamp = timestamp.Value;

			if (amount.HasValue)
				transactionResult.Result.Amount = amount.Value;

			foreach (var s in transactionResult.Result.Subtransactions)
			{
				// TODO how does the amount parameter relate to the subtransaction values?
				s.Memo = memo;
				s.Notes = notes;
				s.IsExcludedFromBudget = isExcludedFromBudget;
				s.IsTransfer = isTransfer;
			}

			transactionResult.Result.IsReconciled = isReconciled;

			// update database
			_db.Update<Transaction>(transactionResult.Result);
			foreach (var s in transactionResult.Result.Subtransactions)
			{
				_db.Update<Subtransaction>(s);
			}

			_accountService.UpdateAccountBalances();

			result.Result = transactionResult.Result;
			return result;
		}
	}
}

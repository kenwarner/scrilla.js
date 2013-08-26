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
	public class TransactionService : EntityService, ITransactionService
	{
		private IAccountService _accountService;
		private ICategoryService _categoryService;
		private IVendorService _vendorService;

		public TransactionService(IDatabase database, IAccountService accountService, ICategoryService categoryService, IVendorService vendorService)
			: base(database)
		{
			_accountService = accountService;
			_categoryService = categoryService;
			_vendorService = vendorService;
		}

		private ServiceResult<IEnumerable<Transaction>> GetTransactions(Filter<int> transactionId = null, Filter<int?> accountId = null, Filter<int?> categoryId = null, Filter<int?> vendorId = null, DateTime? from = null, DateTime? to = null)
		{
			var result = new ServiceResult<IEnumerable<Transaction>>();
			var predicates = new List<IPredicate>();

			var builder = new StringBuilder();
			builder.Append(@"
FROM [Transaction] t
JOIN Subtransaction st ON st.TransactionId = t.Id
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
				whereClauses.Add("t.Id = " + transactionId.Object);
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
				whereClauses.Add("t.AccountId = " + accountId.Object);
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
				whereClauses.Add("st.CategoryId = " + categoryId.Object);
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
				whereClauses.Add("t.VendorId = " + vendorId.Object);
			}

			// start/end clauses
			if (from.HasValue)
				whereClauses.Add("t.Timestamp >= " + from.Value.ToShortDateString());

			if (to.HasValue)
				whereClauses.Add("t.Timestamp <= " + to.Value.ToShortDateString());

			if (whereClauses.Any())
			{
				builder.Append("WHERE ");
				builder.Append(String.Join(" AND ", whereClauses));
			}

			// get transactions and subtransactions separately
			var sql = builder.ToString();
			var transactions = _db.Connection.Query<Transaction>("SELECT t.* " + sql);
			var subtransactions = _db.Connection.Query<Subtransaction>("SELECT st.* " + sql);

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
			return GetTransactions(transactionId: null, accountId: accountId, vendorId: vendorId, from: from, to: to);
		}

		public ServiceResult<Transaction> AddTransaction(int accountId, DateTime timestamp, decimal amount, string memo = null, string notes = null, int? categoryId = null, int? vendorId = null, int? billTransactionId = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false)
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

			result.Result = deletionResult;
			return result;
		}

		public ServiceResult<bool> UpdateTransaction(int transactionId, int? accountId = null, DateTime? timestamp = null, decimal? amount = null, string memo = null, string notes = null, int? categoryId = null, int? vendorId = null, int? billTransactionId = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false)
		{
			throw new NotImplementedException();

			var result = new ServiceResult<bool>();


			return result;
		}
	}
}

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

		public TransactionService(IDatabase database, IAccountService accountService)
			: base(database)
		{
			_accountService = accountService;
		}

		public ServiceResult<Transaction> GetTransaction(int transactionId)
		{
			return base.GetEntity<Transaction>(transactionId);
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
			var result = new ServiceResult<IEnumerable<Transaction>>();

			var accountResult = _accountService.GetAccount(accountId);
			if (accountResult.HasErrors)
			{
				result.AddErrors(accountResult);
				return result;
			}

			var predicates = new List<IPredicate>();
			predicates.Add(Predicates.Field<Transaction>(x => x.AccountId, Operator.Eq, accountId));

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
				Amount = amount,
				CategoryId = categoryId,
				Memo = memo,
				Notes = notes,
				IsTransfer = isTransfer,
				IsExcludedFromBudget = isExcludedFromBudget,
				TransactionId = transaction.Id
			};

			result.Result = transaction;
			return result;
		}

		public ServiceResult<bool> DeleteTransaction(int transactionId)
		{
			// TODO handle cascading deletes
			var result = new ServiceResult<bool>();

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

using DapperExtensions;
using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly IDatabase _db;

		public TransactionService(IDatabase database)
		{
			_db = database;
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


		public ServiceResult<bool> UpdateTransactionReconciled(int transactionId, bool isReconciled)
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

		public ServiceResult<bool> UpdateTransactionCategory(int transactionId, int? categoryId)
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

		public ServiceResult<bool> UpdateTransactionVendor(int transactionId, int? vendorId)
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

		public ServiceResult<bool> UpdateTransactionDate(int transactionId, DateTime date)
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


	}
}

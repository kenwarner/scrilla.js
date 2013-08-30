using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public interface ITransactionService
	{
		ServiceResult<Transaction> GetTransaction(int transactionId);

		ServiceResult<IEnumerable<Transaction>> GetTransactions(Filter<int?> accountId = null, Filter<int?> categoryId = null, Filter<int?> vendorId = null, DateTime? from = null, DateTime? to = null);

		ServiceResult<Transaction> AddTransaction(int accountId, DateTime timestamp, decimal amount, string memo = null, string notes = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false, int? categoryId = null, int? vendorId = null, int? billTransactionId = null);

		ServiceResult<bool> DeleteTransaction(int transactionId);

		ServiceResult<bool> UpdateTransaction(int transactionId, int? accountId = null, DateTime? timestamp = null, decimal? amount = null, string memo = null, string notes = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false, int? categoryId = null, int? vendorId = null, int? billTransactionId = null);
	}
}

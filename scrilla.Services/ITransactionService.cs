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

		ServiceResult<IEnumerable<Transaction>> GetAllTransactions(DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactions(int accountId, int? categoryId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByVendor(int? vendorId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByCategory(int? categoryId, DateTime? from = null, DateTime? to = null);

		ServiceResult<Transaction> AddTransaction(int accountId, DateTime timestamp, decimal amount, string memo = null, string notes = null, int? categoryId = null, int? vendorId = null, int? billTransactionId = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false);

		ServiceResult<bool> DeleteTransaction(int transactionId);

		ServiceResult<bool> UpdateTransaction(int transactionId, int? accountId = null, DateTime? timestamp = null, decimal? amount = null, string memo = null, string notes = null, int? categoryId = null, int? vendorId = null, int? billTransactionId = null, bool isReconciled = false, bool isExcludedFromBudget = false, bool isTransfer = false);
	}
}

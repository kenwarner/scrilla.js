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
		ServiceResult<IEnumerable<Transaction>> GetAllTransactions(DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactions(int accountId, int? categoryId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByVendor(int? vendorId, DateTime? from = null, DateTime? to = null);
		ServiceResult<IEnumerable<Transaction>> GetTransactionsByCategory(int? categoryId, DateTime? from = null, DateTime? to = null);

		ServiceResult<bool> UpdateTransactionReconciled(int transactionId, bool isReconciled);
		ServiceResult<bool> UpdateTransactionCategory(int transactionId, int? categoryId);
		ServiceResult<bool> UpdateTransactionVendor(int transactionId, int? vendorId);
		ServiceResult<bool> UpdateTransactionDate(int transactionId, DateTime date);

	}
}

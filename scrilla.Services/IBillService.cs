using scrilla.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	public interface IBillService
	{
		ServiceResult<Bill> GetBill(int billId);
		ServiceResult<BillGroup> GetBillGroup(int billGroupId);

		ServiceResult<IEnumerable<Bill>> GetAllBills();
		ServiceResult<IEnumerable<BillGroup>> GetAllBillGroups();

		ServiceResult<IEnumerable<BillTransaction>> GetBillTransactions(int? billId, DateTime? from, DateTime? to);

		ServiceResult<Bill> AddBill(string name, decimal amount, int? billGroupId, int? categoryId, int? vendorId, DateTime startDate, DateTime endDate, BillFrequency frequency, DateTime? secondaryStartDate, DateTime? secondaryEndDate);
		ServiceResult<BillGroup> AddBillGroup(string name, int displayOrder, bool isActive);

		ServiceResult<bool> DeleteBill(int billId);
		ServiceResult<bool> DeleteBillGroup(int billGroupId);

		ServiceResult<Bill> UpdateBill(int billId, string name, decimal amount, int billGroupId, int? categoryId, int? vendorId, DateTime startDate, DateTime endDate, int frequency, bool updateExisting, DateTime? secondaryStartDate, DateTime? secondaryEndDate);
		ServiceResult<BillTransaction> UpdateBillTransaction(int billTransactionId, decimal? amount, DateTime? date, bool? isPaid, int? transactionId);
		ServiceResult<List<Tuple<Transaction, double>>> PredictBillTransactionMatch(int billTransactionId);
	}
}

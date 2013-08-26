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
		ServiceResult<BillTransaction> GetBillTransaction(int billTransactionId);

		ServiceResult<IEnumerable<Bill>> GetAllBills();
		ServiceResult<IEnumerable<BillGroup>> GetAllBillGroups();

		ServiceResult<IEnumerable<BillTransaction>> GetBillTransactions(int? billId, DateTime? from = null, DateTime? to = null);

		ServiceResult<Bill> AddBill(string name, decimal amount, BillFrequency frequency, DateTime startDate, DateTime endDate, int? billGroupId = null, int? categoryId = null, int? vendorId = null, DateTime? secondaryStartDate = null, DateTime? secondaryEndDate = null);
		ServiceResult<BillGroup> AddBillGroup(string name, int displayOrder, bool isActive);

		ServiceResult<bool> DeleteBill(int billId);
		ServiceResult<bool> DeleteBillGroup(int billGroupId);
		ServiceResult<bool> DeleteBillTransaction(int billTransactionId);

		ServiceResult<Bill> UpdateBill(int billId, string name, decimal amount, BillFrequency frequency, DateTime startDate, DateTime endDate, int? billGroupId = null, int? categoryId = null, int? vendorId = null, DateTime? secondaryStartDate = null, DateTime? secondaryEndDate = null, bool updateExisting = false);
		ServiceResult<BillTransaction> UpdateBillTransaction(int billTransactionId, decimal? amount, DateTime? date, bool? isPaid, int? transactionId);
		ServiceResult<List<BillTransactionPrediction>> PredictBillTransactionMatch(int billTransactionId);
	}
}

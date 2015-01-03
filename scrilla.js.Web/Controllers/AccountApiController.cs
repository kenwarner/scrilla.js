using scrilla.Data;
using scrilla.js.Web.Models;
using scrilla.Services;
using scrilla.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace scrilla.js.Web.Controllers
{
	public partial class AccountApiController : ApiController
	{
		private readonly IAccountService _accountService;
		private readonly ITransactionService _transactionService;

		public AccountApiController(IAccountService accountService, ITransactionService transactionService)
		{
			_accountService = accountService;
			_transactionService = transactionService;
		}

		[Route("api/accounts/balances"), HttpGet]
		public virtual HttpResponseMessage Accounts(DateTime? from = null, DateTime? to = null)
		{
			var dateRange = new DateRangeModel(from, to);
			var result = _accountService.GetAccountBalances(dateRange.From, dateRange.To);
			return Request.CreateResponse<AccountBalancesModel>(result);
		}

		[Route("api/transactions"), HttpGet]
		public virtual HttpResponseMessage Transactions(string accountId = "", string vendorId = "", string categoryId = "", DateTime? from = null, DateTime? to = null)
		{
			var dateRange = new DateRangeModel(from, to);
			var result = _transactionService.GetTransactions(Filter<int?>.Parse(accountId), Filter<int?>.Parse(categoryId), Filter<int?>.Parse(vendorId), dateRange.From, dateRange.To);
			return Request.CreateResponse<IEnumerable<Transaction>>(result);
		}

		[Route("api/transactions/recent"), HttpGet]
		public virtual HttpResponseMessage RecentTransactions(string accountId = "", string vendorId = "", string categoryId = "")
		{
			var dateRange = new DateRangeModel(new DateTime(2012, 1, 1), new DateTime(2012, 6, 1));
			var result = _transactionService.GetTransactions(Filter<int?>.Parse(accountId), Filter<int?>.Parse(categoryId), Filter<int?>.Parse(vendorId), dateRange.From, dateRange.To);
			return Request.CreateResponse<IEnumerable<Transaction>>(result);
		}

	}
}

namespace System.Net.Http
{
	public static class ServiceResultExtensions
	{
		public static HttpResponseMessage CreateResponse<T>(this HttpRequestMessage request, ServiceResult<T> result)
		{
			if (result.HasErrors && result.ErrorMessages.Any(x => x.Key == ErrorType.Security))
				return request.CreateResponse(HttpStatusCode.Unauthorized, result.ErrorMessages);

			if (result.HasErrors && result.ErrorMessages.Any(x => x.Key == ErrorType.NotFound))
				return request.CreateResponse(HttpStatusCode.NotFound, result.ErrorMessages);

			return request.CreateResponse(HttpStatusCode.OK, result.Result);
		}
	}
}

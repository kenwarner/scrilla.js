using AttributeRouting.Web.Http;
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

		public AccountApiController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[GET("api/accounts/balances"), HttpGet]
		public virtual HttpResponseMessage Accounts(DateTime? from = null, DateTime? to = null)
		{
			return Request.CreateResponse<AccountBalancesModel>(_accountService.GetAccountBalances(from, to));
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

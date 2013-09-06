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

		[GET("api/accounts"), HttpGet]
        public virtual HttpResponseMessage Accounts(DateTime? from = null, DateTime? to = null)
        {
			var model = new DateRangeViewModel(from, to); // fill in defaults if null
			return Request.CreateResponse(HttpStatusCode.OK, _accountService.GetAccounts(model.From, model.To));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace scrilla.js.Web.Controllers
{
	public partial class AccountController : Controller
    {
        //
        // GET: /Account/

		//[GET("", ActionPrecedence = 1)]
		//[GET("Accounts")]
		public virtual ActionResult Accounts()
        {
            return View();
        }

    }
}

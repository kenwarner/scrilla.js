using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace scrilla.js.Web.Controllers
{
    public class AccountApiController : ApiController
    {
        // GET api/accountapi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/accountapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/accountapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/accountapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/accountapi/5
        public void Delete(int id)
        {
        }
    }
}

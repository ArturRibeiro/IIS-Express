
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Scorponok.Web.Api.Controllers
{
    [RoutePrefix("Home")]
    public class HelloWorldController : ApiController
    {
        [HttpGet, Route("Index")]
        public HttpResponseMessage Index()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("Hello World!!!", System.Text.Encoding.UTF8, "application/json");
            return response;
        }
    }
}

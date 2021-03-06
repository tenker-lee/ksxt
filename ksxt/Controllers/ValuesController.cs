﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ksxt.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
        public string Get(string s)
        {
            return "123";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {
            HttpResponseMessage httpResponseMessage = Request.CreateResponse();
            httpResponseMessage.Content = new StringContent("{\"abc\":123}");
            return httpResponseMessage;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
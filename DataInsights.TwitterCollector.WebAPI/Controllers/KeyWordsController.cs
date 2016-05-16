using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Reflection;
using TwitterConnector.WebAPI;

namespace TwitterConnector.WebAPI.Controllers
{
    public class KeyWordsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            String KeyWords = File.ReadAllText(Constants.KeyWordsFileLocation);
            var ret = new KeyValuePair<string, String[]>("key", KeyWords.Split(','));
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
    }
}

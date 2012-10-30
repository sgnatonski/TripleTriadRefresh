using System;
using System.Linq;
using System.Web.Mvc;

namespace TripleTriadRefresh.Server.Controllers.Result
{
    public class LoginResult : JsonNetResult
    {
        public LoginResult(string id)
        {
            Data = string.Format("https://graph.facebook.com/{0}/picture", id);
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }
    }
}
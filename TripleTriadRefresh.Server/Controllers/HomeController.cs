using System;
using System.Linq;
using System.Web.Mvc;
using TripleTriadRefresh.Server.Controllers.Result;
using TripleTriadRefresh.Server.Framework;
using TripleTriadRefresh.Server.Models;

namespace TripleTriadRefresh.Server.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonNetResult GetNews()
        {
            var json = new JsonNetResult();
            return json;
        }
    }
}

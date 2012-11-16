using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
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

            var cachedNews = this.HttpContext.Cache.Get("news");
            if (cachedNews == null)
            {
                json.Data = GetNewsCollection();

                if (!Debugger.IsAttached)
                {
                    this.HttpContext.Cache.Add("news", json.Data, null, DateTime.Now.AddMinutes(60), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                } 
            }
            else
            {
                json.Data = cachedNews;
            }
            
            return json;
        }

        private List<News> GetNewsCollection()
        {
            var jsonNews = new List<News>();
            var document = XDocument.Load(Server.MapPath("~/Data/News/news.xml"));
            if (document.Root != null)
            {
                var messages = JObject.Parse(JsonConvert.SerializeXNode(document.Root)).Children().First().Children()["message"].First();

                jsonNews = messages.Select(c => c.ToObject<News>()).ToList();
                jsonNews.ForEach(n => {
                    n.Texts = n.Text.Split(new[] { '\n' })
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();
                    n.Text = null;
                });
            }
            return jsonNews;
        }

        public class News
        {
            [JsonProperty("@title")]
            public string Title { get; set; }
            [JsonProperty("@type")]
            public string Type { get; set; }
            [JsonProperty("@version")]
            public string Version { get; set; }
            [JsonProperty("@date")]
            public string Date { get; set; }
            [JsonProperty("#text")]
            public string Text { get; set; }
            [JsonProperty("texts")]
            public List<string> Texts { get; set; }
        }
    }
}

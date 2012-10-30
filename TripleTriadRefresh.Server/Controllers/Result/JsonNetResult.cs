using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using StructureMap;
using SignalR;

namespace TripleTriadRefresh.Server.Controllers.Result
{
    public class JsonNetResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            var serializedObject = ObjectFactory.GetInstance<IJsonSerializer>().Stringify(Data);
            response.Write(serializedObject);
        }
    }
}
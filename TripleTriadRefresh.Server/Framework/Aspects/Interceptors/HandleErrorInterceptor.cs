using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using Castle.DynamicProxy;
using Fasterflect;
using TripleTriadRefresh.Server.Hubs;

namespace TripleTriadRefresh.Server.Framework.Aspects.Interceptors
{
    public class HandleErrorInterceptor : AspectMap.AttributeHandler
    {
        public override Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute)
        {
            return i =>
            {
                try
                {
                    invocation(i);
                }
                catch (Exception ex)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(i.Method.Name + " throws exception: " + ex.Message);
                    sb.AppendLine(HttpContext.Current.Request.UrlReferrer.ToString());
                    sb.AppendLine(HttpContext.Current.Request.UserAgent);
                    sb.AppendLine(HttpContext.Current.User.Identity.Name);

                    Debug.WriteLine(sb.ToString());
                    log4net.LogManager.GetLogger(i.TargetType.FullName).Error(sb.ToString(), ex);

                    var hub = (GameHub)i.InvocationTarget.GetPropertyValue("Hub", Flags.AllMembers);
                    hub.Clients.Caller.receiveError("Something bad happened.");
                }
            };
        }
    }
}
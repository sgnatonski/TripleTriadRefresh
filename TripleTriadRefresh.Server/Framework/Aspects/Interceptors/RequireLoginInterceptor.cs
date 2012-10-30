using System;
using System.Web;
using System.Web.Security;
using Castle.DynamicProxy;

namespace TripleTriadRefresh.Server.Framework.Aspects.Interceptors
{
    public class RequireLoginInterceptor : AspectMap.AttributeHandler
    {
        public override Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute)
        {
            return i =>
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    invocation(i);
                }
                else
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                    throw new HttpException(401, "Unauthorized access");
                }
            };
        }
    }
}
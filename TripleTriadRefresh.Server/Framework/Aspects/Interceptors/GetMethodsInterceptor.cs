using System;
using System.Diagnostics;
using System.Web;
using Castle.DynamicProxy;

namespace TripleTriadRefresh.Server.Framework.Aspects.Interceptors
{
    public class GetMethodsInterceptor : AspectMap.AttributeHandler
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
                    throw new HttpException(401, "Unauthorized access");
                }
            };
        }
    }
}
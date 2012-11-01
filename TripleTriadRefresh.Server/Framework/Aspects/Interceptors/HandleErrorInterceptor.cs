using System;
using System.Diagnostics;
using Castle.DynamicProxy;

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
                    Debug.WriteLine(i.Method.Name + " throws exception: " + ex.Message);
                    Debug.WriteLine(i.Method.Name + ": " + ex.StackTrace);
                }
            };
        }
    }
}
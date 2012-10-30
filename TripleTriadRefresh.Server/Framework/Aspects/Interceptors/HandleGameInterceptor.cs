using System;
using System.Diagnostics;
using System.Reflection;
using Castle.DynamicProxy;

namespace TripleTriadWeb.Framework.Aspects.Interceptors
{
    public class HandleGameInterceptor : AspectMap.AttributeHandler
    {
        public override Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute)
        {
            return i =>
            {
                invocation(i);
            };
        }
    }
}
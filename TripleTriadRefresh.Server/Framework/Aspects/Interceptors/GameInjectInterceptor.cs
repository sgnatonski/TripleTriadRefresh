using System;
using Castle.DynamicProxy;
using Fasterflect;
using StructureMap;
using TripleTriadRefresh.Server.Framework.Aspects.Attributes;
using TripleTriadRefresh.Server.Hubs;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Framework.Aspects.Interceptors
{
    public class GameInjectInterceptor : AspectMap.AttributeHandler
    {
        public override Action<IInvocation> Surround(Action<IInvocation> invocation, Attribute sourceAttribute)
        {
            return i =>
            {
                var attr = (GameInjectAttribute)sourceAttribute;
                var source = ObjectFactory.GetInstance<IGameContainer>();
                var hub = (GameHub)i.InvocationTarget.GetPropertyValue("Hub", Flags.AllMembers);

                Game parameter = null;

                if (attr.KeyIndex.HasValue)
                {
                    parameter = source.GetGame(i.Arguments[attr.KeyIndex.Value].ToString());
                }
                else
                {
                    parameter = source.GetGame(hub.Context);
                }

                if (parameter != null)
                {
                    i.Arguments[attr.ParameterIndex] = parameter;
                    invocation(i);
                }
                else
                {
                    hub.Caller.gameLeft();
                    hub.Caller.receiveError(attr.Error);
                }
            };
        }
    }
}
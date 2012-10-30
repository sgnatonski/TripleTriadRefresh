using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using SignalR;
using SignalR.Hubs;
using StructureMap;

namespace TripleTriadRefresh.Server.Framework
{
    public class HubActivator : IHubActivator
    {
        private readonly IHubActivator defaultActivator;
        public HubActivator(IDependencyResolver resolver)
        {
            defaultActivator = new DefaultHubActivator(resolver);
        }

        public IHub Create(HubDescriptor descriptor)
        {
            var hub = ObjectFactory.GetInstance(Type.GetType("TripleTriadRefresh.Web.Hubs." + descriptor.Name));
            /* = defaultActivator.Create(descriptor);*/
            return (IHub)(hub);
        }
    }
}
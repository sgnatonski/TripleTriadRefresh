using System;
using System.Collections.Generic;
using System.Linq;
using SignalR;
using StructureMap;

namespace TripleTriadRefresh.Server.Framework
{
    public class StructureMapDependencyResolver : DefaultDependencyResolver
    {
        readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
            // TODO: if you haven't registered necessary interfaces somewhere else, you'll need to do so here.
        }

        public override object GetService(Type serviceType)
        {
            if (serviceType.IsClass)
            {
                return GetConcreteService(serviceType);
            }
            else
            {
                var r = GetInterfaceService(serviceType);
                return r;
            }
        }

        private object GetConcreteService(Type serviceType)
        {
            try
            {
                // Can't use TryGetInstance here because it won’t create concrete types
                return _container.GetInstance(serviceType);
            }
            catch (StructureMapException)
            {
                return null;
            }
        }

        private object GetInterfaceService(Type serviceType)
        {
            return _container.TryGetInstance(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>().Concat(base.GetServices(serviceType));
        }

        #region IDependencyResolver Members

        public override void Register(Type serviceType, IEnumerable<Func<object>> activators)
        {
            foreach (var activatorFunc in activators)
            {
                var activator = activatorFunc();
                _container.Configure(c => c.For(serviceType).Use(activator));
            }
        }

        public override void Register(Type serviceType, Func<object> activator)
        {
            if (_container == null)
            {
                base.Register(serviceType, activator);
            }
            else
            {
                _container.Configure(c => c.For(serviceType).Use(activator()));
            }
        }
        #endregion
    }
}
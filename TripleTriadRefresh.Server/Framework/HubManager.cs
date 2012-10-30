using System.Collections.Generic;
using System.Linq;
using SignalR;
using SignalR.Hubs;

namespace TripleTriadRefresh.Server.Framework
{
    public class HubManager : IHubManager
    {
        private readonly IHubManager defaultManager;
        public HubManager(IDependencyResolver resolver)
        {
            defaultManager = new DefaultHubManager(resolver);
        }

        #region IHubManager Members

        public MethodDescriptor GetHubMethod(string hubName, string method, params IJsonValue[] parameters)
        {
            return defaultManager.GetHubMethod(hubName, method, parameters);
        }

        public IEnumerable<MethodDescriptor> GetHubMethods(string hubName, System.Func<MethodDescriptor, bool> predicate = null)
        {
            return defaultManager.GetHubMethods(hubName, predicate);
        }

        public HubDescriptor GetHub(string hubName)
        {
            var hubDesc = defaultManager.GetHub(hubName);
            hubDesc.Type = defaultManager.ResolveHub(hubName).GetType();
            return hubDesc;
        }

        public IEnumerable<HubDescriptor> GetHubs(System.Func<HubDescriptor, bool> predicate = null)
        {
            var hubsDesc = defaultManager.GetHubs(predicate).ToList();
            hubsDesc.ForEach(x => x.Type = defaultManager.ResolveHub(x.Name).GetType());
            return hubsDesc;
        }

        public IHub ResolveHub(string hubName)
        {
            var hub = defaultManager.ResolveHub(hubName);
            return hub;
        }

        IEnumerable<IHub> IHubManager.ResolveHubs()
        {
            var hubs = defaultManager.ResolveHubs();
            return hubs;
        }
        #endregion
    }
}
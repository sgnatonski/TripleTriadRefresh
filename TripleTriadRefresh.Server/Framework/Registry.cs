using AspectMap;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using TripleTriadRefresh.Server.Framework.Aspects.Attributes;
using TripleTriadRefresh.Server.Framework.Aspects.Interceptors;
using TripleTriadRefresh.Server.Hubs.Handlers;

namespace TripleTriadRefresh.Server.Framework
{
    public class Registry : AspectsRegistry
    {
        public Registry()
        {
            ForAspect<HandleErrorAttribute>().WithPriority(0).HandleWith<HandleErrorInterceptor>();
            ForAspect<RequireLoginAttribute>().WithPriority(1).HandleWith<RequireLoginInterceptor>();
            ForAspect<GameInjectAttribute>().WithPriority(2).HandleWith<GameInjectInterceptor>();

            For<IDependencyResolver>().Use<StructureMapDependencyResolver>();
            For<IJsonSerializer>().Use<CamelCaseJsonSerializer>();
            For<IConnectionIdPrefixGenerator>().Use<GameConnectionIdPrefixGenerator>();
            For<IGameContainer>().Singleton().Use<GameContainer>();
            For<IPlayerActivator>().Singleton().Use<PlayerActivator>();
            For<IGameHubHandler>().EnrichAllWith(AddAspectsTo).Use<GameHubHandler>();
        }
    }
}
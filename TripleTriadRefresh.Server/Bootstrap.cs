using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Server;
using TripleTriadRefresh.Server.Framework;
using TripleTriadRefresh.Server.Models;
using TripleTriadRefresh.Server.Models.Ai;
using TripleTriadRefresh.Server.Models.System;
using Microsoft.AspNet.SignalR;

namespace TripleTriadRefresh.Server
{
    public class Bootstrap
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(c => c.AddRegistry(new Registry()));

            GlobalHost.DependencyResolver = new StructureMapDependencyResolver(ObjectFactory.Container);
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

            CardImage.Initialize();
            DbRepository.Initialize();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteTable.Routes.MapHubs("~/app");
        }
    }
}
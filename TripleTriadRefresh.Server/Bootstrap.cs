using System.Web.Mvc;
using System.Web.Routing;
using SignalR;
using StructureMap;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Server;
using TripleTriadRefresh.Server.Framework;
using TripleTriadRefresh.Server.Models;
using TripleTriadRefresh.Server.Models.Ai;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server
{
    public class Bootstrap
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(c => c.AddRegistry(new Registry()));

            GlobalHost.DependencyResolver = new StructureMapDependencyResolver(ObjectFactory.Container);
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

            RouteTable.Routes.MapHubs("~/app");

            CardImage.Initialize();
            DbRepository.Initialize();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            /*var ai = new AiPlayer(2, 4) { IsReady = true, ConnectionId = "ai" };
            ai.CreatePlayHand();
            var player = new Player(1) { IsReady = true, ConnectionId = "player" };
            player.CreatePlayHand();

            var game = new Game(player, Data.Models.Rules.Open, Data.Models.TradeRules.One, (g) => { });
            game.SecondPlayer = ai;

            game.PlaceCard("player", player.Hand.PlayCards[0].Id, 1);

            ai.GetCommand(game);*/
        }
    }
}
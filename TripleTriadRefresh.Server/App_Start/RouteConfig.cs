using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TripleTriadRefresh.Server
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Deck",
                url: "deck/{id}",
                defaults: new { controller = "Game", action = "GetDeck", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Games",
                url: "games",
                defaults: new { controller = "Game", action = "GetGameList" }
            );

            routes.MapRoute(
                name: "Standing",
                url: "standing/{id}",
                defaults: new { controller = "Game", action = "GetStanding", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AuthLogin",
                url: "login",
                defaults: new { controller = "Authentication", action = "Login" }
            );
            routes.MapRoute(
                name: "AuthLoginDebug",
                url: "logindebug",
                defaults: new { controller = "Authentication", action = "LoginDebug" }
            );

            routes.MapRoute(
                name: "AuthLogout",
                url: "logout",
                defaults: new { controller = "Authentication", action = "Logout" }
            );

            routes.MapRoute(
                name: "Main",
                url: "{id}",
                defaults: new { controller = "Game", action = "Game", id = UrlParameter.Optional }
            );
        }
    }
}
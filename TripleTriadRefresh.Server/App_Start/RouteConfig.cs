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
                url: "api/deck/{id}",
                defaults: new { controller = "Game", action = "GetDeck", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Games",
                url: "api/games",
                defaults: new { controller = "Game", action = "GetGameList" }
            );

            routes.MapRoute(
                name: "Standing",
                url: "api/standing/{id}",
                defaults: new { controller = "Game", action = "GetStanding", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AuthLogin",
                url: "api/login",
                defaults: new { controller = "Authentication", action = "Login" }
            );
            routes.MapRoute(
                name: "AuthLoginDebug",
                url: "api/logindebug",
                defaults: new { controller = "Authentication", action = "LoginDebug" }
            );

            routes.MapRoute(
                name: "AuthLogout",
                url: "api/logout",
                defaults: new { controller = "Authentication", action = "Logout" }
            );

            // not starting with api
            routes.MapRoute(
                name: "Main",
                url: "{*url}",
                defaults: new { controller = "Game", action = "Game", id = UrlParameter.Optional },
                constraints: new { url = @"^(?!api)\w+$" }
            );

            routes.MapRoute(
                name: "Play",
                url: "play/{*url}",
                defaults: new { controller = "Game", action = "Game", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
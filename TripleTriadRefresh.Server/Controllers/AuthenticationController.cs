using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using TripleTriadRefresh.Server.Controllers.Result;
using TripleTriadRefresh.Server.Framework;
using TripleTriadRefresh.Server.Models;

namespace TripleTriadRefresh.Server.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IPlayerActivator playerActivator;

        public AuthenticationController(IPlayerActivator playerActivator)
        {
            this.playerActivator = playerActivator;
        }

        [HttpPost]
        public JsonNetResult Login(FormCollection form)
        {
            var accessToken = form["accessToken"];

            var fb = new Facebook.FacebookClient(accessToken);
            dynamic user = fb.Get("me");

            this.ActivateUser(user.id);
            this.CreateAuthenticationCookie(accessToken, user);

            return new LoginResult(user.id);
        }

        [HttpPost]
        public JsonNetResult LoginDebug(string id)
        {
            if (Debugger.IsAttached)
            {
                this.ActivateUser(id);
                this.CreateAuthenticationCookie(string.Empty, new { name = id, id = id });

                return new LoginResult(id);
            }
            else { throw new UnauthorizedAccessException(); }
        }

        [HttpPost]
        public void Logout()
        {
            new Player().Logout();
            FormsAuthentication.SignOut();
            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Response.Redirect(Request.UrlReferrer.ToString(), false);
        }

        private void ActivateUser(string id)
        {
            IPlayer player = Debugger.IsAttached ? new Player(id) : new Player();

            if (player.DbEntity == null)
            {
                player = playerActivator.Activate(id);
            }

            player.Login();
        }

        private void CreateAuthenticationCookie(string accessToken, dynamic user)
        {
            var isPersistent = false;
            var ticket = new FormsAuthenticationTicket(
                1,
                JObject.FromObject(new { Id = user.id, Name = user.name }).ToString(),
                DateTime.Now,
                DateTime.Now.AddDays(10),
                isPersistent,
                accessToken,
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket using the machine key
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                Expires = DateTime.Now.AddDays(10),
                HttpOnly = true
            };

            Response.AppendCookie(cookie);
        }
    }
}
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using TripleTriadRefresh.Server;

namespace TripleTriadRefresh.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrap.Configure();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    dynamic user = JObject.Parse(authTicket.Name);

                    var id = new GenericIdentity((string)user["Name"]);
                    var principal = new GenericPrincipal(id, new string[] { });

                    Context.User = principal;
                    Context.Items.Add("UserId", user["Id"].Value);
                }
                catch (HttpException ex)
                {
                    Context.User = null;
                    Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                }
            }
            else
            {
                Context.User = null;
            }
        }
    }
}
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting.AspNet;

[assembly: PreApplicationStartMethod(typeof(TripleTriadRefresh.Server.RegisterHubs), "Start")]

namespace TripleTriadRefresh.Server
{
    public static class RegisterHubs
    {
        public static void Start()
        {
            
        }
    }
}

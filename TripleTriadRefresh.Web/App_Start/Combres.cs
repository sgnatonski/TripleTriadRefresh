[assembly: WebActivator.PreApplicationStartMethod(typeof(TripleTriadRefresh.Web.App_Start.Combres), "PreStart")]
namespace TripleTriadRefresh.Web.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}
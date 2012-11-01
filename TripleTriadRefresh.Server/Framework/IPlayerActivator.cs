using TripleTriadRefresh.Server.Models;

namespace TripleTriadRefresh.Server.Framework
{
    public interface IPlayerActivator
    {
        IPlayer Activate(string id);
    }
}
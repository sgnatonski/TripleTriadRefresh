using System;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Data.Domain;

namespace TripleTriadRefresh.Server.Framework
{
    public interface IPlayerActivator
    {
        void Activate(string id);
    }
}
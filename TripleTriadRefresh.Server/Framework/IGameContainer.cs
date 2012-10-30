using System.Collections.Generic;
using SignalR.Hubs;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Framework
{
    public interface IGameContainer
    {
        void AddGame(Game game);

        Game GetGame(string gameId);

        Game GetGame(HubCallerContext context);

        IEnumerable<object> GetGameList();

        void RemoveGame(Game game);
    }
}
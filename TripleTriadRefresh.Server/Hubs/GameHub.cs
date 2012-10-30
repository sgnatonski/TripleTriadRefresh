using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR.Hubs;
using StructureMap;
using TripleTriadRefresh.Server.Hubs.Handlers;

namespace TripleTriadRefresh.Server.Hubs
{
    public class GameHub : Hub, IDisconnect, IConnected
    {
        public IGameHubHandler GetHandler()
        {
            return ObjectFactory.With(this).GetInstance<IGameHubHandler>();
        }

        public void CreateGame()
        {
            this.GetHandler().CreateGame();
        }

        public void CreateGameWithAi()
        {
            this.GetHandler().CreateGameWithAi();
        }

        public void JoinGame(string gameId)
        {
            this.GetHandler().JoinGame(gameId);
        }

        public void LeaveGame()
        {
            this.GetHandler().LeaveGame();
        }

        public void DeclareReady()
        {
            this.GetHandler().DeclareReady();
        }

        public void PlaceCard(string id, int position)
        {
            this.GetHandler().PlaceCard(id, position);
        }

        public void GetOwnedCards()
        {
            this.GetHandler().GetOwnedCards();
        }

        public Task Disconnect()
        {
            this.GetHandler().Disconnect();
            return null;
        }

        public Task Connect()
        {
            this.GetHandler().Connect();
            return null;
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return null;
        }
    }
}
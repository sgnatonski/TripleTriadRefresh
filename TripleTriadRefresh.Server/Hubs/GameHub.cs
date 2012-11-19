using System.Collections.Generic;
using System.Threading.Tasks;
using StructureMap;
using TripleTriadRefresh.Server.Hubs.Handlers;
using Microsoft.AspNet.SignalR.Hubs;

namespace TripleTriadRefresh.Server.Hubs
{
    public class GameHub : Hub
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

        public void ResolveGame(string gameId)
        {
            this.GetHandler().ResolveGame(gameId);
        }

        public override Task OnDisconnected()
        {
            this.GetHandler().Disconnect();
            return null;
        }

        public override Task OnConnected()
        {
            this.GetHandler().Connect();
            return null;
        }

        public override Task OnReconnected()
        {
            return null;
        }
    }
}
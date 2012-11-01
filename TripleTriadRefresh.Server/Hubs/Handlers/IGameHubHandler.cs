using TripleTriadRefresh.Server.Models.System;
namespace TripleTriadRefresh.Server.Hubs.Handlers
{
    public interface IGameHubHandler
    {
        void CreateGame();

        void CreateGameWithAi();

        void JoinGame(string gameId, Game game = null);

        void LeaveGame(Game game = null);

        void DeclareReady(Game game = null);

        void PlaceCard(string id, int position, Game game = null);

        void GetOwnedCards(Game game = null);

        void ResolveGame(string gameId, Game game = null);

        void Disconnect();

        void Connect();
    }
}
namespace TripleTriadRefresh.Server.Hubs.Handlers
{
    public interface IGameHubHandler
    {
        void CreateGame();

        void CreateGameWithAi();

        void JoinGame(string gameId);

        void LeaveGame();

        void DeclareReady();

        void PlaceCard(string id, int position);

        void GetOwnedCards();

        void Disconnect();

        void Connect();
    }
}
/// <reference path="../signalR-1.0.d.ts" />

interface GameHubServer {
    createGame();
    createGameWithAi();
    joinGame(gameId: string);
    leaveGame();
    declareReady();
    placeCard(id: string, position: number);
}

interface GameHubClient {
    receiveError(data: string);
    receiveGames(data: any[]);
    updateBoard(data: any);
    gameJoined(data: any);
    gameLeft();
    receiveResult(data: any);
}

interface GameHub extends HubConnection {
    server: GameHubServer;
    client: GameHubClient;
}

// extend SignalR interface
interface SignalR {
    gameHub: GameHub;
}
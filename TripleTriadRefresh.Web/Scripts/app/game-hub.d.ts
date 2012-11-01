/// <reference path="../jquery.signalR-0.5.3.d.ts" />

interface GameHub extends HubConnection {
    // server
    createGame();
    createGameWithAi();
    joinGame(gameId: string);
    leaveGame();
    declareReady();
    placeCard(id: string, position: number);
    // client
    receiveError(data: string);
    receiveGames(data: any[]);
    updateBoard(data: any);
    gameJoined(data: any);
    gameLeft();
    receiveResult(data: any);
}

// extend SignalR interface
interface SignalR {
    gameHub: GameHub;
}
var ViewFactory = (function () {
    function ViewFactory() { }
    ViewFactory.prototype.createGamesView = function () {
        return new GamesView();
    };
    ViewFactory.prototype.createGameView = function (gameId, withAi) {
        return new GameView(gameId, withAi);
    };
    ViewFactory.prototype.createDeckView = function () {
        return new DeckView();
    };
    return ViewFactory;
})();

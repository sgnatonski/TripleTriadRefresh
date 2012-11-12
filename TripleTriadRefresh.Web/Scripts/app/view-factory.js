var ViewFactory = (function () {
    function ViewFactory() { }
    ViewFactory.prototype.createViewFromUrl = function (path) {
        var arg = $.url(path);
        if(arg.segment('1') == 'play') {
            if(arg.segment('2')) {
                return this.createGameView(arg.segment('2'));
            } else {
                return this.createGamesView();
            }
        } else {
            if(arg.segment('1') == 'cards') {
                return this.createDeckView();
            } else {
                if(arg.segment('1') == 'me') {
                    return this.createStandingView();
                } else {
                    return this.createHomeView();
                }
            }
        }
    };
    ViewFactory.prototype.createGamesView = function () {
        return new GamesView();
    };
    ViewFactory.prototype.createGameView = function (gameId, withAi) {
        return new GameView(gameId, withAi);
    };
    ViewFactory.prototype.createDeckView = function () {
        return new DeckView();
    };
    ViewFactory.prototype.createStandingView = function () {
        return new StandingView();
    };
    ViewFactory.prototype.createHomeView = function () {
        return new HomeView();
    };
    return ViewFactory;
})();

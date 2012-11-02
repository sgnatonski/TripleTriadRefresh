/// <reference path="../ui/view/games-view.ts" />
/// <reference path="../ui/view/game-view.ts" />
/// <reference path="../ui/view/deck-view.ts" />
/// <reference path="../ui/view/standing-view.ts" />

class ViewFactory {
    createGamesView() {
        return new GamesView();
    }

    createGameView(gameId?: string, withAi?: bool) {
        return new GameView(gameId, withAi);
    }

    createDeckView() {
        return new DeckView();
    }

    createStandingView() {
        return new StandingView();
    }
}

declare var viewFac: ViewFactory;
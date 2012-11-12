/// <reference path="../purl.d.ts" />
/// <reference path="../ui/view/games-view.ts" />
/// <reference path="../ui/view/game-view.ts" />
/// <reference path="../ui/view/deck-view.ts" />
/// <reference path="../ui/view/standing-view.ts" />
/// <reference path="../ui/view/home-view.ts" />

class ViewFactory {
    createViewFromUrl(path: string) : View {
        var arg = $.url(path);

        if (arg.segment('1') == 'play') {
            if (arg.segment('2'))
                return this.createGameView(arg.segment('2'));
            else
                return this.createGamesView();
        }
        else if (arg.segment('1') == 'cards')
            return this.createDeckView();
        else if (arg.segment('1') == 'me')
            return this.createStandingView();   
        else
            return this.createHomeView();
    }

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

    createHomeView() {
        return new HomeView();
    }
}

declare var viewFac: ViewFactory;
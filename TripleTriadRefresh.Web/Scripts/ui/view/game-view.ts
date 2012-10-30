/// <reference path="../../knockout-2.2.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../dto/card.ts" />
/// <reference path="../../dto/game.ts" />

class GameView extends View {
    viewName = "game-view";
    gameId = <string>null;
    game = ko.observable(new Game(null));
    dragging = ko.observable(<Card>null);
    isReady = ko.observable(false);
    isStarted = ko.observable(false);

    constructor (gameId? :string, withAi? :bool) {
        super();

        var leaveBtn = new Button('Leave game');
        leaveBtn.action = () => this.connection.leaveGame();

        var readyBtn = new Button('Set ready');
        readyBtn.action = () => this.connection.declareReady();
        readyBtn.hidden = ko.computed(() => this.game().getPlayer().isReady());

        this.menu().items([leaveBtn, readyBtn]);

        this.gameId = gameId;

        this.connection.updateBoard = (data: any) => {
            this.game(new Game(data));
        };
        this.connection.gameJoined = (data) => {
            window.history.pushState(data, "Game", app.getPathAbs() + data);
            app.currentGameId(data);
            this.isLoading(false);
        };
        this.connection.gameLeft = () => {
            window.history.pushState(null, "Game list", app.getPathAbs());
            app.currentGameId('');
            app.view(app.viewFac.createGamesView());
        };

        this.startConnection(() => {
            this.isLoading(true);
            if (!this.gameId) {
                if (withAi) {
                    this.connection.createGameWithAi();
                }
                else {
                    this.connection.createGame();
                }
            }
            else {
                this.connection.joinGame(this.gameId);
            }
        });

        this.placeCard = (index: number) => {
            this.connection.placeCard(this.dragging().id(), index);
            this.dragging(null);
        }
    }

    placeCard: (index: number) => void;
}
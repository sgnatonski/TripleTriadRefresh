/// <reference path="../../knockout-2.2.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../dto/card.ts" />
/// <reference path="../../dto/game.ts" />
/// <reference path="../../dto/game-result.ts" />

declare var app;
class GameView extends View {
    viewName = ko.observable('game-view');
    gameId = <string>null;
    game = ko.observable(new Game(null));
    gameResult = ko.observable(new GameResult(null));
    dragging = ko.observable(<Card>null);
    isReady = ko.observable(false);
    isStarted = ko.observable(false);
    expDone = ko.observable(false);
    cardPtsDone = ko.observable(false);
    closeResultHidden = ko.observable(true);

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
            window.history.pushState(data, "Game", app.getPathAbs() + 'play/' + data);
            app.currentGameId(data);
            this.isLoading(false);
        };
        this.connection.gameLeft = () => {
            window.history.pushState(null, "Game list", app.getPathAbs() + 'play/');
            app.currentGameId('');
            app.view(app.viewFac.createGamesView());
        };
        this.connection.receiveResult = (data) => {
            this.gameResult(new GameResult(data));
            this.closeResultHidden(this.gameResult().expGain() && this.gameResult().cardPtsGain());
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
            var row = parseInt(((index - 1) / 3).toString(), 10);
            var col = (index - 1) % 3;
            this.dragging().position(index);
            this.dragging().confirmed(false);
            this.game().board()[row]()[col] = this.dragging();
            this.game().board()[row].valueHasMutated();

            this.connection.placeCard(this.dragging().id(), index);
            this.dragging(null);
        }

        this.resolveGame = () => {
            //this.connection.resolveGame(this.gameId);
        }

        this.closeResult = () => {
            this.closeResultHidden(true);
            window.history.pushState(null, "Game list", app.getPathAbs() + 'play/');
            app.currentGameId('');
            app.view(app.viewFac.createGamesView());
        }

        this.cardPtsDone.subscribe((val) => {
            if (val) {
                this.closeResultHidden(false);
            }
        });
    }

    placeCard: (index: number) => void;
    resolveGame: () => void;
    closeResult: () => void;
}
/// <reference path="../../knockout-2.2.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../app/view-factory.ts" />
/// <reference path="../../dto/game-list-item.ts" />

class GamesView extends View {
    viewName = "game-list-view";
    games = ko.observableArray(<GameListItem[]>[]);
    selected = ko.observable(<GameListItem>null);
    service = new GameService();
    constructor () {
        super();

        var createBtn = new Button('Create game');
        createBtn.action = () => app.view(app.viewFac.createGameView());
        createBtn.hidden = ko.computed(() => app.currentGameId() == null);

        var createAiBtn = new Button('Create AI game');
        createAiBtn.action = () => app.view(app.viewFac.createGameView(null, true));
        createAiBtn.hidden = ko.computed(() => app.currentGameId() == null);

        var continueBtn = new Button('Continue game');
        continueBtn.action = () => app.view(app.viewFac.createGameView(app.currentGameId()));
        continueBtn.hidden = ko.computed(() => app.currentGameId() != null);

        this.menu().items([createBtn, createAiBtn, continueBtn]);

        this.isLoading(true);
        this.service.getGames((data: any[]) => {
            this.selected(null);
            this.games(data);
            //this.games(ko.mapping.fromJS(data));
            this.isLoading(false);
        });

        this.connection.receiveGames = (data: any[]) => {
            this.selected(null);
            this.games(data);
        };

        this.selectGame = (game: GameListItem) => {
            this.selected(game);
        }

        this.isSelected = (game: GameListItem) => {
            this.selected().id() == game.id();
        }

        this.joinGame = (game: GameListItem) => {
            app.view(app.viewFac.createGameView(game.id()));
        }
    }

    selectGame: (game: GameListItem) => void;
    isSelected: (game: GameListItem) => void;
    joinGame: (game: GameListItem) => void;
}
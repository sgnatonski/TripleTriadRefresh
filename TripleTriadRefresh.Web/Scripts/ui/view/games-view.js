var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
}
var GamesView = (function (_super) {
    __extends(GamesView, _super);
    function GamesView() {
        var _this = this;
        _super.call(this);
        this.viewName = ko.observable('game-list-view');
        this.games = ko.observableArray([]);
        this.selected = ko.observable(null);
        this.service = new GameService();
        var createBtn = new Button('Create game');
        createBtn.action = function () {
            return app.view(app.viewFac.createGameView());
        };
        createBtn.hidden = ko.computed(function () {
            return app.currentGameId() == null;
        });
        var createAiBtn = new Button('Create AI game');
        createAiBtn.action = function () {
            return app.view(app.viewFac.createGameView(null, true));
        };
        createAiBtn.hidden = ko.computed(function () {
            return app.currentGameId() == null;
        });
        var continueBtn = new Button('Continue game');
        continueBtn.action = function () {
            return app.view(app.viewFac.createGameView(app.currentGameId()));
        };
        continueBtn.hidden = ko.computed(function () {
            return app.currentGameId() != null;
        });
        this.menu().items([
            createBtn, 
            createAiBtn, 
            continueBtn
        ]);
        this.isLoading(true);
        this.service.getGames(function (data) {
            _this.selected(null);
            _this.games(data);
            _this.isLoading(false);
        });
        this.connection.client.receiveGames = function (data) {
            _this.selected(null);
            _this.games(data);
        };
        this.selectGame = function (game) {
            _this.selected(game);
        };
        this.isSelected = function (game) {
            _this.selected().id() == game.id();
        };
        this.joinGame = function (game) {
            app.view(app.viewFac.createGameView(game.id()));
        };
    }
    return GamesView;
})(View);

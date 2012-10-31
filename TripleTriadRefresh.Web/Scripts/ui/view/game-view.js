var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
}
var GameView = (function (_super) {
    __extends(GameView, _super);
    function GameView(gameId, withAi) {
        var _this = this;
        _super.call(this);
        this.viewName = "game-view";
        this.gameId = null;
        this.game = ko.observable(new Game(null));
        this.dragging = ko.observable(null);
        this.isReady = ko.observable(false);
        this.isStarted = ko.observable(false);
        this.gameResult = ko.observable('');
        var leaveBtn = new Button('Leave game');
        leaveBtn.action = function () {
            return _this.connection.leaveGame();
        };
        var readyBtn = new Button('Set ready');
        readyBtn.action = function () {
            return _this.connection.declareReady();
        };
        readyBtn.hidden = ko.computed(function () {
            return _this.game().getPlayer().isReady();
        });
        this.menu().items([
            leaveBtn, 
            readyBtn
        ]);
        this.gameId = gameId;
        this.connection.updateBoard = function (data) {
            _this.game(new Game(data));
            if(_this.game().winner()) {
                var result = _this.game().firstPlayerScore() === _this.game().secondPlayerScore() ? 'Draw' : undefined;
                if(!result) {
                    result = _this.game().firstPlayerScore() > _this.game().secondPlayerScore() ? 'Won' : 'Loose';
                }
                _this.gameResult(result);
            }
        };
        this.connection.gameJoined = function (data) {
            window.history.pushState(data, "Game", app.getPathAbs() + data);
            app.currentGameId(data);
            _this.isLoading(false);
        };
        this.connection.gameLeft = function () {
            window.history.pushState(null, "Game list", app.getPathAbs());
            app.currentGameId('');
            app.view(app.viewFac.createGamesView());
        };
        this.connection.gameResolve = function (data) {
        };
        this.startConnection(function () {
            _this.isLoading(true);
            if(!_this.gameId) {
                if(withAi) {
                    _this.connection.createGameWithAi();
                } else {
                    _this.connection.createGame();
                }
            } else {
                _this.connection.joinGame(_this.gameId);
            }
        });
        this.placeCard = function (index) {
            _this.connection.placeCard(_this.dragging().id(), index);
            _this.dragging(null);
        };
        this.resolveGame = function () {
            _this.connection.resolveGame(_this.gameId);
        };
    }
    return GameView;
})(View);

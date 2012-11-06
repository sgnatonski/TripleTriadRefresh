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
        this.viewName = ko.observable('game-view');
        this.gameId = null;
        this.game = ko.observable(new Game(null));
        this.gameResult = ko.observable(new GameResult(null));
        this.dragging = ko.observable(null);
        this.isReady = ko.observable(false);
        this.isStarted = ko.observable(false);
        this.expDone = ko.observable(false);
        this.cardPtsDone = ko.observable(false);
        this.closeResultHidden = ko.observable(true);
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
        this.connection.receiveResult = function (data) {
            _this.gameResult(new GameResult(data));
            _this.closeResultHidden(_this.gameResult().expGain() && _this.gameResult().cardPtsGain());
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
            var row = parseInt(((index - 1) / 3).toString(), 10);
            var col = (index - 1) % 3;
            _this.dragging().position(index);
            _this.dragging().confirmed(false);
            _this.game().board()[row]()[col] = _this.dragging();
            _this.game().board()[row].valueHasMutated();
            _this.connection.placeCard(_this.dragging().id(), index);
            _this.dragging(null);
        };
        this.resolveGame = function () {
        };
        this.closeResult = function () {
            _this.closeResultHidden(true);
            window.history.pushState(null, "Game list", app.getPathAbs());
            app.currentGameId('');
            app.view(app.viewFac.createGamesView());
        };
        this.cardPtsDone.subscribe(function (val) {
            if(val) {
                _this.closeResultHidden(false);
            }
        });
    }
    return GameView;
})(View);

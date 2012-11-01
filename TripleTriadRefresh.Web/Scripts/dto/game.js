var Game = (function () {
    function Game(data) {
        this.gameId = ko.observable(0);
        this.firstPlayer = ko.observable(new Player(null));
        this.secondPlayer = ko.observable(new Player(null));
        this.currentPlayer = ko.observable(null);
        this.winner = ko.observable(null);
        this.tradeRule = ko.observable(0);
        this.rules = ko.observable(0);
        this.cardChain = ko.observableArray([]);
        this.firstPlayerScore = ko.observable(0);
        this.secondPlayerScore = ko.observable(0);
        this.inProgress = ko.observable(false);
        this.isFull = ko.observable(false);
        this.canJoin = ko.observable(false);
        this.board = ko.observableArray([]);
        this.elementsOnBoard = ko.observableArray([]);
        if(data) {
            ko.mapping.fromJS(data, {
            }, this);
        }
        this.isStarted = ko.computed(function () {
            return this.firstPlayer().isReady() && this.secondPlayer().isReady() && this.inProgress();
        }, this);
        this.isFirstPlayerTurn = ko.computed(function () {
            return this.isStarted() && this.firstPlayer().connectionId() === this.currentPlayer().connectionId();
        }, this);
        this.isSecondPlayerTurn = ko.computed(function () {
            return this.isStarted() && this.secondPlayer().connectionId() === this.currentPlayer().connectionId();
        }, this);
    }
    Game.prototype.getPlayer = function () {
        return this.firstPlayer() && this.firstPlayer().connectionId() == app.getConnectionId() ? this.firstPlayer() : this.secondPlayer();
    };
    Game.prototype.getEnemy = function () {
        return this.firstPlayer() && this.firstPlayer().connectionId() != app.getConnectionId() ? this.firstPlayer() : this.secondPlayer();
    };
    Game.prototype.getPlayerScore = function () {
        return this.firstPlayer() && this.firstPlayer().connectionId() == app.getConnectionId() ? this.firstPlayerScore() : this.secondPlayerScore();
    };
    Game.prototype.getEnemyScore = function () {
        return this.firstPlayer() && this.firstPlayer().connectionId() != app.getConnectionId() ? this.firstPlayerScore() : this.secondPlayerScore();
    };
    return Game;
})();

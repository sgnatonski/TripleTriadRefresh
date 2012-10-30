/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />
/// <reference path="player.ts" />
class Game {
    gameId = ko.observable(0);
    firstPlayer = ko.observable(new Player(null));
    secondPlayer = ko.observable(new Player(null));
    currentPlayer = ko.observable(<Player>null);
    winner = ko.observable(<Player>null);
    tradeRule = ko.observable(0);
    rules = ko.observable(0);
    cardChain = ko.observableArray([]);
    firstPlayerScore = ko.observable(0);
    secondPlayerScore = ko.observable(0);
    inProgress = ko.observable(0);
    isFull = ko.observable(false);
    canJoin = ko.observable(false);
    board = ko.observableArray([]);
    elementsOnBoard = ko.observableArray([]);

    constructor (data: any) {
        if (data) {
            ko.mapping.fromJS(data, {}, this);
        }
    }

    getPlayer() {
        return this.firstPlayer() && this.firstPlayer().connectionId() == app.getConnectionId() ? this.firstPlayer() : this.secondPlayer();
    }

    getEnemy() {
        return this.firstPlayer() && this.firstPlayer().connectionId() != app.getConnectionId() ? this.firstPlayer() : this.secondPlayer();
    }

    getPlayerScore() {
        return this.firstPlayer() && this.firstPlayer().connectionId() == app.getConnectionId() ? this.firstPlayerScore() : this.secondPlayerScore();
    }

    getEnemyScore() {
        return this.firstPlayer() && this.firstPlayer().connectionId() != app.getConnectionId() ? this.firstPlayerScore() : this.secondPlayerScore();
    }
}
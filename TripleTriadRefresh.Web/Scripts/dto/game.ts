/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />
/// <reference path="player.ts" />

declare var app;

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
    inProgress = ko.observable(false);
    isFull = ko.observable(false);
    canJoin = ko.observable(false);
    board = ko.observableArray([]);
    elementsOnBoard = ko.observableArray([]);
    isStarted: () => bool;
    isFirstPlayerTurn: () => bool;
    isSecondPlayerTurn: () => bool;

    constructor (data: any) {
        if (data) {
            ko.mapping.fromJS(data, {}, this);
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
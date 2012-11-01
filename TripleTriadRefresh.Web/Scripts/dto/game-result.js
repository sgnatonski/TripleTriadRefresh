var GameResult = (function () {
    function GameResult(data) {
        this.gameId = ko.observable('');
        this.score = ko.observable(0);
        this.expGain = ko.observable(0);
        this.cardPtsGain = ko.observable(0);
        this.rules = ko.observable(0);
        this.tradeRules = ko.observable(0);
        this.result = ko.observable('Draw');
        ko.mapping.fromJS(data, {
        }, this);
    }
    return GameResult;
})();

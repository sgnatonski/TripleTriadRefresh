var Standing = (function () {
    function Standing(data) {
        this.won = ko.observable(0);
        this.lost = ko.observable(0);
        this.draw = ko.observable(0);
        this.experience = ko.observable(0);
        this.cardPoints = ko.observable(0);
        this.unlockedRules = ko.observable(0);
        this.unlockedTradeRules = ko.observable(0);
        ko.mapping.fromJS(data, {
        }, this);
    }
    return Standing;
})();

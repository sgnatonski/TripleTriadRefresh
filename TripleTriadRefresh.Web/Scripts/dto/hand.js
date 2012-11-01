var Hand = (function () {
    function Hand(data) {
        this.playCards = ko.observableArray([]);
        this.handStrength = ko.observable(0);
        this.isValid = ko.observable(false);
        if(data) {
            ko.mapping.fromJS(data, {
            }, this);
            var cards = [];
            ko.utils.arrayForEach(data.playCards, function (value) {
                cards.push(new Card(value));
            });
            this.playCards(cards);
        }
    }
    return Hand;
})();

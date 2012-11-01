var Card = (function () {
    function Card(data) {
        this.id = ko.observable(0);
        this.cardId = ko.observable(0);
        this.ownedOriginallyBy = ko.observable(0);
        this.ownedBy = ko.observable(0);
        this.position = ko.observable(0);
        this.strength = ko.observable(0);
        this.elemental = ko.observable(0);
        this.image = ko.observable('');
        this.confirmed = ko.observable(true);
        ko.mapping.fromJS(data, {
        }, this);
    }
    return Card;
})();

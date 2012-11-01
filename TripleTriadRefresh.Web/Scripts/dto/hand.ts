/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />
/// <reference path="card.ts" />
class Hand {
    playCards = ko.observableArray(<Card[]>[]);
    handStrength = ko.observable(0);
    isValid = ko.observable(false);

    constructor (data: any) {
        if (data) {
            ko.mapping.fromJS(data, {}, this);
            var cards = [];
            ko.utils.arrayForEach(data.playCards, (value) => {
                cards.push(new Card(value));
            });

            this.playCards(cards);
        }
    }
}
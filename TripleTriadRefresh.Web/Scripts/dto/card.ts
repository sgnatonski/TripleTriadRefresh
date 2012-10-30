/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />
class Card {
    id = ko.observable(0);
    cardId = ko.observable(0);
    ownedOriginallyBy = ko.observable(0);
    ownedBy = ko.observable(0);
    position = ko.observable(0);
    strength = ko.observable(0);
    elemental = ko.observable(0);
    image = ko.observable('');

    constructor (data: any) {
        ko.mapping.fromJS(data, {}, this);            
    }
}
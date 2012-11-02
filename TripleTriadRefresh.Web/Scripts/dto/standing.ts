/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />
class Standing {
    won = ko.observable(0);
    lost = ko.observable(0);
    draw = ko.observable(0);
    experience = ko.observable(0);
    cardPoints = ko.observable(0);
    unlockedRules = ko.observable(0);
    unlockedTradeRules = ko.observable(0);

    constructor (data: any) {
        ko.mapping.fromJS(data, {}, this);
    }
}
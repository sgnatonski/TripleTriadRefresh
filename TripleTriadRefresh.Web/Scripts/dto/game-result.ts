/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />

class GameResult {
    gameId = ko.observable('');
    score = ko.observable(0);
    expGain = ko.observable(0);
    cardPtsGain = ko.observable(0);
    rules = ko.observable(0);
    tradeRules = ko.observable(0);
    result = ko.observable('Draw');

    constructor (data: any) {
        ko.mapping.fromJS(data, {}, this);            
    }
}
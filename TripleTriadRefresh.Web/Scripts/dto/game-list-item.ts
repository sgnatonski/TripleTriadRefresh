/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />

class GameListItem {
    id = ko.observable('');
    firstName = ko.observable('');
    secondName = ko.observable('');
    winnerName = ko.observable('');
    firstHandStr = ko.observable(0);
    secondHandStr = ko.observable(0);
    canJoin = ko.observable(false);

    constructor (data: any) {
        ko.mapping.fromJS(data, {}, this);            
    }
}
/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../knockoutmapping-2.0.d.ts" />
/// <reference path="hand.ts" />
class Player {
    connectionId = ko.observable('');
    isReady = ko.observable(false);
    userName = ko.observable('');
    hand = ko.observable(new Hand(null));

    constructor (data: any) {
        if (data) {
            ko.mapping.fromJS(data, {}, this);
        }
    }
}
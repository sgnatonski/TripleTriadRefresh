/// <reference path="../knockout-2.2.d.ts" />
class Button {
    name: string;
    action = () => { }: void;
    hidden = ko.computed(() => false);
    constructor (name: string) {
        this.name = name;
    }
}
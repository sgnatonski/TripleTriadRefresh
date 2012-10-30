/// <reference path="../jquery.d.ts" />
/// <reference path="../jqueryui.d.ts" />
/// <reference path="../jquery.signalR-0.5.3.d.ts" />
/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../app/common.js" />
/// <reference path="../app/game-hub.d.ts" />
/// <reference path="./menu.ts" />
class View {
    viewName: string;
    connection: GameHub;
    isLoading = ko.observable(false);
    menu = ko.observable(Menu);
    constructor () {
        $.extendSignalR();
        $.connection.hub.clearProxy();

        this.menu(new Menu());
        
        this.connection = this.connection ? this.connection : $.connection.gameHub;
        this.connection.logging = true;
        this.connection.receiveError = (data) => {
            alert(data);
        };
    }

    startConnection(connectedCallback: () => any) {
        $.connection.hub.recreateProxy();
        if ($.connection.hub.state == 1) {
            //already started
            connectedCallback()
        }
        else {
            $.connection.hub.start().done(() => {
                sessionStorage.setItem("srconnectionid", $.connection.hub.id);
                connectedCallback()
            });
        }
    }

    receive(data) {
    }
}
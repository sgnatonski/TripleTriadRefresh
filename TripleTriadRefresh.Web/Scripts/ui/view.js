var View = (function () {
    function View() {
        this.viewName = ko.observable(null);
        this.isLoading = ko.observable(false);
        this.menu = ko.observable(Menu);
        $.extendSignalR();
        $.connection.hub.clearProxy();
        this.menu(new Menu());
        this.connection = this.connection ? this.connection : $.connection.gameHub;
        this.connection.logging = true;
        this.connection.receiveError = function (data) {
            alert(data);
        };
    }
    View.prototype.startConnection = function (connectedCallback) {
        $.connection.hub.recreateProxy();
        if($.connection.hub.state == 1) {
            connectedCallback();
        } else {
            $.connection.hub.start().done(function () {
                sessionStorage.setItem("srconnectionid", $.connection.hub.id);
                connectedCallback();
            });
        }
    };
    View.prototype.receive = function (data) {
    };
    return View;
})();

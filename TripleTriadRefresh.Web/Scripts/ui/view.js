var View = (function () {
    function View() {
        this.viewName = ko.observable(null);
        this.isLoading = ko.observable(false);
        this.menu = ko.observable(Menu);
        this.menu(new Menu());
        this.connection = this.connection ? this.connection : $.connection.gameHub;
        this.connection.logging = true;
        this.connection.client.receiveError = function (data) {
            alert(data);
        };
        this.connection.error = function (error) {
            alert(error);
            return $.connection;
        };
    }
    View.prototype.startConnection = function (connectedCallback) {
        if($.connection.connectionState == 1) {
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

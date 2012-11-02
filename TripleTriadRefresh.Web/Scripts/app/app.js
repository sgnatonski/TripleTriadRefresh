var App = (function () {
    function App() {
        this.menu = ko.observable(null);
        this.view = ko.observable(null);
        this.authInitializing = ko.observable(true);
        this.isLoggedIn = ko.observable(false);
        this.userPicture = ko.observable('');
        this.currentGameId = ko.observable('');
        this.pathAbs = $('#ApplicationRoot').attr('href');
        this.path = '';
        this.viewFac = new ViewFactory();
    }
    App.prototype.launch = function () {
        var pathPart = $.url(this.pathAbs).attr('path');
        if(pathPart.charAt(pathPart.length - 1) != '/') {
            pathPart += '/';
        }
        var path = $.url().attr('path').replace(pathPart, '');
        if(!path || path == '/') {
            this.view(this.viewFac.createGamesView());
        } else {
            if(path == 'cards') {
                this.view(this.viewFac.createDeckView());
            } else {
                if(path == 'standing') {
                    this.view(this.viewFac.createStandingView());
                } else {
                    this.view(this.viewFac.createGameView(path));
                }
            }
        }
    };
    App.prototype.getPath = function () {
        return this.path;
    };
    App.prototype.getPathAbs = function () {
        return this.pathAbs;
    };
    App.prototype.showStanding = function () {
        window.history.pushState(null, 'Standing', this.pathAbs + 'standing');
        this.view(this.viewFac.createStandingView());
    };
    App.prototype.showGames = function () {
        window.history.pushState(null, 'Game list', this.pathAbs);
        this.view(this.viewFac.createGamesView());
    };
    App.prototype.showCards = function () {
        window.history.pushState(null, 'Card list', this.pathAbs + 'cards');
        this.view(this.viewFac.createDeckView());
    };
    App.prototype.login = function (form) {
        var _this = this;
        var successfulLogin = function (data) {
            _this.userPicture(data);
            _this.isLoggedIn(true);
        };
        var userId = $.url().param('userId');
        if(userId) {
            $.ajax(this.path + 'logindebug', {
                type: 'POST',
                data: {
                    id: userId
                },
                success: successfulLogin
            });
        } else {
            $.ajax(this.path + 'login', {
                type: 'POST',
                data: $(form).serialize(),
                success: successfulLogin
            });
        }
    };
    App.prototype.getConnectionId = function () {
        return sessionStorage.getItem('srconnectionid');
    };
    return App;
})();

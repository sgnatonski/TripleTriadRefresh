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
        this.path = '/' + $.url().attr('path').replace(pathPart, '');
        this.view(this.viewFac.createViewFromUrl(this.path));
    };
    App.prototype.getPath = function () {
        return this.path;
    };
    App.prototype.getPathAbs = function () {
        return this.pathAbs;
    };
    App.prototype.showHome = function () {
        window.history.pushState(null, '', this.pathAbs);
        this.view(this.viewFac.createHomeView());
    };
    App.prototype.showStanding = function () {
        window.history.pushState(null, 'Me', this.pathAbs + 'me');
        this.view(this.viewFac.createStandingView());
    };
    App.prototype.showGames = function () {
        window.history.pushState(null, 'Game list', this.pathAbs + 'play');
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
            $.ajax(this.pathAbs + 'api/logindebug', {
                type: 'POST',
                data: {
                    id: userId
                },
                success: successfulLogin
            });
        } else {
            $.ajax(this.pathAbs + 'api/login', {
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

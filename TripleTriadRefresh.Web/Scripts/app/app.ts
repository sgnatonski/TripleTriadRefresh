/// <reference path="../jquery.d.ts" />
/// <reference path="../knockout-2.2.d.ts" />
/// <reference path="../purl.d.ts" />
/// <reference path="view-factory.ts" />

class App {
    menu = ko.observable(<Menu>null);
    view = ko.observable(<View>null);
    authInitializing = ko.observable(true);
    isLoggedIn = ko.observable(false);
    userPicture = ko.observable('');
    currentGameId = ko.observable('');
    pathAbs = $('#ApplicationRoot').attr('href');
    path = '';
    viewFac = new ViewFactory();
    launch() {
        var pathPart = $.url(this.pathAbs).attr('path');
        if (pathPart.charAt(pathPart.length - 1) != '/') {
            pathPart += '/';
        }
        //this.path = _s.rtrim(pathPart, '/') + '/';

        var path = $.url().attr('path').replace(pathPart, '');
        if (!path || path == '/')
            this.view(this.viewFac.createGamesView());
        else if (path == 'cards')
            this.view(this.viewFac.createDeckView());
        else if (path == 'me')
            this.view(this.viewFac.createStandingView());
        else
            this.view(this.viewFac.createGameView(path));
    }

    getPath() {
        return this.path;
    }

    getPathAbs() {
        return this.pathAbs;
    }

    showStanding() {
        window.history.pushState(null, 'Me', this.pathAbs + 'me');
        this.view(this.viewFac.createStandingView());
    }

    showGames() {
        window.history.pushState(null, 'Game list', this.pathAbs);
        this.view(this.viewFac.createGamesView());
    }

    showCards() {
        window.history.pushState(null, 'Card list', this.pathAbs + 'cards');
        this.view(this.viewFac.createDeckView());
    }

    login(form) {
        var successfulLogin = (data) => {
            this.userPicture(data);
            this.isLoggedIn(true);
        };

        var userId = $.url().param('userId');
        if (userId) {
            $.ajax(this.path + 'logindebug', {
                type: 'POST',
                data: { id: userId },
                success: successfulLogin
            });
        }
        else {
            $.ajax(this.path + 'login', {
                type: 'POST',
                data: $(form).serialize(),
                success: successfulLogin
            });
        }
    }

    getConnectionId() {
        return sessionStorage.getItem('srconnectionid');
    }
}

declare var app: App;
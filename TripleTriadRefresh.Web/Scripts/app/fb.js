/// <reference path="app.js" />
var app = new App();

window.fbAsyncInit = function () {
    FB.init({
        appId: '134305300045765', // App ID
        status: true, // check login status
        cookie: false, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });

    FB.Event.subscribe('auth.authResponseChange', function (response) {
        if (response.status === 'connected') {
            var accessToken = response.authResponse.accessToken;

            var form = document.createElement("form");

            var field = document.createElement("input");
            field.setAttribute("type", "hidden");
            field.setAttribute("name", 'accessToken');
            field.setAttribute("value", accessToken);
            form.appendChild(field);

            app.login(form);

        } else if (response.status === 'not_authorized') {
            // the user is logged in to Facebook, 
            // but has not authenticated your app
        } else {
            // the user isn't logged in to Facebook.
        }

        setTimeout(function () {
            $('.fb-login-button iframe.fb_ltr').load(function () {
                app.authInitializing(false);
                $('.fsm').hide();
            });
        }, 1);
    });

    FB.getLoginStatus(function (response) {
        if (response.status !== 'connected') {
            // the user isn't logged in to Facebook.
            $.post(app.getPathAbs() + 'api/logout', function () {
                app.authInitializing(false);
                $('.fsm').hide();
            });
        }
    });
};

(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    try {
        ref.parentNode.insertBefore(js, ref);
    }
    catch(e) {}

    setTimeout(function(){
        app.authInitializing(false);
    }, 2000);

    
}(document));

$(function () {
    app.launch();
    infuser.defaults.templateUrl = app.getPathAbs() + 'templates';
    ko.applyBindings(app, $("body > .content").get(0));
});
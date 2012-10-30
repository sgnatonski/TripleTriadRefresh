var Player = (function () {
    function Player(data) {
        this.connectionId = ko.observable('');
        this.isReady = ko.observable('');
        this.userName = ko.observable('');
        this.hand = ko.observable(new Hand(null));
        if(data) {
            ko.mapping.fromJS(data, {
            }, this);
        }
    }
    return Player;
})();

var GameListItem = (function () {
    function GameListItem(data) {
        this.id = ko.observable('');
        this.firstName = ko.observable('');
        this.secondName = ko.observable('');
        this.winnerName = ko.observable('');
        this.firstHandStr = ko.observable(0);
        this.secondHandStr = ko.observable(0);
        this.canJoin = ko.observable(false);
        ko.mapping.fromJS(data, {
        }, this);
    }
    return GameListItem;
})();

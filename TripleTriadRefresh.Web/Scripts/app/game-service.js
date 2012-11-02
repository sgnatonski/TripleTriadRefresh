var GameService = (function () {
    function GameService() { }
    GameService.prototype.getStanding = function (completeCallback) {
        $.get('standing', null, completeCallback);
    };
    GameService.prototype.getDeck = function (completeCallback) {
        $.get('deck', null, completeCallback);
    };
    GameService.prototype.getGames = function (completeCallback) {
        $.get('games', null, completeCallback);
    };
    return GameService;
})();

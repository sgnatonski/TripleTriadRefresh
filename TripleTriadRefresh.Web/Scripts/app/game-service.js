var GameService = (function () {
    function GameService() { }
    GameService.prototype.getStanding = function (completeCallback) {
        $.get('api/standing', null, completeCallback);
    };
    GameService.prototype.getDeck = function (completeCallback) {
        $.get('api/deck', null, completeCallback);
    };
    GameService.prototype.getGames = function (completeCallback) {
        $.get('api/games', null, completeCallback);
    };
    return GameService;
})();

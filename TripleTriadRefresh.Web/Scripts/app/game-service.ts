/// <reference path="../jquery.d.ts" />
class GameService {
    getStanding(completeCallback : (data: any[]) => void) {
        $.get('api/standing', null, completeCallback);
    }
    getDeck(completeCallback : (data: any[]) => void) {
        $.get('api/deck', null, completeCallback);
    }
    getGames(completeCallback : (data: any[]) => void) {
        $.get('api/games', null, completeCallback);
    }
}
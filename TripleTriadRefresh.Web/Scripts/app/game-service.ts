/// <reference path="../jquery.d.ts" />
class GameService {
    getStanding(completeCallback : (data: any[]) => void) {
        $.get('standing', null, completeCallback);
    }
    getDeck(completeCallback : (data: any[]) => void) {
        $.get('deck', null, completeCallback);
    }
    getGames(completeCallback : (data: any[]) => void) {
        $.get('games', null, completeCallback);
    }
}
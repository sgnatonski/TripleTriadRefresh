/// <reference path="../jquery.d.ts" />
class GameService {
    getDeck(completeCallback : (data: any[]) => void) {
        $.get('deck', null, completeCallback);
    }
    getGames(completeCallback : (data: any[]) => void) {
        $.get('games', null, completeCallback);
    }
}
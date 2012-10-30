/// <reference path="../../jquery.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../app/game-service.ts" />
class DeckView extends View { 
    viewName = 'deck-view';
    cards = ko.observableArray(<Card[]>[]);
    isHandSelection = ko.observable(false);
    service = new GameService();
    constructor() {
        super();

        var createBtn = new Button('Create game');
        createBtn.action = () => app.view(app.viewFac.createGameView());

        this.menu().items([createBtn]);
        
        this.isLoading(true);
        this.service.getDeck((data: any[]) => {
            this.cards(data);
            this.isLoading(false);
        });
    }
}
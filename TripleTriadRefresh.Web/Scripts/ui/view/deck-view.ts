/// <reference path="../../jquery.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../app/game-service.ts" />
/// <reference path="../../dto/card.ts" />

declare var app;
class DeckView extends View { 
    public viewName = ko.observable('deck-view');
    public cards = ko.observableArray(<Card[]>[]);
    public isHandSelection = ko.observable(false);
    public dragging = ko.observable(<Card>null);
    private service = new GameService();
    constructor() {
        super();

        var createBtn = new Button('Create game');
        createBtn.action = () => app.view(app.viewFac.createGameView());

        this.menu().items([createBtn]);
        
        this.isLoading(true);
        this.service.getDeck((data: any[]) => {
            var cards = [];
            ko.utils.arrayForEach(data, (value) => {
                cards.push(new Card(value));
            });

            this.cards([]);
            this.cards(cards);
            this.isLoading(false);
        });
    }
}
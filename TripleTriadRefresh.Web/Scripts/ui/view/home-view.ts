/// <reference path="../../jquery.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../app/game-service.ts" />

declare var app;
class HomeView extends View { 
    public viewName = ko.observable('home-view');
    private service = new GameService();
    constructor() {
        super();

        /*this.isLoading(true);
        this.service.getDeck((data: any[]) => {
            var cards = [];
            ko.utils.arrayForEach(data, (value) => {
                cards.push(new Card(value));
            });

            this.cards([]);
            this.cards(cards);
            this.isLoading(false);
        });*/
    }
}
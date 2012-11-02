/// <reference path="../../jquery.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../app/game-service.ts" />
/// <reference path="../../dto/standing.ts" />
class StandingView extends View { 
    viewName = 'standing-view';
    standing = ko.observable(new Standing(null));
    service = new GameService();
    constructor() {
        super();

        this.menu().items([]);
        
        this.isLoading(true);
        this.service.getStanding((data: any) => {
            this.standing(new Standing(data));
            this.isLoading(false);
        });
    }
}
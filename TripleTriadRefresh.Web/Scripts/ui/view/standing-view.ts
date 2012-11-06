/// <reference path="../../jquery.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../app/game-service.ts" />
/// <reference path="../../dto/standing.ts" />
class StandingView extends View { 
    public viewName = ko.observable('standing-view');
    public standing = ko.observable(new Standing(null));
    private service = new GameService();
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
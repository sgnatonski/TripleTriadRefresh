/// <reference path="../../jquery.d.ts" />
/// <reference path="../view.ts" />
/// <reference path="../../app/home-service.ts" />

declare var app;
class HomeView extends View { 
    public viewName = ko.observable('home-view');
    public news = ko.observableArray(<string[]>[]);
    private service = new HomeService();
    constructor() {
        super();

        this.isLoading(true);
        this.service.getNews((data: any[]) => {
            var news = [];
            ko.utils.arrayForEach(data, (value) => {
                news.push({ 
                    title: value['@title'],
                    version: value['@version'],
                    date: value['@date'],
                    type: value['@type'],
                    texts: value['texts']
                });
            });

            this.news([]);
            this.news(news);
            this.isLoading(false);
        });
    }
}
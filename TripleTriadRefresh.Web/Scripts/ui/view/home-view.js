var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
}
var HomeView = (function (_super) {
    __extends(HomeView, _super);
    function HomeView() {
        var _this = this;
        _super.call(this);
        this.viewName = ko.observable('home-view');
        this.news = ko.observableArray([]);
        this.service = new HomeService();
        this.isLoading(true);
        this.service.getNews(function (data) {
            var news = [];
            ko.utils.arrayForEach(data, function (value) {
                news.push({
                    title: value['@title'],
                    version: value['@version'],
                    date: value['@date'],
                    type: value['@type'],
                    texts: value['texts']
                });
            });
            _this.news([]);
            _this.news(news);
            _this.isLoading(false);
        });
    }
    return HomeView;
})(View);

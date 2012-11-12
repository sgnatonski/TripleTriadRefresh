var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
}
var HomeView = (function (_super) {
    __extends(HomeView, _super);
    function HomeView() {
        _super.call(this);
        this.viewName = ko.observable('home-view');
        this.service = new GameService();
    }
    return HomeView;
})(View);

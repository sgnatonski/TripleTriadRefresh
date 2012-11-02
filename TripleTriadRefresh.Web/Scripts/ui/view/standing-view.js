var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
}
var StandingView = (function (_super) {
    __extends(StandingView, _super);
    function StandingView() {
        var _this = this;
        _super.call(this);
        this.viewName = 'standing-view';
        this.standing = ko.observable(null);
        this.service = new GameService();
        this.menu().items([]);
        this.isLoading(true);
        this.service.getStanding(function (data) {
            _this.standing(new Standing(data));
            _this.isLoading(false);
        });
    }
    return StandingView;
})(View);

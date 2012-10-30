var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
}
var DeckView = (function (_super) {
    __extends(DeckView, _super);
    function DeckView() {
        var _this = this;
        _super.call(this);
        this.viewName = 'deck-view';
        this.cards = ko.observableArray([]);
        this.isHandSelection = ko.observable(false);
        this.service = new GameService();
        var createBtn = new Button('Create game');
        createBtn.action = function () {
            return app.view(app.viewFac.createGameView());
        };
        this.menu().items([
            createBtn
        ]);
        this.isLoading(true);
        this.service.getDeck(function (data) {
            _this.cards(data);
            _this.isLoading(false);
        });
    }
    return DeckView;
})(View);

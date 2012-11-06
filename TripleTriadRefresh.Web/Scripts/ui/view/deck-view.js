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
        this.viewName = ko.observable('deck-view');
        this.cards = ko.observableArray([]);
        this.isHandSelection = ko.observable(false);
        this.dragging = ko.observable(null);
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
            var cards = [];
            ko.utils.arrayForEach(data, function (value) {
                cards.push(new Card(value));
            });
            _this.cards([]);
            _this.cards(cards);
            _this.isLoading(false);
        });
    }
    return DeckView;
})(View);

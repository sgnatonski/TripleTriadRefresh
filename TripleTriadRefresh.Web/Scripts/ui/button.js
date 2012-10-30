var Button = (function () {
    function Button(name) {
        this.action = function () {
        };
        this.hidden = ko.computed(function () {
            return false;
        });
        this.name = name;
    }
    return Button;
})();

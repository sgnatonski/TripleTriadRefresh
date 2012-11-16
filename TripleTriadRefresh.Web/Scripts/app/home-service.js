var HomeService = (function () {
    function HomeService() { }
    HomeService.prototype.getNews = function (completeCallback) {
        $.get('api/news', null, completeCallback);
    };
    return HomeService;
})();

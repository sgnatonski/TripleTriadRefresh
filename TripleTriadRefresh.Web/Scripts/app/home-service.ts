/// <reference path="../jquery.d.ts" />
class HomeService {
    getNews(completeCallback : (data: any[]) => void) {
        $.get('api/news', null, completeCallback);
    }
}
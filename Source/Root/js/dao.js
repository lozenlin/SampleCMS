/// <reference path="jquery-3.2.1.js"/>

var langNo = "1";   // change this value at page.
var serviceUrl = "";    // change this value at page. e.g.,"jsonService.ashx?l=" + langNo;

// data access object of JsonService
// cr: ClientResult (a class of server side)
var dao = {
    //取得附縮圖的網頁內容清單
    Article_GetListWithThumb: function (artid, p, crCallBack) {
        $.post(serviceUrl, {
            serviceName: "Article_GetListWithThumb",
            artid: artid,
            p: p
        }, function (data) {
            var cr = $.parseJSON(data);
            crCallBack(cr);
        }).fail(function () {
            crCallBack({ b: false, err: "connect failed" });
        });
    },

    emptyTail: 0
};

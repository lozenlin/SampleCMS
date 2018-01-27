/// <reference path="jquery-3.2.1.js"/>

var langNo = "1";   // change this value at page.
var serviceUrl = "";    // change this value at page. e.g.,"jsonService.ashx?l=" + langNo;

// data access object of JsonService
// cr: ClientResult (a class of server side)
var dao = {
    //暫存身分的權限
    TempStoreRolePvg: function (
        roleName, opId, itemVal,
        selfVal, crewVal, othersVal,
        addVal, roleId, crCallBack
        ) {
        $.post(serviceUrl, {
            serviceName: "TempStoreRolePvg",
            roleName: roleName,
            opId: opId,
            itemVal: itemVal,
            selfVal: selfVal,
            crewVal: crewVal,
            othersVal: othersVal,
            addVal: addVal,
            roleId: roleId
        }, function (data) {
            var cr = $.parseJSON(data);
            crCallBack(cr);
        }).fail(function () {
            crCallBack({ b: false, err: "connect failed" });
        });
    },

    //更新網頁內容的指定區域是否在前台顯示
    UpdateArticleIsAreaShowInFrontStage: function (artId, areaName, isShow, token, crCallBack) {
        $.post(serviceUrl, {
            serviceName: "UpdateArticleIsAreaShowInFrontStage",
            artId: artId,
            areaName: areaName,
            isShow: isShow,
            token: token
        }, function (data) {
            var cr = $.parseJSON(data);
            crCallBack(cr);
        }).fail(function () {
            crCallBack({ b: false, err: "connect failed" });
        });
    },

    //更新網頁內容的前台子項目排序欄位
    UpdateArticleSortFieldOfFrontStage: function (artId, sortField, isSortDesc, token, crCallBack) {
        $.post(serviceUrl, {
            serviceName: "UpdateArticleSortFieldOfFrontStage",
            artId: artId,
            sortField: sortField,
            isSortDesc: isSortDesc,
            token: token
        }, function (data) {
            var cr = $.parseJSON(data);
            crCallBack(cr);
        }).fail(function () {
            crCallBack({ b: false, err: "connect failed" });
        });
    },

    emptyTail: 0
};

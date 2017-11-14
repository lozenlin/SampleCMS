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
        addVal, crCallBack
        ) {
        $.post(serviceUrl, {
            serviceName: "TempStoreRolePvg",
            roleName: roleName,
            opId: opId,
            itemVal: itemVal,
            selfVal: selfVal,
            crewVal: crewVal,
            othersVal: othersVal,
            addVal: addVal
        }, function (data) {
            var cr = $.parseJSON(data);
            crCallBack(cr);
        }).fail(function () {
            crCallBack({ b: false, err: "connect failed" });
        });
    },

    emptyTail: 0
};

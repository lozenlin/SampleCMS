/// <reference path="jquery-3.2.1.js"/>
var useIframeDialog = true;

// Global Settings
$(function () {
    $(".main-navbar").click(function () {
        smoothUp();
    });
});

function smoothUp(id) {
    if (id == undefined)
        id = "";

    $("html, body").animate({
        scrollTop: (id == "") ? 0 : $("#" + id).offset().top
    }, 500);
}

function applyDialogHeight(heightValue) {
    var borderTotalHeight = 81 - 10;
    var contentHeight = heightValue;

    if (contentHeight + borderTotalHeight > heightValue) {
        contentHeight = heightValue - borderTotalHeight;
        if (contentHeight < 0)
            contentHeight = 0;
    }

    $("#popupContainer")
        .css("height", contentHeight);
}

function popWinCore(link, w, h) {
    if (!useIframeDialog || isMobile) {
        var winObject = null;
        var fw = (screen.availWidth - w) / 2;
        var fh = (screen.availHeight - (h + 80)) / 2;
        winObject = window.open(link, '', 'scrollbars=yes,resizable=yes,width=' + w + ',height=' + h + ',top=' + fh + ',left=' + fw);
    } else {
        var mainHeight = $(window).height() - 65;
        if (mainHeight < 0)
            mainHeight = 0;

        var contentHeight = h;
        var borderTotalWidth = 31;
        var borderTotalHeight = 81;

        if (contentHeight + borderTotalHeight > mainHeight) {
            contentHeight = mainHeight - borderTotalHeight;
            if (contentHeight < 0)
                contentHeight = 0;
        }

        $("#popupContainer")
            .attr("src", link)
            .css("width", "100%")
            .css("height", contentHeight)
            .hide();

        $('#dialog>.msg').show();

        $("#dialog")
            .dialog("option", "title", link)
            .dialog("option", "width", w + borderTotalWidth)
            .dialog("option", "height", contentHeight + borderTotalHeight)
            .dialog("option", "position", { my: "center", at: "center", of: window })
            .dialog("open")
            .dialogExtend("maximize");
    }
}

function popWin(link, w, h) {
    useIframeDialog = true;
    popWinCore(link, w, h);
}

function popWinOut(link, w, h) {
    useIframeDialog = false;
    popWinCore(link, w, h);
}

// Helper function to get parameters from the query string.
// reference: https://docs.ckeditor.com/ckeditor4/docs/#!/guide/dev_file_browser_api
function getUrlParam(paramName) {
    var reParam = new RegExp('(?:[\?&]|&)' + paramName + '=([^&]+)', 'i');
    var match = window.location.search.match(reParam);

    return (match && match.length > 1) ? match[1] : null;
}

// SearchPanel
var $searchPanel = $(".search-panel");
var $btnCollapseSearchPanel= $("#btnCollapseSearchPanel");
var $btnExpandSearchPanel = $("#btnExpandSearchPanel");

$("#btnCollapseSearchPanel").click(function () {
    $searchPanel.slideUp("fast");
    $btnExpandSearchPanel.show();
    return false;
});

$("#btnExpandSearchPanel").click(function () {
    $searchPanel.slideDown("fast");
    $btnExpandSearchPanel.hide();
    return false;
});

// initial SearchPanel state
var isSearchPanelCollapsingAtBeginning = false;

$(function () {
    if (isSearchPanelCollapsingAtBeginning) {
        $searchPanel.hide();
        $btnExpandSearchPanel.show();
    }
});

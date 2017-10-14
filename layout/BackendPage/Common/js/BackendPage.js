/// <reference path="jquery-3.2.1.js"/>

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

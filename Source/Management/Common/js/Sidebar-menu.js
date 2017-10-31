/// <reference path="jquery-3.2.1.js"/>

var initOpId = "";
var initArticleId = "";

// folding all of items at the beginning
$(".sidebar-menu .items-group>ul").hide();
var $itemHeaders = $(".sidebar-menu .items-group>.item-header");

$itemHeaders.click(function () {
    $(this).siblings(".item-list, .tree-list").slideToggle("fast");
    return false;
});

//離開選單後回預設值
if (!isMobile) {
    $(".sidebar-container").mouseleave(function () {
        ToggleOperation(initOpId, initArticleId);
    });
}

// open all of operations
$("#btnExpand").click(function () {
    $(".sidebar-menu .items-group>ul").slideDown("fast");
    return false;
});

function ToggleItemsGroup($itemsGroup) {
    $itemsGroup.siblings().children(".item-list, .tree-list").slideUp("fast");
    $itemsGroup.children(".item-list, .tree-list").show();
}

function ToggleOperation(opId, articleId) {
    var $itemAreaOrItemsGroup = $("[opId='" + opId + "']");

    if ($itemAreaOrItemsGroup.length > 0) {
        if ($itemAreaOrItemsGroup.hasClass("items-group")) {
            ToggleItemsGroup($itemAreaOrItemsGroup);
            $itemAreaOrItemsGroup.children(".item-header").addClass("active");
        } else if ($itemAreaOrItemsGroup.hasClass("item-area")) {
            $itemHeader = $itemAreaOrItemsGroup.parent().siblings(".item-header");
            $itemHeader.addClass("active");
            ToggleItemsGroup($itemHeader.parent());
            $itemAreaOrItemsGroup.children(".item").addClass("active");
        }
    } else {
        $(".sidebar-menu .items-group>ul").slideUp("fast");
    }

}

function ToggleInitialOperation(opId, articleId) {
    initOpId = opId;
    initArticleId = articleId;

    ToggleOperation(initOpId, initArticleId);
}

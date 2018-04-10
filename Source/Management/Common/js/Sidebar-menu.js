/// <reference path="jquery-3.3.1.js"/>
/// <reference path="jquery.hoverIntent.js"/>

var articleMenu = {
    foldAllBranches: function (useAnimate) {
        if (useAnimate)
            $(".tree-list .branch-list").slideUp("fast");
        else
            $(".tree-list .branch-list").hide();
    },

    unfoldBranch: function ($branch, useAnimate) {
        if (useAnimate)
            $branch.siblings(".branch-list").slideDown("fast");
        else
            $branch.siblings(".branch-list").show();
    },

    unfoldBranchArea: function(articleId){
        if (articleId != null && articleId != "") {
            var $branchArea = $("[articleId='" + articleId + "']");

            if ($branchArea.length > 0) {
                $branchArea.children(".branch").addClass("active");
                articleMenu.unfoldBranch($branchArea.children(".branch"), false);

                var $curBranchArea = $branchArea;
                while ($curBranchArea.parent().parent().hasClass("branch-area")) {
                    $curBranchArea = $curBranchArea.parent().parent();
                    articleMenu.unfoldBranch($curBranchArea.children(".branch"), false);
                }

            } else {
            }
        }
    },

    tail: function () { }
};

var opMenu = {
    initOpId: "",
    initArticleId: "",

    foldAllOperations: function (useAnimate) {
        if (useAnimate)
            $(".sidebar-menu .items-group>ul").slideUp("fast");
        else
            $(".sidebar-menu .items-group>ul").hide();
    },

    unfoldAllOperations: function (useAnimate) {
        if (useAnimate)
            $(".sidebar-menu .items-group>ul").slideDown("fast");
        else
            $(".sidebar-menu .items-group>ul").show();
    },

    toggleItemsGroup: function ($itemsGroup) {
        $itemsGroup.siblings().children(".item-list, .tree-list").slideUp("fast");
        $itemsGroup.children(".item-list, .tree-list").show();
    },

    unfoldItemsGroup: function ($itemsGroup, useAnimate) {
        if (useAnimate)
            $itemsGroup.children(".item-list, .tree-list").slideDown("fast");
        else
            $itemsGroup.children(".item-list, .tree-list").show();
    },

    toggleOperation: function (opId, articleId) {
        var $itemAreaOrItemsGroup = $("[opId='" + opId + "']");

        if ($itemAreaOrItemsGroup.length > 0) {
            if ($itemAreaOrItemsGroup.hasClass("items-group")) {
                opMenu.toggleItemsGroup($itemAreaOrItemsGroup);
                $itemAreaOrItemsGroup.children(".item-header").addClass("active");
            } else if ($itemAreaOrItemsGroup.hasClass("item-area")) {
                $itemHeader = $itemAreaOrItemsGroup.parent().siblings(".item-header");
                $itemHeader.addClass("active");
                opMenu.toggleItemsGroup($itemHeader.parent());
                $itemAreaOrItemsGroup.children(".item").addClass("active");
            }
        } else {
            opMenu.foldAllOperations(true);

            if (articleId != null && articleId != "") {
                // if item of opId disappeared, show article tree list
                $(".sidebar-menu .items-group>ul").finish();
                opMenu.unfoldItemsGroup($(".tree-list").parent(), false);
            }
        }

        //article menu
        articleMenu.foldAllBranches(false);
        articleMenu.unfoldBranchArea(articleId);
    },

    initialize: function (opId, articleId) {
        opMenu.initOpId = opId;
        opMenu.initArticleId = articleId;

        opMenu.toggleOperation(opId, articleId);
    },

    tail: function () { }
};

// folding all of items at the beginning
opMenu.foldAllOperations(false);
articleMenu.foldAllBranches(false);
var $itemHeaders = $(".sidebar-menu .items-group>.item-header");

$itemHeaders.click(function () {
    $(this).siblings(".item-list, .tree-list").slideToggle("fast");

    // return false;
});

if (!isMobile) {
    // enable hover-intent
    // branch of tree list
    var $articleBranches = $(".tree-list .branch");

    $articleBranches.hoverIntent(function () {
        articleMenu.unfoldBranch($(this), true);
    }, function () {
    });

    // items group of tree list
    var $treeList = $(".tree-list");

    if ($treeList.length > 0) {
        $treeList.parent().hoverIntent(function () {
            opMenu.unfoldItemsGroup($(this), true);
        }, function () {
        });
    }

    $(".sidebar-menu .hover-intent-notice").show();

    //離開選單後回預設值
    $(".sidebar-container").mouseleave(function () {
        opMenu.toggleOperation(opMenu.initOpId, opMenu.initArticleId);
    });
}

// open all of operations
$("#btnExpand").click(function () {
    opMenu.unfoldAllOperations(true);
    return false;
});

// switch sidebar menu area
var $sidebarMenu = $(".sidebar-menu");
var $sidebarMenuCtrlArea = $("#SidebarMenuCtrlArea");
var sidebarMenuMode = "";

function initSidebarMenu(mode) {
    if (mode == "desktop") {
        $sidebarMenu.show();
        $sidebarMenuCtrlArea.hide();
        sidebarMenuMode = mode;
    } else if (mode == "mobile") {
        $sidebarMenu.hide();
        $sidebarMenuCtrlArea.show();
        sidebarMenuMode = mode;
    }
}

function updateSidebarMenuStatus(w) {
    if (w >= 768) {
        if (sidebarMenuMode != "desktop") {
            initSidebarMenu("desktop");
        }
    } else {
        if (sidebarMenuMode != "mobile") {
            initSidebarMenu("mobile");
        }
    }
}

$(window).resize(function () {
    // var w = window.innerWidth;
    var w = $("#mainNavbar").outerWidth();
    updateSidebarMenuStatus(w);
});

$("#btnToggleSidebarMenu").click(function () {
    $sidebarMenu.slideToggle("fast");
    return false;
});

// updateSidebarMenuStatus(window.innerWidth);
updateSidebarMenuStatus($("#mainNavbar").outerWidth());
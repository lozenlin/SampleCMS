/// <reference path="jquery-3.2.1.js"/>
/// <reference path="../noUiSlider/nouislider.js"/>

// select operation and config privileges

$(".op-area").click(function () {
    $(this).siblings().removeClass("active");
    $(this).addClass("active");

    var opId = $(this).attr("opid");
    var $configPanel = $(".privilege-config-panel");
    $configPanel.show();
    $configPanel.find(".seqno").html($(this).find(".seqno").html() + ". ");
    $configPanel.find(".op-name").html($(this).find(".op-name").html());

    var pvgOfSubitemSelf = $(this).find(".hidPvgOfSubitemSelf").val();

    var canAddSubItemOfSelf = false;
    if (pvgOfSubitemSelf & 4 == 4) {
        canAddSubItemOfSelf = true;
    }
    
    var $chkAdd = $configPanel.find("#chkAdd");
    if ($chkAdd.length > 0) {
        $chkAdd[0].checked = canAddSubItemOfSelf;
    }
});


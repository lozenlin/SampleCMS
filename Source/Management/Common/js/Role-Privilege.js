/// <reference path="jquery-3.2.1.js"/>
/// <reference path="../noUiSlider/nouislider.js"/>

var ctlPvgOfItem = document.getElementById("ctlPvgOfItem");
noUiSlider.create(ctlPvgOfItem, {
    start: 0,
    step: 1,
    connect: [true, false],
    range: {
        "min": 0,
        "max": 2
    }
});

var ctlPvgOfSubitemSelf = document.getElementById("ctlPvgOfSubitemSelf");
noUiSlider.create(ctlPvgOfSubitemSelf, {
    start: 0,
    step: 1,
    connect: [true, false],
    range: {
        "min": 0,
        "max": 3
    }
});

var ctlPvgOfSubitemCrew = document.getElementById("ctlPvgOfSubitemCrew");
noUiSlider.create(ctlPvgOfSubitemCrew, {
    start: 0,
    step: 1,
    connect: [true, false],
    range: {
        "min": 0,
        "max": 3
    }
});

var ctlPvgOfSubitemOthers = document.getElementById("ctlPvgOfSubitemOthers");
noUiSlider.create(ctlPvgOfSubitemOthers, {
    start: 0,
    step: 1,
    connect: [true, false],
    range: {
        "min": 0,
        "max": 3
    }
});

// select operation and config privileges

$(".op-area").click(function () {
    $(this).siblings().removeClass("active");
    $(this).addClass("active");

    var opId = $(this).attr("opid");
    var $configPanel = $(".privilege-config-panel");
    $configPanel.show();
    $configPanel.find(".seqno").html($(this).find(".seqno").html() + ". ");
    $configPanel.find(".op-name").html($(this).find(".op-name").html());

    var pvgOfItem = $(this).find(".hidPvgOfItem").val();
    var pvgOfSubitemSelf = $(this).find(".hidPvgOfSubitemSelf").val();
    var pvgOfSubitemCrew = $(this).find(".hidPvgOfSubitemCrew").val();
    var pvgOfSubitemOthers = $(this).find(".hidPvgOfSubitemOthers").val();

    setSlider(ctlPvgOfItem, pvgOfItem);
    setSlider(ctlPvgOfSubitemSelf, pvgOfSubitemSelf);
    setSlider(ctlPvgOfSubitemCrew, pvgOfSubitemCrew);
    setSlider(ctlPvgOfSubitemOthers, pvgOfSubitemOthers);

    var canAddSubItemOfSelf = false;
    if ((pvgOfSubitemSelf & 4) == 4) {
        canAddSubItemOfSelf = true;
    }

    var $chkAdd = $configPanel.find("#chkAdd");
    if ($chkAdd.length > 0) {
        $chkAdd[0].checked = canAddSubItemOfSelf;
    }
});

function setSlider(sliderRoot, serverValue) {
    var value = 0;

    if ((serverValue & 8) == 8) {
        value = 3;
    } else if ((serverValue & 2) == 2) {
        value = 2;
    } else if ((serverValue & 1) == 1) {
        value = 1;
    }

    sliderRoot.noUiSlider.set(value);
    setConnectBarColor($(sliderRoot), value);
}

function setConnectBarColor($sliderRoot, value) {
    var $connBar = $sliderRoot.find(".noUi-connect");

    $connBar
        .removeClass("bg-warning")
        .removeClass("bg-success")
        .removeClass("bg-primary");

    switch (parseInt(value)) {
        case 1:
            $connBar.addClass("bg-warning");
            break;
        case 2:
            $connBar.addClass("bg-success");
            break;
        case 3:
            $connBar.addClass("bg-primary");
            break;
    }
}

ctlPvgOfItem.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfItem), result)
});

ctlPvgOfSubitemSelf.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfSubitemSelf), result)
});

ctlPvgOfSubitemCrew.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfSubitemCrew), result)
});

ctlPvgOfSubitemOthers.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfSubitemOthers), result)
});


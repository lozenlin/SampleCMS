/// <reference path="jquery-3.2.1.js"/>
/// <reference path="../noUiSlider/nouislider.js"/>
/// <reference path="dao.js"/>

// create sliders

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

// functions of select operation and config privileges

$(".op-area").click(function () {
    $(this).siblings().removeClass("active");
    $(this).addClass("active");

    var opId = $(this).attr("opid");
    var $configPanel = $(".privilege-config-panel");
    $configPanel.show();
    $configPanel.find(".seqno").html($(this).find(".seqno").html() + ". ");
    $configPanel.find(".op-name").html($(this).find(".op-name>.subject").html());

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
            // read
            $connBar.addClass("bg-warning");
            break;
        case 2:
            // edit
            $connBar.addClass("bg-success");
            break;
        case 3:
            // delete
            $connBar.addClass("bg-primary");
            break;
    }
}

function applyPvgLimitation(itemValue, selfValue, crewValue, othersValue, from) {
    // itemValue, selfValue, crewValue, othersValue: -1=ignore, 0:not-allowed, 1:can-read, 2:can-edit, 3:can-delete
    // from: item, self, add, crew, others

    var resultOfItem = ctlPvgOfItem.noUiSlider.get();
    var resultOfSelf = ctlPvgOfSubitemSelf.noUiSlider.get();
    var resultOfCrew = ctlPvgOfSubitemCrew.noUiSlider.get();
    var resultOfOthers = ctlPvgOfSubitemOthers.noUiSlider.get();
    var resultOfAdd = $("#chkAdd")[0].checked;

    var newValueOfItem = 0;
    var newValueOfSelf = 0;
    var newValueOfCrew = 0;
    var newValueOfOthers = 0;
    var newValueOfAdd = false;

    var reducePvg = false;
    var extendPvg = false;

    // Privilege of item
    if (itemValue > -1) {
        switch (from) {
            case "item":
                reducePvg = true;
                break;
            case "self":
                extendPvg = true;
                break;
        }

        if (reducePvg) {
            if (itemValue == 0) {
                // reduce privilege of subitem-slef
                if (resultOfSelf > 0) {
                    newValueOfSelf = 0;
                    applyPvgLimitation(-1, newValueOfSelf, -1, -1, "item");
                }
            }
        }

        if (extendPvg) {
            // extend privilege of item
            ctlPvgOfItem.noUiSlider.set(itemValue);
            setConnectBarColor($(ctlPvgOfItem), itemValue);
        }
    }

    // Privilege of subitem-self
    if (selfValue > -1) {
        if (from != "self") {
            ctlPvgOfSubitemSelf.noUiSlider.set(selfValue);
            setConnectBarColor($(ctlPvgOfSubitemSelf), selfValue);
        }

        switch (from) {
            case "item":
                reducePvg = true;
                break;
            case "self":
                reducePvg = true;
                extendPvg = true;
                break;
            case "add":
            case "crew":
                extendPvg = true;
                break;
        }

        if (reducePvg) {
            // reduce privilege of subitem-crew
            if (resultOfCrew > selfValue) {
                newValueOfCrew = selfValue;
                applyPvgLimitation(-1, -1, newValueOfCrew, -1, "self");
            }

            // reducre privilege of subitem-self-add
            if (selfValue < 2 && resultOfAdd == true) {
                newValueOfAdd = false;
                $("#chkAdd")[0].checked = newValueOfAdd;
            }
        }

        if (extendPvg) {
            // extend privilege of item
            if (selfValue > 0 && resultOfItem == 0) {
                newValueOfItem = 1;
                applyPvgLimitation(newValueOfItem, -1, -1, -1, "self");
            }
        }
    }

    // Privilege of subitem-crew
    if (crewValue > -1) {
        if (from != "crew") {
            ctlPvgOfSubitemCrew.noUiSlider.set(crewValue);
            setConnectBarColor($(ctlPvgOfSubitemCrew), crewValue);
        }

        switch (from) {
            case "self":
                reducePvg = true;
                break;
            case "crew":
                reducePvg = true;
                extendPvg = true;
                break;
            case "others":
                extendPvg = true;
                break;
        }

        if (reducePvg) {
            // reduce privilege of subitem-others
            if (resultOfOthers > crewValue) {
                newValueOfOthers = crewValue;
                applyPvgLimitation(-1, -1, -1, newValueOfOthers, "crew");
            }
        }

        if (extendPvg) {
            // extend privilege of subitem-self
            if (resultOfSelf < crewValue) {
                newValueOfSelf = crewValue;
                applyPvgLimitation(-1, newValueOfSelf, -1, -1, "crew");
            }
        }
    }

    // Privilege of subitem-others
    if (othersValue > -1) {
        switch (from) {
            case "crew":
                reducePvg = true;
                break;
            case "others":
                extendPvg = true;
                break;
        }

        if (reducePvg) {
            // reduce privilege of subitem-others
            ctlPvgOfSubitemOthers.noUiSlider.set(othersValue);
            setConnectBarColor($(ctlPvgOfSubitemOthers), othersValue);
        }

        if (extendPvg) {
            // extend privilege of subitem-crew
            if (resultOfCrew < othersValue) {
                newValueOfCrew = othersValue;
                applyPvgLimitation(-1, -1, newValueOfCrew, -1, "others");
            }
        }
    }
}

function can(typeName, serverValue) {
    if (typeName == "doNothing" && serverValue == 0) {
        return true;
    } else if (typeName == "read" && (serverValue & 1) == 1) {
        return true;
    } else if (typeName == "edit" && (serverValue & 2) == 2) {
        return true;
    } else if (typeName == "add" && (serverValue & 4) == 4) {
        return true;
    } else if (typeName == "delete" && (serverValue & 8) == 8) {
        return true;
    }

    return false;
}

function refreshTags($pvgTags, serverValue) {
    $pvgTags.html("");
    if (can("doNothing", serverValue)) {
        $pvgTags.html(tagHtmlNotAllowed);
    } else {
        if (can("read", serverValue)) {
            $pvgTags.html($pvgTags.html() + tagHtmlRead);
        }

        if (can("edit", serverValue)) {
            $pvgTags.html($pvgTags.html() + tagHtmlEdit);
        }

        if (can("add", serverValue)) {
            $pvgTags.html($pvgTags.html() + tagHtmlAdd);
        }

        if (can("delete", serverValue)) {
            $pvgTags.html($pvgTags.html() + tagHtmlDelete);
        }
    }
}

function sendPvgsToServer() {
    var roleName = $("#roleName").html();
    var roleId = $("#hidRoleId").val();
    var $activeOpArea = $(".op-area.active");
    var $status = null;
    var opId = 0;

    if ($activeOpArea.length > 0) {
        opId = $activeOpArea.eq(0).attr("opid");
        $status = $activeOpArea.find(".status");
    } else {
        alert("get operation failed.");
        return;
    }

    var itemVal = parseInt(ctlPvgOfItem.noUiSlider.get());
    var selfVal = parseInt(ctlPvgOfSubitemSelf.noUiSlider.get());
    var crewVal = parseInt(ctlPvgOfSubitemCrew.noUiSlider.get());
    var othersVal = parseInt(ctlPvgOfSubitemOthers.noUiSlider.get());
    var addval = $("#chkAdd")[0].checked;

    if ($status != null) {
        $status.html(status_sending + "...");
    }

    dao.TempStoreRolePvg(roleName, opId, itemVal,
        selfVal, crewVal, othersVal,
        addval, roleId,
        function (cr) {
            if (cr.b) {
                var pvg = cr.o;
                console.log("retrived data of " + pvg.RoleName + " opid:" + pvg.OpId);
                
                var $opArea = $(".op-area[opid='" + pvg.OpId + "']");
                var $itemTags = $opArea.find(".item .tags");
                var $subitemSelfTags = $opArea.find(".subitem-self .tags");
                var $subitemCrewTags = $opArea.find(".subitem-crew .tags");
                var $subitemOthersTags = $opArea.find(".subitem-others .tags");
                var $pvgOfItem = $opArea.find(".hidPvgOfItem");
                var $pvgOfSubitemSelf = $opArea.find(".hidPvgOfSubitemSelf");
                var $pvgOfSubitemCrew = $opArea.find(".hidPvgOfSubitemCrew");
                var $pvgOfSubitemOthers = $opArea.find(".hidPvgOfSubitemOthers");

                $pvgOfItem.val(pvg.PvgOfItem);
                $pvgOfSubitemSelf.val(pvg.PvgOfSubitemSelf);
                $pvgOfSubitemCrew.val(pvg.PvgOfSubitemCrew);
                $pvgOfSubitemOthers.val(pvg.PvgOfSubitemOthers);

                refreshTags($itemTags, $pvgOfItem.val());
                refreshTags($subitemSelfTags, $pvgOfSubitemSelf.val());
                refreshTags($subitemCrewTags, $pvgOfSubitemCrew.val());
                refreshTags($subitemOthersTags, $pvgOfSubitemOthers.val());

                $status.html(status_temporarily_stored);
            } else {
                $status.html(status_sent_failed);
                alert(cr.err);
            }
        });
}

ctlPvgOfItem.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfItem), result);
    applyPvgLimitation(result, -1, -1, -1, "item");
    sendPvgsToServer();
});

ctlPvgOfSubitemSelf.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfSubitemSelf), result);
    applyPvgLimitation(-1, result, -1, -1, "self");
    sendPvgsToServer();
});

ctlPvgOfSubitemCrew.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfSubitemCrew), result);
    applyPvgLimitation(-1, -1, result, -1, "crew");
    sendPvgsToServer();
});

ctlPvgOfSubitemOthers.noUiSlider.on("change", function () {
    var result = this.get();
    setConnectBarColor($(ctlPvgOfSubitemOthers), result);
    applyPvgLimitation(-1, -1, -1, result, "others");
    sendPvgsToServer();
});

$("#chkAdd").change(function () {
    if (this.checked) {
        var resultOfSelf = ctlPvgOfSubitemSelf.noUiSlider.get();
        if (resultOfSelf < 2) {
            var newValueOfSelf = 2;
            applyPvgLimitation(-1, newValueOfSelf, -1, -1, "add");
        }
    }

    sendPvgsToServer();
});

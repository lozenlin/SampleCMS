
function smoothUp(id) {
    if (id == undefined)
        id = "";

    $("html, body").animate({
        scrollTop: (id == "") ? 0 : $("#" + id).offset().top
    }, 500);
}

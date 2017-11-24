// reference: https://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript/3855394#3855394
/*
//Get a param
$.QueryString.param
//-or-
$.QueryString["param"]
//This outputs something like...
//"val"

//Get all params as object
$.QueryString
//This outputs something like...
//Object { param: "val", param2: "val" }

//Set a param (only in the $.QueryString object, doesn't affect the browser's querystring)
$.QueryString.param = "newvalue"
//This doesn't output anything, it just updates the $.QueryString object

//Convert object into string suitable for url a querystring (Requires jQuery)
$.param($.QueryString)
//This outputs something like...
//"param=newvalue&param2=val"

//Update the url/querystring in the browser's location bar with the $.QueryString object
history.replaceState({}, '', "?" + $.param($.QueryString));
//-or-
history.pushState({}, '', "?" + $.param($.QueryString));
*/

(function($) {
    $.QueryString = (function(paramsArray) {
        let params = {};

        for (let i = 0; i < paramsArray.length; ++i)
        {
            let param = paramsArray[i]
                .split('=', 2);

            if (param.length !== 2)
                continue;

            params[param[0]] = decodeURIComponent(param[1].replace(/\+/g, " "));
        }

        return params;
    })(window.location.search.substr(1).split('&'))
})(jQuery);

(function(window, angular) {
    'use strict';
    angular.module('FileManagerApp', ['pascalprecht.translate', 'ngFileUpload']);

    // 2018/04/06, lozen_lin, fixes the problem "cannot open context menu in iOS devices."
    var isJustOpenedByIOSclient = false;
    var isIOS = false;

    if (navigator) {
        if (navigator.userAgent.match(/(iPhone|iPad);/i) != null) {
            isIOS = true;
        }
    }

    /**
     * jQuery inits
     */
    angular.element(window.document).on('shown.bs.modal', '.modal', function() {
        window.setTimeout(function() {
            angular.element('[autofocus]', this).focus();
        }.bind(this), 100);
    });

    // 2018/04/06, lozen_lin
    angular.element(window.document).on('click', '.item-list.ng-scope, .item-list .thumbnail', function (e) {
        if (isIOS) {
            var menu = angular.element('#context-menu');

            if (e.pageX >= window.innerWidth - menu.width()) {
                e.pageX -= menu.width();
            }
            if (e.pageY >= window.innerHeight - menu.height()) {
                e.pageY -= menu.height();
            }

            menu.hide().css({
                left: e.pageX,
                top: e.pageY
            }).appendTo('body').show();

            isJustOpenedByIOSclient = true;
        }
    });

    angular.element(window.document).on('click', function() {
        if (isIOS) {
            // 2018/04/06, lozen_lin
            if (isJustOpenedByIOSclient) {
                isJustOpenedByIOSclient = false;
            } else {
                angular.element('#context-menu').hide();
            }
        } else {
            angular.element('#context-menu').hide();
        }
    });

    angular.element(window.document).on('contextmenu', '.main-navigation .table-files tr.item-list:has("td"), .item-list', function(e) {
        var menu = angular.element('#context-menu');

        if (e.pageX >= window.innerWidth - menu.width()) {
            e.pageX -= menu.width();
        }
        if (e.pageY >= window.innerHeight - menu.height()) {
            e.pageY -= menu.height();
        }

        menu.hide().css({
            left: e.pageX,
            top: e.pageY
        }).appendTo('body').show();
        e.preventDefault();
    });

    if (! Array.prototype.find) {
        Array.prototype.find = function(predicate) {
            if (this == null) {
                throw new TypeError('Array.prototype.find called on null or undefined');
            }
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }
            var list = Object(this);
            var length = list.length >>> 0;
            var thisArg = arguments[1];
            var value;

            for (var i = 0; i < length; i++) {
                value = list[i];
                if (predicate.call(thisArg, value, i, list)) {
                    return value;
                }
            }
            return undefined;
        };
    }

})(window, angular);

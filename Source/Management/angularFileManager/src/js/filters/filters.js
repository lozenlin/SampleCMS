(function(angular) {
    'use strict';
    var app = angular.module('FileManagerApp');

    app.filter('strLimit', ['$filter', function($filter) {
        return function(input, limit, more) {
            if (input.length <= limit) {
                return input;
            }
            return $filter('limitTo')(input, limit) + (more || '...');
        };
    }]);

    app.filter('fileExtension', ['$filter', function($filter) {
        return function(input) {
            return /\./.test(input) && $filter('strLimit')(input.split('.').pop(), 3, '..') || '';
        };
    }]);

    app.filter('formatDate', ['$filter', function () {
        function dateToStr(input) {
            // 2017/11/24, lozen_lin, modify, fix time 19:05 (browser at Taiwan) show 11:05
            // original: var str = input.toISOString().substring(0, 19).replace('T', ' ');

            var y = input.getFullYear();
            var M = input.getMonth() + 1;
            var d = input.getDate();
            var h = input.getHours();
            var m = input.getMinutes();
            var s = input.getSeconds();

            if (M < 10) M = "0" + M;
            if (d < 10) d = "0" + d;
            if (h < 10) h = "0" + h;
            if (m < 10) m = "0" + m;
            if (s < 10) s = "0" + s;

            var str = y + "-" + M + "-" + d + " " + h + ":" + m + ":" + s;

            return str;
        }

        return function(input) {
            return input instanceof Date ?
                dateToStr(input) :
                (input.toLocaleString || input.toString).apply(input);
        };
    }]);

    app.filter('humanReadableFileSize', ['$filter', 'fileManagerConfig', function($filter, fileManagerConfig) {
      // See https://en.wikipedia.org/wiki/Binary_prefix
      var decimalByteUnits = [' kB', ' MB', ' GB', ' TB', 'PB', 'EB', 'ZB', 'YB'];
      var binaryByteUnits = ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'];

      return function(input) {
        var i = -1;
        var fileSizeInBytes = input;

        do {
          fileSizeInBytes = fileSizeInBytes / 1024;
          i++;
        } while (fileSizeInBytes > 1024);

        var result = fileManagerConfig.useBinarySizePrefixes ? binaryByteUnits[i] : decimalByteUnits[i];
        return Math.max(fileSizeInBytes, 0.1).toFixed(1) + ' ' + result;
      };
    }]);
})(angular);

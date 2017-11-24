<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="angularFileManager_Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-ng-app="FileManagerApp">
<head runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>angular-filemanager</title>

    <!-- third party -->
    <script src="bower_components/jquery/dist/jquery.min.js"></script>
    <script src="bower_components/angular/angular.min.js"></script>
    <script src="bower_components/angular-translate/angular-translate.min.js"></script>
    <script src="bower_components/ng-file-upload/ng-file-upload.min.js"></script>
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="../Common/js/jquery.QueryString.js"></script>
    <link rel="stylesheet" href="bower_components/bootswatch/paper/bootstrap.min.css" />
    <!-- /third party -->

    <!-- Uncomment if you need to use raw source code
    <script src="src/js/app.js"></script>
    <script src="src/js/directives/directives.js"></script>
    <script src="src/js/filters/filters.js"></script>
    <script src="src/js/providers/config.js"></script>
    <script src="src/js/entities/chmod.js"></script>
    <script src="src/js/entities/item.js"></script>
    <script src="src/js/services/apihandler.js"></script>
    <script src="src/js/services/apimiddleware.js"></script>
    <script src="src/js/services/filenavigator.js"></script>
    <script src="src/js/providers/translations.js"></script>
    <script src="src/js/controllers/main.js"></script>
    <script src="src/js/controllers/selector-controller.js"></script>
    <link href="src/css/animations.css" rel="stylesheet">
    <link href="src/css/dialogs.css" rel="stylesheet">
    <link href="src/css/main.css" rel="stylesheet">
    -->

    <!-- Comment if you need to use raw source code -->
    <link href="dist/angular-filemanager.min.css" rel="stylesheet"/>
    <script src="dist/angular-filemanager.min.js"></script>
    <!-- /Comment if you need to use raw source code -->

    <script type="text/javascript">
        var listType = $.QueryString["listtype"];
        var fnSelected = $.QueryString["fnSelected"];

        //example to override angular-filemanager default config
        angular.module('FileManagerApp').config(['fileManagerConfigProvider', function (config) {
            var defaults = config.$get();
            config.set({
                //appName: 'angular-filemanager',
                listUrl: '../afmService.ashx?listtype=' + listType,
                uploadUrl: '../afmService.ashx?listtype=' + listType,

                pickCallback: function (item) {
                    /*
                    var msg = 'Picked %s "%s" for external use'
                    .replace('%s', item.type)
                    .replace('%s', item.fullPath());
                    window.alert(msg);
                    */

                    if (fnSelected != null && fnSelected != "") {
                        var parentWin = window.opener;

                        if (parentWin == null) {
                            window.parent;
                        }

                        if (parentWin != null) {
                            var iconPath = item.fullPath();

                            if (iconPath.indexOf("/") == 0 && iconPath.length > 1) {
                                iconPath = iconPath.substr(1);
                            }

                            var cmd = "parentWin." + fnSelected + "('" + iconPath + "');";
                            eval(cmd);

                            window.close();
                        }
                    }
                },

                allowedActions: angular.extend(defaults.allowedActions, {
                    upload: true,
                    rename: false,
                    move: false,
                    copy: false,
                    edit: false,
                    changePermissions: false,
                    compress: false,
                    compressChooseName: false,
                    extract: false,
                    download: false,
                    downloadMultiple: false,
                    preview: false,
                    remove: false,
                    createFolder: false,
                    pickFiles: true,
                    pickFolders: false
                }),
            });
        }]);
    </script>
</head>
<body class="ng-cloak">
  <angular-filemanager></angular-filemanager>
</body>
</html>

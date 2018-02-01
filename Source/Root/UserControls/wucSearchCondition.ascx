<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucSearchCondition.ascx.cs" Inherits="UserControls_wucSearchCondition" %>

    <div class="form-inline">
        <div class="form-group">
            <input id="txtKeyword" type="text" class="form-control" placeholder='<%= Resources.Lang.SearchResult_txtKeyword_Hint %>' accesskey="S" autocomplete="off" 
                title='<%= Resources.Lang.SearchResult_txtKeyword_Hint %>' />
        </div>
        <div class="form-group">
            <a id="btnToSearchResult" href="#" class="btn btn-primary btn-sm" style="margin-bottom:0;" title='<%= Resources.Lang.SearchResult_btnSearch_Hint %>'>
                <i class="glyphicon glyphicon-search"></i></a>
            <a id="btnOpenUrl" href="#" style="display:none;"></a>
            <%-- target="_blank" --%>
        </div>
    </div>
    <script>
        $(function () {
            var $txtKeyword = $("#txtKeyword");

            $txtKeyword.autocomplete({
                source: function (request, response) {
                    dao.Keyword_GetList(request.term, response);
                }
            });

            $txtKeyword.keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();

                    $("#btnToSearchResult")[0].click();
                }
            });

            //搜尋鈕
            $("#btnToSearchResult").click(function () {
                var kw = $.trim($txtKeyword.val());

                if (kw == "")
                    return false;

                $("#btnOpenUrl").attr("href", "ToSearchResult.ashx?q=" + encodeURIComponent(kw) + "&l=" + langNo);
                $("#btnOpenUrl")[0].click();

                return false;
            });
        });
    </script>

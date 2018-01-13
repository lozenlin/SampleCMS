<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucSearchConditionPost.ascx.cs" Inherits="UserControls_wucSearchConditionPost" %>

    <asp:Panel ID="pnlSearchCondition" runat="server" CssClass="form-inline" DefaultButton="btnToSearchResult">
        <div class="form-group">
            <asp:TextBox ID="txtKeyword" runat="server" CssClass="form-control" accesskey="S" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:LinkButton ID="btnToSearchResult" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnToSearchResult_Click"
                ToolTip="Search" style="margin-bottom:0;">
                <i class="glyphicon glyphicon-search"></i></asp:LinkButton>
        </div>
    </asp:Panel>
    <script>
        $(function () {
            var $txtKeyword = $("#txtKeyword");

            $txtKeyword.autocomplete({
                source: function (request, response) {
                    dao.Keyword_GetList(request.term, response);
                }
            });
        });
    </script>
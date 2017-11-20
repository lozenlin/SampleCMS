<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucDataPager.ascx.cs" Inherits="UserControls_wucDataPager" %>
<div id="PaginationArea" runat="server" class="PageArea d-flex flex-md-row flex-column justify-content-center">
    <div class="text">
        <a id="btnFirstPage" runat="server" href="#">第一頁</a>
    </div>
    <ul class="pagination">
        <li id="PreviousPageArea" runat="server" class="page-item">
            <a id="btnPreviousPage" runat="server" href="#" class="page-link" title="上一頁">«</a>
        </li>
        <asp:Repeater ID="rptPagination" runat="server" OnItemDataBound="rptPagination_ItemDataBound">
        <ItemTemplate>
            <li id="PageCodeArea" runat="server" class="page-item">
                <a id="btnPageCode" runat="server" href="#" class="page-link">1</a>
            </li>
        </ItemTemplate>
        </asp:Repeater>
        <li id="NextPageArea" runat="server" class="page-item disabled">
            <a id="btnNextPage" runat="server" href="#" class="page-link" title="下一頁">»</a>
        </li>
    </ul>
    <div class="text pl-md-2">
        <a id="btnLastPage" runat="server" href="#">最後頁</a>
    </div>
</div>
<div id="PaginationInfoArea" runat="server" class="PageArea d-flex flex-md-row flex-column justify-content-center">
    <div class="text">
        <asp:Literal ID="ltrTotalCount" runat="server" Text="共 0 筆"></asp:Literal> -
        <%= Resources.Lang.Pager_PageInfo_Head %>
        <asp:Literal ID="ltrCurrentPageCode" runat="server"></asp:Literal>
        <%= Resources.Lang.Pager_PageInfo_Mid %>
        <asp:Literal ID="ltrLastPageCode" runat="server"></asp:Literal>
        <%= Resources.Lang.Pager_PageInfo_Tail %> -
        <%= Resources.Lang.Pager_JumpTo %>
        <asp:DropDownList ID="ddlPageSelect" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSelect_SelectedIndexChanged"></asp:DropDownList>
        <span id="TextCtrlArea" runat="server" visible="false">
            <asp:TextBox ID="txtPageCode" runat="server" Width="30px" MaxLength="9" autocomplete="off" ToolTip="輸入頁碼後可按 Enter 送出"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPageCode" runat="server" ControlToValidate="txtPageCode" CssClass="text-danger"
                Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="P" ></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="rvPageCode" runat="server" ControlToValidate="txtPageCode" CssClass="text-danger" Display="Dynamic"
                ErrorMessage="*請輸入 1 以上的數字" SetFocusOnError="true" Type="Integer" ValidationGroup="P" MinimumValue="1" MaximumValue="999999999"></asp:RangeValidator>
            <asp:LinkButton ID="btnJumpToPage" runat="server" OnClick="btnJumpToPage_Click" ValidationGroup="P"
                CssClass="btn btn-sm btn-success" ToolTip="執行跳頁">執行跳頁</asp:LinkButton>
        </span>
    </div>
</div>
<script>
    $(function () {
        var btnJumpToPageId = '<%= btnJumpToPage.ClientID %>';
        var txtPageCodeId = '<%= txtPageCode.ClientID %>';
        var $txtPageCode = $("#" + txtPageCodeId);

        $txtPageCode.tooltip();
        $txtPageCode.keyup(function (event) {
            if (event.which == 13) {
                //enter
                $("#" + btnJumpToPageId)[0].click();
            }
        });
    });
</script>
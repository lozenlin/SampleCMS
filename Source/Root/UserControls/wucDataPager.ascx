<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucDataPager.ascx.cs" Inherits="UserControls_wucDataPager" %>
<div id="PaginationArea" runat="server" class="PageArea">
    <ul class="pagination">
        <li id="PreviousPageArea" runat="server">
            <a id="btnPreviousPage" runat="server" href="#" aria-label="Previous">
                <span aria-hidden="true">«</span></a>
        </li>
        <asp:Repeater ID="rptPagination" runat="server" OnItemDataBound="rptPagination_ItemDataBound">
        <ItemTemplate>
            <li id="PageCodeArea" runat="server">
                <a id="btnPageCode" runat="server" href="#">1</a>
            </li>
        </ItemTemplate>
        </asp:Repeater>
        <li id="NextPageArea" runat="server">
            <a id="btnNextPage" runat="server" href="#" aria-label="Next">
                <span aria-hidden="true">»</span></a>
        </li>
    </ul>
</div>

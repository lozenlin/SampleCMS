<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListItemsThumb.ascx.cs" Inherits="LayoutControls_ListItemsThumb" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Repeater ID="rptSubitems" runat="server" OnItemDataBound="rptSubitems_ItemDataBound">
<ItemTemplate>
    <div class="row list-item-thumb">
        <div class="col-xs-3">
            <a id="btnPic" runat="server" href="#" class="thumbnail">
                <img id="imgPic" runat="server" src="/images/project_7.jpg" alt="*" class="img-responsive"/>
            </a>
        </div>
        <div class="col-xs-9">
            <h2><a id="btnItem" runat="server" href="#"><%# basePage.EvalToSafeStr("ArticleSubject") %></a></h2>
            <p class="descText"><%# basePage.EvalToSafeStr("TextContext") %></p>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>

<uc1:wucDataPager ID="ucDataPager" runat="server" />

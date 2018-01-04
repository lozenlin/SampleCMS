<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListItemsThumb.ascx.cs" Inherits="LayoutControls_ListItemsThumb" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

    <asp:Repeater ID="rptSubitems" runat="server" OnItemDataBound="rptSubitems_ItemDataBound">
        <HeaderTemplate>
            <div class="list-thumb">
        </HeaderTemplate>
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
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:PlaceHolder ID="LazyLoadingArea" runat="server" Visible="false">
        <div class="list-thumb">

        </div>
    </asp:PlaceHolder>
    <div id="LazyLoadingCtrlArea" runat="server" class="lazy-loading-ctrls thumbCtrls" visible="false">
        <a href="#" class="btnLoad">載入更多</a>
        <span class="loadingIcon" style="display:none;"><img src="/images/Spinner.gif" alt="載入中" width="80" /></span>
        <span class="isLastData" style="display:none;">已到最後一筆</span>
    </div>
    <uc1:wucDataPager ID="ucDataPager" runat="server" />

<%@ Page Language="C#" MasterPageFile="~/MasterArticle.master" AutoEventWireup="true" CodeFile="Article.aspx.cs" Inherits="Article" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>
<%@ OutputCache Location="Server" VaryByParam="artid;l;p;preview" Duration="1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <asp:PlaceHolder ID="SubitemsArea" runat="server" Visible="false">
        <div class="list-group item-list">
        <asp:Repeater ID="rptSubitems" runat="server" OnItemDataBound="rptSubitems_ItemDataBound">
        <ItemTemplate>
            <a id="btnItem" runat="server" href="#" class="list-group-item">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="item-date text-primary">
                            <%# EvalToSafeStr("PublishDate", "{0:yyyy-MM-dd}") %>
                        </div>
                    </div>
                    <div class="col-sm-9">
                        <h2 class="list-group-item-heading">
                            <span class="text-primary"><%# EvalToSafeStr("ArticleSubject") %></span>
                        </h2>
                        <p class="list-group-item-text descText">
                            <%# EvalToSafeStr("TextContext") %>
                        </p>
                    </div>
                </div>
            </a>
        </ItemTemplate>
        </asp:Repeater>
        </div>

        <uc1:wucDataPager ID="ucDataPager" runat="server" />
    </asp:PlaceHolder>
    <asp:Literal ID="ltrArticleContext" runat="server"></asp:Literal>
    <asp:PlaceHolder ID="ControlArea" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


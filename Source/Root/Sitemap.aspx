<%@ Page Language="C#" MasterPageFile="~/MasterArticle.master" AutoEventWireup="true" CodeFile="Sitemap.aspx.cs" Inherits="Sitemap" %>
<%@ OutputCache Location="Server" VaryByParam="l;preview" Duration="1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
	<div class="fh5co-spacer fh5co-spacer-sm"></div>
    <ul class="sitemap-list">
    <asp:Repeater ID="rptSitemapLinks" runat="server" OnItemDataBound="rptSitemapLinks_ItemDataBound">
    <ItemTemplate>
        <li>
            <h2><a id="btnItem" runat="server" href="#">x. Subject</a></h2>
            <asp:Repeater ID="rptSubitems" runat="server" OnItemDataBound="rptSitemapLinks_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <a id="btnItem" runat="server" href="#">x-y. Subject</a>
                        <asp:Repeater ID="rptSubitems" runat="server" OnItemDataBound="rptSitemapLinks_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <a id="btnItem" runat="server" href="#">x-y. Subject</a>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </li>
    </ItemTemplate>
    </asp:Repeater>
    </ul>
    <div class="row" style="display:none;">
        <div class="col-md-push-1 col-md-6">
	        <div class="fh5co-spacer fh5co-spacer-sm"></div>
            <table class="table table-bordered table-condensed small">
                <tbody>
                    <tr>
                        <th style="width:20%;">Website Ver.</th>
                        <td style="width:80%;">
                            <asp:Literal ID="ltrSystemVersion" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th>Common.LogicObject Ver.</th>
                        <td>
                            <asp:Literal ID="ltrLogicObjectVersion" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th>Common.DataAccess Ver.</th>
                        <td>
                            <asp:Literal ID="ltrDataAccessVersion" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th>Common.Utility Ver.</th>
                        <td>
                            <asp:Literal ID="ltrUtilityVersion" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContentOfHomePage" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterArticle.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>
<%@ Register Src="~/UserControls/wucSearchCondition.ascx" TagPrefix="uc1" TagName="wucSearchCondition" %>
<%@ OutputCache Location="Server" VaryByParam="l;preview" Duration="1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
</asp:Content>
<asp:Content ID="cContentOfHomePage" ContentPlaceHolderID="cphContentOfHomePage" Runat="Server">
    <div class="container">
        <div class="row">
            <div class="col-md-8">

            </div>
            <div class="col-md-4">
                <uc1:wucSearchCondition ID="ucSearchCondition" runat="server" />
            </div>
        </div>
    </div>

    <asp:Literal ID="ltrContext" runat="server"></asp:Literal>

	<section id="fh5co-newsletter">
		<div class="container SitemapInHome">
			<div class="row">
			    <div class="col-md-12">
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
				</div>
			</div>
		</div>
	</section>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


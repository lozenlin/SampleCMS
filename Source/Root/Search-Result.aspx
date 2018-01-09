﻿<%@ Page Language="C#" MasterPageFile="~/MasterArticle.master" AutoEventWireup="true" CodeFile="Search-Result.aspx.cs" Inherits="Search_Result" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script>
        var txtKeywordId = '<%= txtKeyword.ClientID %>';

        function checkKw() {
            var kw = $.trim($("#" + txtKeywordId).val());

            if (kw == "")
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <asp:Panel ID="pnlSearchCondition" runat="server" CssClass="row" DefaultButton="btnToSearchResult">
        <div class="col-sm-8 col-sm-offset-1">
            <div class="form-group">
                <asp:TextBox ID="txtKeyword" runat="server" CssClass="form-control" placeholder="keyword" accesskey="S" autocomplete="off"></asp:TextBox>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="form-group">
                <asp:LinkButton ID="btnToSearchResult" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnToSearchResult_Click"
                    ToolTip="Search" style="margin-bottom:0;" OnClientClick="return checkKw();">
                    <i class="glyphicon glyphicon-search"></i> Search</asp:LinkButton>
            </div>
        </div>
    </asp:Panel>
    <div class="row">
        <div class="col-sm-11 col-sm-offset-1">
            <b>Keyword:</b> <span class="text-success"><asp:Literal ID="ltrKeywords" runat="server"></asp:Literal></span>
        </div>
        <div class="col-sm-11 col-sm-offset-1">
            <b>Total:</b> <span class="text-success"><asp:Literal ID="ltrTotal" runat="server"></asp:Literal></span>
        </div>
    </div>
	<div class="fh5co-spacer fh5co-spacer-sm"></div>
    <div class="search-result">
    <asp:Repeater ID="rptResultItems" runat="server" OnItemDataBound="rptResultItems_ItemDataBound">
    <ItemTemplate>
        <div class="row result-item">
            <div class="col-xs-12">
                <h2 class="subject">
                    <a id="btnSubject" runat="server" href="#" target="_blank">Subject</a>
                </h2>
                <p class="descText"><asp:Literal ID="ltrContext" runat="server"></asp:Literal></p>
                <ol class="breadcrumb item-route">
                    <li>Position: </li>
                    <asp:Literal ID="ltrBreadcrumb" runat="server"></asp:Literal>
                </ol>
            </div>
        </div>
    </ItemTemplate>
    </asp:Repeater>
    </div>

    <uc1:wucDataPager ID="ucDataPager" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContentOfHomePage" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Pick-LayoutControl.aspx.cs" Inherits="Pick_LayoutControl" %>
<%@ MasterType TypeName="MasterConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        版型控制項
    </div>
    <div class="container-fluid">
        <div class="row LayoutControls">
            <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound" OnItemCommand="rptList_ItemCommand">
            <ItemTemplate>
                <div class="col-sm-3">
                    <div class="card">
                        <a id="btnPic" runat="server" class="ThumbArea" href="#" target="_blank">
                            <img id="imgPic" runat="server" class="card-img-top" src="BPimages/LayoutControl/ListItemsThumb.png" alt="*"/></a>
                        <div class="card-body">
                            <h5 id="ctlNameArea" runat="server" class="card-title"></h5>
                            <asp:LinkButton ID="btnSelect" runat="server" CssClass="btn btn-primary" CommandName="sel">選擇</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActionButtons" Runat="Server">
    <a href="#" class="btn btn-light" onclick="closeThisForm(); return false;"><%= Resources.Lang.ConfigForm_btnCancel %></a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


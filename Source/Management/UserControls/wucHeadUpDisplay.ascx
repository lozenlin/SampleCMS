<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucHeadUpDisplay.ascx.cs" Inherits="UserControls_wucHeadUpDisplay" %>
<nav class="breadcrumb">
    <asp:Literal ID="ltrBreadcrumb" runat="server"></asp:Literal>
</nav>
<h5 class="card main-subject text-info">
    <img  id="imgHead" runat="server" src="~/BPimages/icon/data.gif" border="0" align="absMiddle" alt="*" style="width:24px; height:24px;" class="mr-1"/> 
    <asp:Literal ID="ltrHead" runat="server" Text="default_head"></asp:Literal>
</h5>
<div class="sys-controllers">
    <a id="btnBackToParent" runat="server" href="#" class="btn btn-info">
        <i class="fa fa-level-up fa-flip-horizontal"></i> <%= Resources.Lang.Main_btnBackToParent %></a>
    <div class="btn-group">
        <a id="btnEdit" runat="server" href="#" class="btn btn-primary" visible="false">
            <i class="fa fa-pencil-square-o"></i> <asp:Literal ID="ltrEdit" runat="server" Text="修改"></asp:Literal></a>
        <a id="btnAddNew" runat="server" href="#" class="btn btn-primary" visible="false">
            <i class="fa fa-plus"></i> <asp:Literal ID="ltrAddNew" runat="server" Text="新增"></asp:Literal></a>
        <a id="btnCustomPrimary1" runat="server" href="#" class="btn btn-primary" visible="false">自訂主要1</a>
        <a id="btnCustomPrimary2" runat="server" href="#" class="btn btn-primary" visible="false">自訂主要2</a>
    </div>
    <div class="btn-group">
        <a id="btnView" runat="server" href="#" target="_blank" class="btn btn-light border" visible="false"><i class="fa fa-eye"></i> <%= Resources.Lang.Main_btnView %></a>
        <a id="btnViewZhTw" runat="server" href="#" target="_blank" class="btn btn-light border" visible="false"><i class="fa fa-eye"></i> <%= Resources.Lang.Main_btnViewZhTw %></a>
        <a id="btnViewEn" runat="server" href="#" target="_blank" class="btn btn-light border" visible="false"><i class="fa fa-eye"></i> <%= Resources.Lang.Main_btnViewEn %></a>
    </div>
    <div class="btn-group">
        <a id="btnPreview" runat="server" href="#" target="_blank" class="btn btn-secondary" visible="false"><i class="fa fa-eye"></i> <%= Resources.Lang.Main_btnPreview %></a>
        <a id="btnPreviewZhTw" runat="server" href="#" target="_blank" class="btn btn-secondary" visible="false"><i class="fa fa-eye"></i> <%= Resources.Lang.Main_btnPreviewZhTw %></a>
        <a id="btnPreviewEn" runat="server" href="#" target="_blank" class="btn btn-secondary" visible="false"><i class="fa fa-eye"></i> <%= Resources.Lang.Main_btnPreviewEn %></a>
    </div>
</div>
<hr class="content-divider" />

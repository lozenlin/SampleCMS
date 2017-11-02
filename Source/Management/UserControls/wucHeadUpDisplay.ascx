<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wucHeadUpDisplay.ascx.cs" Inherits="UserControls_wucHeadUpDisplay" %>
<nav class="breadcrumb">
    <a href="#" class="breadcrumb-item">首頁</a>
    <span class="breadcrumb-item active">主要資料</span>
    <a href="#" class="breadcrumb-item">網站管理</a>
    <span class="breadcrumb-item active">主要資料 ( 2 )</span>
</nav>
<h5 class="card main-subject text-info">
    <img  id="imgHead" runat="server" src="~/BPimages/icon/data.gif" border="0" align="absMiddle" alt="*" style="width:24px; height:24px;"/> 
    <asp:Literal ID="ltrHead" runat="server"></asp:Literal>
</h5>
<div class="sys-controllers">
    <a id="btnBackToParent" runat="server" href="#" class="btn btn-info"><i class="fa fa-level-up fa-flip-horizontal"></i> 回上層</a>
    <div class="btn-group">
        <a id="btnEdit" runat="server" href="javascript:popWin('Config-Form.html', 700, 630);" class="btn btn-primary" visible="false">
            <i class="fa fa-pencil-square-o"></i> <asp:Literal ID="ltrEdit" runat="server" Text="修改"></asp:Literal></a>
        <a id="btnAddNew" runat="server" href="javascript:popWin('Config-Form.html', 700, 630);" class="btn btn-primary" visible="false">
            <i class="fa fa-plus"></i> <asp:Literal ID="ltrAddNew" runat="server" Text="新增"></asp:Literal></a>
        <a id="btnCustomPrimary1" runat="server" href="#" class="btn btn-primary" visible="false">自訂主要1</a>
        <a id="btnCustomPrimary2" runat="server" href="#" class="btn btn-primary" visible="false">自訂主要2</a>
    </div>
    <div class="btn-group">
        <a id="btnPreview" runat="server" href="#" class="btn btn-light" visible="false"><i class="fa fa-eye"></i> 預覽</a>
        <a id="btnPreviewZhTw" runat="server" href="#" class="btn btn-light" visible="false"><i class="fa fa-eye"></i> 預覽(中)</a>
        <a id="btnPreviewEn" runat="server" href="#" class="btn btn-light" visible="false"><i class="fa fa-eye"></i> (Eng)</a>
    </div>
</div>
<hr class="content-divider" />

<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Role-Privilege.aspx.cs" Inherits="Role_Privilege" %>
<%@ MasterType TypeName="MasterConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2">作業選項名單</div>
            <div class="col-sm-6">
                <span class="text-primary">
                    <i class="fa fa-user-circle-o"></i>
                    <%= Resources.Lang.Main_RoleTitle %>: 
                    <asp:Literal ID="ltrRoleDisplayName" runat="server"></asp:Literal>
                    (<asp:Literal ID="ltrRoleName" runat="server"></asp:Literal>)
                </span>
            </div>
        </div>
    </div>
    <table class="table table-responsive-sm table-bordered table-sm table-striped table-hover bg-white subitem-list privileges-header">
        <thead>
            <tr>
                <th title="序號" class="seqno">&nbsp;</th>
                <th title="作業選項名稱" class="op-name">作業選項名稱</th>
                <th title="授權作業選項" class="item">作業選項</th>
                <th title="授權擁有的子項目" class="subitem-self">擁有的子項目</th>
                <th title="授權部門的子項目" class="subitem-dept">部門的子項目</th>
                <th title="授權所有的子項目" class="subitem-all">所有的子項目</th>
                <th title="狀態" class="status">狀態</th>
            </tr>
        </thead>
    </table>
    <div class="privileges-body-container">
        <table class="table table-responsive-sm table-bordered table-sm table-striped table-hover bg-white subitem-list privileges-body">
            <tbody>
            <asp:Repeater ID="rptOperations" runat="server" OnItemDataBound="rptOperations_ItemDataBound">
            <ItemTemplate>
                <tr id="OpArea" runat="server" class="op-area">
                    <td class="seqno">
                        <asp:Literal ID="ltrSeqno" runat="server"></asp:Literal>
                    </td>
                    <td class="op-name">
                        <img id="imgOpItem" runat="server" src="BPimages/icon/vectory_mini/basic/028.png" align="absmiddle" style="width:16px; height:16px;" alt="*"/> 
                        <asp:Literal ID="ltrOpItemSubject" runat="server"></asp:Literal>
                    </td>
                    <td class="item">
                        <asp:Literal ID="ltrPvgOfItem" runat="server"></asp:Literal>
                    </td>
                    <td class="subitem-self">
                        <asp:Literal ID="ltrPvgOfSubitemSelf" runat="server"></asp:Literal>
                    </td>
                    <td class="subitem-dept">
                        <asp:Literal ID="ltrPvgOfSubitemDept" runat="server"></asp:Literal>
                    </td>
                    <td class="subitem-all">
                        <asp:Literal ID="ltrPvgOfSubitemAll" runat="server"></asp:Literal>
                    </td>
                    <td class="status">
                        
                    </td>
                </tr>
                <asp:Repeater ID="rptSubOperations" runat="server">
                <ItemTemplate>
                    <tr id="OpArea" runat="server" class="op-area">
                        <td class="seqno">
                            <asp:Literal ID="ltrSeqno" runat="server"></asp:Literal>
                        </td>
                        <td class="op-name">
                            <img id="imgOpItem" runat="server" src="BPimages/icon/vectory_mini/basic/028.png" align="absmiddle" style="width:16px; height:16px;" alt="*"/> 
                            <asp:Literal ID="ltrOpItemSubject" runat="server"></asp:Literal>
                        </td>
                        <td class="item">
                            <asp:Literal ID="ltrPvgOfItem" runat="server"></asp:Literal>
                        </td>
                        <td class="subitem-self">
                            <asp:Literal ID="ltrPvgOfSubitemSelf" runat="server"></asp:Literal>
                        </td>
                        <td class="subitem-dept">
                            <asp:Literal ID="ltrPvgOfSubitemDept" runat="server"></asp:Literal>
                        </td>
                        <td class="subitem-all">
                            <asp:Literal ID="ltrPvgOfSubitemAll" runat="server"></asp:Literal>
                        </td>
                        <td class="status">
                        
                        </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
            </asp:Repeater>
            </tbody>
        </table>
    </div>
    <hr class="content-divider" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActionButtons" Runat="Server">
    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success" ValidationGroup="g" Visible="false"
        OnClick="btnSave_Click"><i class="fa fa-check-circle"></i> <%= Resources.Lang.ConfigForm_btnSave %></asp:LinkButton>
    <a href="#" class="btn btn-light" onclick="closeThisForm(); return false;"><%= Resources.Lang.ConfigForm_btnCancel %></a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


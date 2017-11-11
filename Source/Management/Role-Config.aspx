<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Role-Config.aspx.cs" Inherits="Role_Config" %>
<%@ MasterType TypeName="MasterConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        <%= Resources.Lang.GroupLabel_BasicInfo %>
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;"><span class="required-symbol">排序編號</span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtSortNo" runat="server" style="width:5rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSortNo" runat="server" ControlToValidate="txtSortNo" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="covSortNo" runat="server" ControlToValidate="txtSortNo" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*限整數" Operator="DataTypeCheck" Type="Integer" SetFocusOnError="true" ValidationGroup="g" ></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol">身分名稱</span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtRoleName" runat="server" MaxLength="20" style="width:10rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" ControlToValidate="txtRoleName" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revRoleName" runat="server" ControlToValidate="txtRoleName" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*限英數字和底線" SetFocusOnError="true" ValidationExpression="\w+" ValidationGroup="g"></asp:RegularExpressionValidator>
                    <asp:Literal ID="ltrRoleNameComment" runat="server" Text="(請輸入英數字或底線)"/>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol">顯示用名稱</span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtRoleDisplayName" runat="server" MaxLength="20" style="width:22rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRoleDisplayName" runat="server" ControlToValidate="txtRoleDisplayName" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="CopyPrivilegeArea" runat="server">
                <th><span class="">複製權限自</span></th>
                <td colspan="3">
                    <asp:DropDownList ID="ddlCopyPrivilegeFrom" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActionButtons" Runat="Server">
    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success" ValidationGroup="g" Visible="false"
        OnClick="btnSave_Click"><i class="fa fa-check-circle"></i> <%= Resources.Lang.ConfigForm_btnSave %></asp:LinkButton>
    <a href="#" class="btn btn-light" onclick="closeThisForm(); return false;"><%= Resources.Lang.ConfigForm_btnCancel %></a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


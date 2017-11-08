<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Account-Config.aspx.cs" Inherits="Account_Config" %>
<%@ MasterType TypeName="MasterConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        基本資料
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;"><span class="required-symbol">帳號</span></th>
                <td style="width:35%;">
                    <asp:TextBox ID="txtEmpAccount" runat="server" Width="90%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmpAccount" runat="server" ControlToValidate="txtEmpAccount" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
                <th style="width:15%;"><span class="required-symbol">姓名</span></th>
                <td style="width:35%;">
                    <asp:TextBox ID="txtEmpName" runat="server" Width="90%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmpName" runat="server" ControlToValidate="txtEmpName" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol">密碼</span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtPsw" runat="server" TextMode="Password" style="width:15rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPsw" runat="server" ControlToValidate="txtPsw" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <asp:LinkButton ID="btnGenPsw" runat="server" CssClass="btn btn-sm btn-info"><i class="fa fa-refresh"></i> 重新產生亂數密碼</asp:LinkButton>
                    <asp:Literal ID="hidEmpPasswordOri" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="hidPasswordHashed" runat="server" Text="True" Visible="false"></asp:Literal>
                    <asp:Literal ID="hidDefaultRandomPassword" runat="server" Visible="false"></asp:Literal>
                </td>
            </tr>
            <tr id="PswConfirmArea" runat="server">
                <th><span class="">再次確認密碼</span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtPswConfirm" runat="server" TextMode="Password" style="width:15rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPswConfirm" runat="server" ControlToValidate="txtPswConfirm" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" Enabled="false"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol">Email</span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtEmail" runat="server" style="width:15rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th>備註</th>
                <td colspan="3">
                    <asp:TextBox ID="txtRemarks" runat="server" Width="90%"></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <hr class="content-divider" />
    <div class="sys-subtitle">
        進階資料
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr id="IsAccessDeniedArea" runat="server" class="table-danger">
                <th style="width:15%;">停權</th>
                <td colspan="3">
                    <asp:CheckBox ID="chkIsAccessDenied" runat="server" Text="設定為停權" Visible="false" />
                    <asp:Literal ID="ltrIsAccessDenied" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="DateRangeArea" runat="server">
                <th><span class="required-symbol">有效日期</span></th>
                <td colspan="3">
                    <asp:PlaceHolder ID="DateRangeEditCtrl" runat="server" Visible="false">
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="datepicker" style="width:10rem;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                        &nbsp;~&nbsp;
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="datepicker" style="width:10rem;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </asp:PlaceHolder>
                    <asp:Literal ID="ltrDateRange" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th>部門</th>
                <td colspan="3">
                    <asp:DropDownList ID="ddlDept" runat="server" Visible="false"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDept" runat="server" ControlToValidate="ddlDept" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g"></asp:RequiredFieldValidator>
                    <asp:Literal ID="ltrDept" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th>身分</th>
                <td colspan="3">
                    <asp:DropDownList ID="ddlRoles" runat="server" Visible="false"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvRoles" runat="server" ControlToValidate="ddlRoles" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g"></asp:RequiredFieldValidator>
                    <asp:Literal ID="ltrRoles" runat="server"></asp:Literal>
                </td>
            </tr>
        </tbody>
    </table>
    <hr class="content-divider" />
    <div class="sys-subtitle">
        管理者專用
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;">擁有者帳號</th>
                <td colspan="3">
                    <asp:TextBox ID="txtOwnerAccount" runat="server" Visible="false"></asp:TextBox>
                    <asp:Literal ID="ltrOwnerAccount" runat="server"></asp:Literal>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActionButtons" Runat="Server">
    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success" ValidationGroup="g" Visible="false"
        OnClick="btnSave_Click"><i class="fa fa-check-circle"></i> 儲 存</asp:LinkButton>
    <a href="#" class="btn btn-light" onclick="closeThisForm(); return false;">取 消</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


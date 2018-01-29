<%@ Page Language="C#" MasterPageFile="~/MasterLogin.master" AutoEventWireup="true" CodeFile="Psw-Require.aspx.cs" Inherits="Psw_Require" %>
<%@ MasterType TypeName="MasterLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCardHeader" Runat="Server">
    <%= Resources.Lang.PswRequire_Subtitle %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCardBody" Runat="Server">
    <asp:Panel ID="pnlForm" runat="server" DefaultButton="btnSubmit">
        <div class="form-group form-row">
            <label for="txtAccount" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.Login_AccountTitle %></label>
            <div class="col-md-9">
                <asp:TextBox ID="txtAccount" runat="server" ClientIDMode="Static" CssClass="form-control" 
                    placeholder="Your account" autocomplete="off" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ControlToValidate="txtAccount" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                <div class="text-success">
                    <%= Resources.Lang.ErrMsg_RoleGuestIsNotAllowedToUse %>
                </div>
            </div>
        </div>
        <div class="form-group form-row">
            <label for="txtEmail" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.PswRequire_BoundEmailTitle %> </label>
            <div class="col-md-9">
                <asp:TextBox ID="txtEmail" runat="server" ClientIDMode="Static" CssClass="form-control" 
                    placeholder="Email" autocomplete="off" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" CssClass="text-danger"
                    Display="Dynamic" SetFocusOnError="true" ValidationGroup="g"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group form-row">
            <label for="txtCheckCode" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.Login_CheckCodeTitle %></label>
            <div class="col-md-4">
                <asp:TextBox ID="txtCheckCode" runat="server" ClientIDMode="Static" CssClass="form-control"
                    placeholder="Verification code" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCheckCode" runat="server" ControlToValidate="txtCheckCode" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cuvCheckCode" runat="server" ControlToValidate="txtCheckCode" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*錯誤" SetFocusOnError="true" ValidationGroup="g" OnServerValidate="cuvCheckCode_ServerValidate"></asp:CustomValidator>
            </div>
            <div class="col-md-5">
                <img id="imgCaptcha" src="captcha.ashx" alt="*" style="height:33px;" />
                <a id="btnRefreshCodePic" runat="server" href="#" >Refresh</a>
            </div>
        </div>
        <div class="form-group form-row align-items-end">
            <div class="col-md-3"></div>
            <div class="col-md-3 col-5">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-primary" ValidationGroup="g"
                    OnClick="btnSubmit_Click"><i class="fa fa-check-circle" aria-hidden="true"></i> <%= Resources.Lang.Login_btnSubmit %></asp:LinkButton>
            </div>
            <div class="col-md-6 col-7">
                <a id="btnBackToLogin" runat="server" href="#" class="btn btn-link"><%= Resources.Lang.Login_btnBackToLogin %></a>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
    <script>
        var btnRefreshCodePicId = '<%= btnRefreshCodePic.ClientID %>';

        $("#" + btnRefreshCodePicId).click(function () {
            $("#imgCaptcha").attr("src", "captcha.ashx?" + (new Date()).valueOf());
            return false;
        });
    </script>
</asp:Content>


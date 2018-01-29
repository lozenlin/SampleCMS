<%@ Page Language="C#" MasterPageFile="~/MasterLogin.master" AutoEventWireup="true" CodeFile="Psw-Change.aspx.cs" Inherits="Psw_Change" %>
<%@ MasterType TypeName="MasterLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script>
        function cuvPsw_Validate(obj, args) {
            var isValidPsw = true;

            //內文至少包含下列種類

            //特殊符號
            if (!/[~`!@#$%^&*()\-_=+,\.<>;':"\[\]{}\\|?/]{1,1}/g.test(args.Value)) {
                isValidPsw = false;
            }
            //英文字母大寫
            if (!/[A-Z]+/.test(args.Value)) {
                isValidPsw = false;
            }
            //及小寫
            if (!/[a-z]+/.test(args.Value)) {
                isValidPsw = false;
            }
            //數字
            if (!/[0-9]+/.test(args.Value)) {
                isValidPsw = false;
            }
            //最少12字
            if (args.Value.length < 12) {
                isValidPsw = false;
            }

            args.IsValid = isValidPsw;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCardHeader" Runat="Server">
    <%= Resources.Lang.PswChange_Subtitle %>
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
        <div id="CurrentPswArea" runat="server" class="form-group form-row">
            <label for="txtPassword" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.Login_PasswordTitle %></label>
            <div class="col-md-9">
                <asp:TextBox ID="txtPassword" runat="server" ClientIDMode="Static" TextMode="Password" 
                    CssClass="form-control" placeholder="Password" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group form-row">
            <label for="txtNewPsw" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.PswChange_NewPswTitle %></label>
            <div class="col-md-9">
                <asp:TextBox ID="txtNewPsw" runat="server" ClientIDMode="Static" TextMode="Password" 
                    CssClass="form-control" placeholder="New password" autocomplete="off" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNewPsw" runat="server" ControlToValidate="txtNewPsw" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revNewPsw" runat="server" ControlToValidate="txtNewPsw" CssClass="text-danger" 
                    Display="Dynamic" ErrorMessage="*限6個字元以上的英文字母、阿拉伯數字與特殊符號(!@#...)" SetFocusOnError="true" 
                    ValidationGroup="g" Enabled="false"></asp:RegularExpressionValidator>
                <asp:CustomValidator ID="cuvNewPsw" runat="server" ControlToValidate="txtNewPsw" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*12~15個字元，內容至少包含特殊符號、英文字母大寫及小寫、數字" SetFocusOnError="true"
                    ValidationGroup="g" EnableClientScript="true" ClientValidationFunction="cuvPsw_Validate" OnServerValidate="cuvNewPsw_ServerValidate"
                    Enabled="false"></asp:CustomValidator>
                <div id="NewPswRuleNotice" runat="server" class="text-success"></div>
            </div>
        </div>
        <div class="form-group form-row">
            <label for="txtNewPswConfirm" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.PswChange_NewPswConfirmTitle %></label>
            <div class="col-md-9">
                <asp:TextBox ID="txtNewPswConfirm" runat="server" ClientIDMode="Static" TextMode="Password" 
                    CssClass="form-control" placeholder="New password" autocomplete="off" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNewPswConfirm" runat="server" ControlToValidate="txtNewPswConfirm" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="covNewPswConfirm" runat="server" ControlToValidate="txtNewPswConfirm" ControlToCompare="txtNewPsw" CssClass="text-danger"
                    Display="Dynamic" ErrorMessage="*請輸入相同的文字內容" SetFocusOnError="true" ValidationGroup="g"></asp:CompareValidator>
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
    <asp:Literal ID="hidEmpAccountOfToken" runat="server" Visible="false"></asp:Literal>
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


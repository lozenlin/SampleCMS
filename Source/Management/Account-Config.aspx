<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Account-Config.aspx.cs" Inherits="Account_Config" %>
<%@ MasterType TypeName="MasterConfig" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        <%= Resources.Lang.GroupLabel_BasicInfo %>
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;"><span class="required-symbol"><%= Resources.Lang.Col_EmpAccount %></span></th>
                <td style="width:35%;">
                    <asp:TextBox ID="txtEmpAccount" runat="server" Width="90%" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmpAccount" runat="server" ControlToValidate="txtEmpAccount" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
                <th style="width:15%;"><span class="required-symbol"><%= Resources.Lang.Col_EmpName %></span></th>
                <td style="width:35%;">
                    <asp:TextBox ID="txtEmpName" runat="server" Width="90%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmpName" runat="server" ControlToValidate="txtEmpName" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol"><%= Resources.Lang.Col_Psw %></span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtPsw" runat="server" TextMode="Password" style="width:15rem;" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPsw" runat="server" ControlToValidate="txtPsw" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPsw" runat="server" ControlToValidate="txtPsw" CssClass="text-danger" 
                        Display="Dynamic" ErrorMessage="*限6個字元以上的英文字母、阿拉伯數字與特殊符號(!@#...)" SetFocusOnError="true" 
                        ValidationGroup="g" Enabled="false"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="cuvPsw" runat="server" ControlToValidate="txtPsw" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*12~15個字元，內容至少包含特殊符號、英文字母大寫及小寫、數字" SetFocusOnError="true"
                        ValidationGroup="g" EnableClientScript="true" ClientValidationFunction="cuvPsw_Validate" OnServerValidate="cuvPsw_ServerValidate"
                        Enabled="false"></asp:CustomValidator>
                    <asp:Literal ID="ltrPswComment" runat="server" Text="(不變更密碼，請留空白)"/>
                    <asp:LinkButton ID="btnGenPsw" runat="server" CssClass="btn btn-sm btn-info" OnClick="btnGenPsw_Click">
                        <i class="fa fa-refresh"></i> <%= Resources.Lang.Account_btnGenPsw %></asp:LinkButton>
                    <div id="PswRuleNotice" runat="server" class="text-success"></div>
                    <asp:Literal ID="hidEmpPasswordOri" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="hidPasswordHashed" runat="server" Text="True" Visible="false"></asp:Literal>
                    <asp:Literal ID="hidDefaultRandomPassword" runat="server" Visible="false"></asp:Literal>
                </td>
            </tr>
            <tr id="PswConfirmArea" runat="server">
                <th><span class=""><%= Resources.Lang.Col_PswConfirm %></span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtPswConfirm" runat="server" TextMode="Password" style="width:15rem;" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPswConfirm" runat="server" ControlToValidate="txtPswConfirm" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" Enabled="false"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="covPswConfirm" runat="server" ControlToValidate="txtPswConfirm" ControlToCompare="txtPsw" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*請輸入相同的文字內容" SetFocusOnError="true" ValidationGroup="g"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol">Email</span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtEmail" runat="server" style="width:15rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" CssClass="text-danger"
                        Display="Dynamic" SetFocusOnError="true" ValidationGroup="g"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_Remark %></th>
                <td colspan="3">
                    <asp:TextBox ID="txtRemarks" runat="server" Width="90%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <hr class="content-divider" />
    <div class="sys-subtitle">
        <%= Resources.Lang.GroupLabel_AdvancedInfo %>
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr id="IsAccessDeniedArea" runat="server" class="table-danger">
                <th style="width:15%;"><%= Resources.Lang.Col_AccessDenied %></th>
                <td colspan="3">
                    <asp:CheckBox ID="chkIsAccessDenied" runat="server" Text="設定為停權" Visible="false" />
                    <asp:Literal ID="ltrIsAccessDenied" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr id="DateRangeArea" runat="server">
                <th><span class="required-symbol"><%= Resources.Lang.Col_ValidationDate %></span></th>
                <td colspan="3">
                    <asp:PlaceHolder ID="DateRangeEditCtrl" runat="server" Visible="false">
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="datepicker" style="width:10rem;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="covStartDate" runat="server" ControlToValidate="txtStartDate" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" SetFocusOnError="true" Type="Date" ValidationGroup="g"></asp:CompareValidator>
                        &nbsp;~&nbsp;
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="datepicker" style="width:10rem;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="covEndDate" runat="server" ControlToValidate="txtEndDate" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" SetFocusOnError="true" Type="Date" ValidationGroup="g"></asp:CompareValidator>
                    </asp:PlaceHolder>
                    <asp:Literal ID="ltrDateRange" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_DeptName %></th>
                <td colspan="3">
                    <asp:DropDownList ID="ddlDept" runat="server" Visible="false"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDept" runat="server" ControlToValidate="ddlDept" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g"></asp:RequiredFieldValidator>
                    <asp:Literal ID="ltrDept" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_Role %></th>
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
        <%= Resources.Lang.GroupLabel_AdminOnly %>
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;"><%= Resources.Lang.Col_OwnerAccount %></th>
                <td colspan="3">
                    <asp:TextBox ID="txtOwnerAccount" runat="server" Visible="false"></asp:TextBox>
                    <asp:Literal ID="ltrOwnerAccount" runat="server"></asp:Literal>
                </td>
            </tr>
        </tbody>
    </table>
    <hr class="content-divider" />
    <div class="sys-subtitle">
        <%= Resources.Lang.GroupLabel_ModificationInfo %>
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;"><%= Resources.Lang.Col_Creator %></th>
                <td style="width:35%;">
                    <asp:Literal ID="ltrPostAccount" runat="server"></asp:Literal>
                </td>
                <th style="width:15%;"><%= Resources.Lang.Col_CreateDate %></th>
                <td style="width:35%;">
                    <asp:Literal ID="ltrPostDate" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_Modifier %></th>
                <td>
                    <asp:Literal ID="ltrMdfAccount" runat="server"></asp:Literal>
                </td>
                <th><%= Resources.Lang.Col_ModifyDate %></th>
                <td>
                    <asp:Literal ID="ltrMdfDate" runat="server"></asp:Literal>
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


<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Article-Attach.aspx.cs" Inherits="Article_Attach" %>
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
                <th style="width:15%;"><span class="required-symbol"><%= Resources.Lang.Col_SortNo %></span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtSortNo" runat="server" style="width:5rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSortNo" runat="server" ControlToValidate="txtSortNo" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="covSortNo" runat="server" ControlToValidate="txtSortNo" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*限整數" Operator="DataTypeCheck" Type="Integer" SetFocusOnError="true" ValidationGroup="g" ></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol"><%= Resources.Lang.Col_Subject %></span></th>
                <td colspan="3">
                    <div id="AttSubjectZhTwArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">中:</span>
                        <asp:TextBox ID="txtAttSubjectZhTw" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAttSubjectZhTw" runat="server" ControlToValidate="txtAttSubjectZhTw" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                    <div id="AttSubjectEnArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">Eng:</span>
                        <asp:TextBox ID="txtAttSubjectEn" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAttSubjectEn" runat="server" ControlToValidate="txtAttSubjectEn" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_Language %></th>
                <td colspan="3">
                    <asp:CheckBox ID="chkIsShowInLangZhTw" runat="server" Text="中" Checked="true" />&nbsp;
                    <asp:CheckBox ID="chkIsShowInLangEn" runat="server" Text="Eng" Checked="true" />&nbsp;
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_BrowseFile %></th>
                <td colspan="3">
                    <div class="text-success">(<%= Resources.Lang.Attachment_fuPickedFile_Notice %>)</div>
                    <asp:FileUpload ID="fuPickedFile" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPickedFile" runat="server" ControlToValidate="fuPickedFile" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <div id="ExtLimitationsArea" runat="server" visible="false">
                        <span class="text-info"><%= Resources.Lang.Col_ExtLimitations %>:</span> <asp:Literal ID="ltrExtLimitations" runat="server"></asp:Literal>
                    </div>
                    <div id="CurFileArea" runat="server" visible="false">
                        <span class="text-info"><%= Resources.Lang.Col_CurrentFile %>:</span> <asp:Literal ID="ltrFileSavedName" runat="server"></asp:Literal>
                        &nbsp;
                        <a id="btnDownloadAtt" runat="server" href="#" target="_blank" class="btn btn-sm btn-info">
                            <i class="fa fa-download"></i> <asp:Literal ID="ltrDownloadAtt" runat="server" Text="下載"></asp:Literal></a>
                    </div>
                </td>
            </tr>
            <tr class="table-warning">
                <th><%= Resources.Lang.Col_NotAllowedToDelete %></th>
                <td colspan="3">
                    <asp:CheckBox ID="chkDontDelete" runat="server" Text="隱藏刪除鈕" />
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


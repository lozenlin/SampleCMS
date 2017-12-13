<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Article-Video.aspx.cs" Inherits="Article_Video" %>
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
                    <div id="VidSubjectZhTwArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">中:</span>
                        <asp:TextBox ID="txtVidSubjectZhTw" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvVidSubjectZhTw" runat="server" ControlToValidate="txtVidSubjectZhTw" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                    <div id="VidSubjectEnArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">Eng:</span>
                        <asp:TextBox ID="txtVidSubjectEn" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvVidSubjectEn" runat="server" ControlToValidate="txtVidSubjectEn" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_YoutubeVideoUrl %></th>
                <td colspan="3">
                    <asp:TextBox ID="txtVidLinkUrl" runat="server" MaxLength="2048" style="width:90%;"></asp:TextBox>
                    <div class="my-2">
                        <asp:LinkButton ID="btnGetYoutubeId" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnGetYoutubeId_Click">
                            <i class="fa fa-arrow-down"></i> <%= Resources.Lang.ArticleVideo_btnGetYoutubeId %></asp:LinkButton>
                    </div>
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol"><%= Resources.Lang.Col_YoutubeVideoId %></span></th>
                <td colspan="3">
                    <asp:TextBox ID="txtSourceVideoId" runat="server" MaxLength="100" style="width:10rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSourceVideoId" runat="server" ControlToValidate="txtSourceVideoId" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
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
                <th><%= Resources.Lang.Col_VideoDesc %></th>
                <td colspan="3">
                    <div id="VidDescZhTwArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">中:</span>
                        <asp:TextBox ID="txtVidDescZhTw" runat="server" TextMode="MultiLine" Rows="3" CssClass="align-text-top" style="width:90%;"></asp:TextBox>
                    </div>
                    <div id="VidDescEnArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">Eng:</span>
                        <asp:TextBox ID="txtVidDescEn" runat="server" TextMode="MultiLine" Rows="3" CssClass="align-text-top" style="width:90%;"></asp:TextBox>
                    </div>
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


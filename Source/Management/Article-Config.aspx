﻿<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Article-Config.aspx.cs" Inherits="Article_Config" %>
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
                <th><span class="required-symbol"><%= Resources.Lang.Col_ValidationDate %></span></th>
                <td colspan="3">
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
                </td>
            </tr>
            <tr>
                <th><span class="required-symbol"><%= Resources.Lang.Col_Subject %></span></th>
                <td colspan="3">
                    <div class="config-textbox-lang">
                        <span class="lang-label">中:</span>
                        <asp:TextBox ID="txtArticleSubjectZhTw" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvArticleSubjectZhTw" runat="server" ControlToValidate="txtArticleSubjectZhTw" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                    <div class="config-textbox-lang">
                        <span class="lang-label">Eng:</span>
                        <asp:TextBox ID="txtArticleSubjectEn" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvArticleSubjectEn" runat="server" ControlToValidate="txtArticleSubjectEn" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <th>橫幅區圖檔名</th>
                <td colspan="3">
                    <asp:TextBox ID="txtBannerPicFileName" runat="server" ClientIDMode="Static" MaxLength="255" style="width:25rem;"></asp:TextBox>
                    <a id="btnBrowseBannerPic" href="#" class="btn btn-sm btn-secondary" title="<%= Resources.Lang.Operation_btnBrowseImage_Hint %>">
                        <i class="fa fa-folder-open"></i> <%= Resources.Lang.Operation_btnBrowseImage %></a>
                    <div class="text-success">
                        (請將圖檔放至網站目錄[ (語言編號) ]中)
                    </div>
                    <div class="mt-2">
                        <%= Resources.Lang.Operation_lblPreview %> -
                        中: <img id="imgBannerPicZhTw" src="images/default.png" alt="*" style="width:32px; height:32px;" />
                        Eng: <img id="imgBannerPicEn" src="images/default.png" alt="*" style="width:32px; height:32px;" />
                    </div>
                </td>
            </tr>
            <tr>
                <th>語言版本</th>
                <td colspan="3">
                    <asp:CheckBox ID="IsShowInLangZhTw" runat="server" Text="中" Checked="true" />&nbsp;
                    <asp:CheckBox ID="IsShowInLangEn" runat="server" Text="Eng" Checked="true" />&nbsp;
                </td>
            </tr>
        </tbody>
    </table>
    <hr class="content-divider" />
    <div class="sys-subtitle">
        頁面設定
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;"><span class="">版面模式</span></th>
                <td colspan="3">
                    <asp:RadioButtonList ID="rdolLayoutMode" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                        <asp:ListItem Text="全版　" Value="1" Selected="True" />
                        <asp:ListItem Text="兩欄式　" Value="2" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>開啟方式</th>
                <td colspan="3">
                    <div class="config-textbox-lang">
                        <asp:RadioButtonList ID="rdolShowType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ClientIDMode="Static">
                            <asp:ListItem Text="呈現網頁　" Value="1" Selected="True" />
                            <asp:ListItem Text="跳轉下層　" Value="2" />
                            <asp:ListItem Text="超連結　" Value="3" />
                            <asp:ListItem Text="使用控制項　" Value="4" />
                        </asp:RadioButtonList>
                    </div>
                    <div id="ShowTypeDetailArea" runat="server" class="config-textbox-lang ShowTypeDetailArea" style="display:none;">
                        <div class="text-success">
                            (填入連結網址，客製化網頁程式請用 ~/ 開頭， e.g., ~/News.aspx，或填入控制項名稱)
                        </div>
                        <asp:TextBox ID="txtLinkUrl" runat="server" MaxLength="2048" Width="90%"></asp:TextBox>
                        <div class="mt-2 IsNewWindow">
                            <asp:CheckBox ID="chkIsNewWindow" runat="server" Text="開啟在新視窗" ClientIDMode="Static" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <th>隱藏文章</th>
                <td colspan="3">
                    <asp:CheckBox ID="chkIsHideSelf" runat="server" Text="隱藏本篇" />
                    <span class="text-success">(若有選單，在父層的選單中不出現本篇文章)</span>
                    <br />
                    <asp:CheckBox ID="chkIsHideChild" runat="server" Text="隱藏子項目" />
                    <span class="text-success">(若有選單，在選單中不列出所有子項目)</span>
                </td>
            </tr>
            <tr class="table-warning">
                <th>禁止刪除</th>
                <td colspan="3">
                    <asp:CheckBox ID="chkDontDelete" runat="server" Text="隱藏刪除鈕" />
                </td>
            </tr>
        </tbody>
    </table>
    <hr class="content-divider" />
    <div class="sys-subtitle">
        內文編輯
    </div>
    <ul class="nav nav-tabs">
        <li class="nav-item">
            <a class="nav-link active" data-toggle="tab" href="#pnlZhTw">正體中文</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#pnlEn">English</a>
        </li>
    </ul>
    <div class="tab-content bg-white p-3 border border-top-0">
        <div class="tab-pane fade show active" id="pnlZhTw">
            <asp:TextBox ID="txtCkeContentZhTw" runat="server" TextMode="MultiLine" Rows="5" Width="90%" ClientIDMode="Static"></asp:TextBox>
        </div>
        <div class="tab-pane fade" id="pnlEn">
            <asp:TextBox ID="txtCkeContentEn" runat="server" TextMode="MultiLine" Rows="5" Width="90%" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActionButtons" Runat="Server">
    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success" ValidationGroup="g" Visible="false"
        OnClick="btnSave_Click"><i class="fa fa-check-circle"></i> <%= Resources.Lang.ConfigForm_btnSave %></asp:LinkButton>
    <a href="#" class="btn btn-light" onclick="closeThisForm(); return false;"><%= Resources.Lang.ConfigForm_btnCancel %></a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
    <script src="ckeditor/ckeditor.js"></script>
    <script>
        // show type
        function setShowTypeDetailArea(showTypeId) {
            if (showTypeId == 3 || showTypeId == 4) {
                $(".ShowTypeDetailArea").show();
            } else {
                $(".ShowTypeDetailArea").hide();
            }

            if (showTypeId == 4) {
                $(".IsNewWindow").hide();
            } else {
                $(".IsNewWindow").show();
            }
        }

        $("#rdolShowType>input:radio").change(function () {
            setShowTypeDetailArea(this.value);
        });

        var $selectedShowType = $("#rdolShowType>input[type='radio']:checked");

        if ($selectedShowType.length > 0) {
            setShowTypeDetailArea($selectedShowType.val());
        }

        // article context
        CKEDITOR.replace("txtCkeContentZhTw", {
            allowedContent: true,
            width: "90%"
        });

        CKEDITOR.replace("txtCkeContentEn", {
            allowedContent: true,
            width: "90%"
        });
    </script>
</asp:Content>


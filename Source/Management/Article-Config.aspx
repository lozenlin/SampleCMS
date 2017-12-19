<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Article-Config.aspx.cs" ValidateRequest="false" Inherits="Article_Config" %>
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
                    <div id="ArticleSubjectZhTwArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">中:</span>
                        <asp:TextBox ID="txtArticleSubjectZhTw" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvArticleSubjectZhTw" runat="server" ControlToValidate="txtArticleSubjectZhTw" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                    <div id="ArticleSubjectEnArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">Eng:</span>
                        <asp:TextBox ID="txtArticleSubjectEn" runat="server" MaxLength="200" style="width:90%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvArticleSubjectEn" runat="server" ControlToValidate="txtArticleSubjectEn" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                    <div class="config-textbox-lang pt-1">
                        <span class="lang-label"></span>
                        <asp:CheckBox ID="chkSubjectAtBannerArea" runat="server" Text="標題位置在橫幅區" Checked="true" />
                        <span class="text-success">(取消則位置在內容區)</span>
                    </div>
                </td>
            </tr>
            <tr>
                <th>副標題</th>
                <td colspan="3">
                    <div id="SubtitleZhTwArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">中:</span>
                        <asp:TextBox ID="txtSubtitleZhTw" runat="server" MaxLength="500" style="width:90%;"></asp:TextBox>
                    </div>
                    <div id="SubtitleEnArea" runat="server" class="config-textbox-lang">
                        <span class="lang-label">Eng:</span>
                        <asp:TextBox ID="txtSubtitleEn" runat="server" MaxLength="500" style="width:90%;"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_BannerPicFileName %></th>
                <td colspan="3">
                    <asp:TextBox ID="txtBannerPicFileName" runat="server" ClientIDMode="Static" MaxLength="255" style="width:25rem;"></asp:TextBox>
                    <a id="btnBrowseBannerPic" href="#" class="btn btn-sm btn-secondary" title="<%= Resources.Lang.Operation_btnBrowseImage_Hint %>">
                        <i class="fa fa-folder-open"></i> <%= Resources.Lang.Operation_btnBrowseImage %></a>
                    <div class="text-success">
                        (<%= Resources.Lang.Article_txtBannerPicFileName_Notice %>)
                    </div>
                    <div class="mt-2">
                        <%= Resources.Lang.Operation_lblPreview %> -
                        <asp:PlaceHolder ID="PreviewBannerZhTwArea" runat="server">
                            中: <img id="imgBannerPicZhTw" src="images/default.png" alt="*" style="min-height:32px; max-height:96px;" />
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PreviewBannerEnArea" runat="server">
                            Eng: <img id="imgBannerPicEn" src="images/default.png" alt="*" style="min-height:32px; max-height:96px;" />
                        </asp:PlaceHolder>
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
                <th><%= Resources.Lang.Col_ArticleAlias %></th>
                <td colspan="3">
                    <asp:TextBox ID="txtArticleAlias" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width:15%;"><span class="required-symbol">發佈者</span></th>
                <td style="width:35%;">
                    <div id="PublisherNameAreaZhTw" runat="server" class="config-textbox-lang">
                        <span class="lang-label">中:</span>
                        <asp:TextBox ID="txtPublisherNameZhTw" runat="server" MaxLength="50" style="width:15rem;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPublisherNameZhTw" runat="server" ControlToValidate="txtPublisherNameZhTw" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                    <div id="PublisherNameAreaEn" runat="server" class="config-textbox-lang">
                        <span class="lang-label">Eng:</span>
                        <asp:TextBox ID="txtPublisherNameEn" runat="server" MaxLength="50" style="width:15rem;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPublisherNameEn" runat="server" ControlToValidate="txtPublisherNameEn" CssClass="text-danger"
                            Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    </div>
                </td>
                <th style="width:15%;"><span class="required-symbol">發佈日期</span></th>
                <td style="width:35%;">
                    <asp:TextBox ID="txtPublishDate" runat="server" CssClass="datepicker" style="width:10rem;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPublishDate" runat="server" ControlToValidate="txtPublishDate" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="covPublishDate" runat="server" ControlToValidate="txtPublishDate" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" SetFocusOnError="true" Type="Date" ValidationGroup="g"></asp:CompareValidator>
                </td>
            </tr>
        </tbody>
    </table>
    <hr class="content-divider" />
    <div class="sys-subtitle">
        <%= Resources.Lang.GroupLabel_PageSettings %>
    </div>
    <table class="table table-responsive-sm table-bordered table-striped table-hover table-sm bg-white config-list">
        <tbody>
            <tr>
                <th style="width:15%;"><span class=""><%= Resources.Lang.Col_LayoutMode %></span></th>
                <td colspan="3">
                    <asp:RadioButtonList ID="rdolLayoutMode" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_PageShowType %></th>
                <td colspan="3">
                    <div class="config-textbox-lang">
                        <asp:RadioButtonList ID="rdolShowType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ClientIDMode="Static">
                        </asp:RadioButtonList>
                    </div>
                    <div id="LinkUrlArea" class="config-textbox-lang" style="display:none;">
                        <div class="text-success">
                            (<%= Resources.Lang.Article_txtLinkUrl_Notice %>)
                        </div>
                        <asp:TextBox ID="txtLinkUrl" runat="server" MaxLength="2048" Width="90%"></asp:TextBox>
                        <div class="mt-2 IsNewWindow">
                            <asp:CheckBox ID="chkIsNewWindow" runat="server" Text="開啟在新視窗" ClientIDMode="Static" />
                        </div>
                    </div>
                    <div id="ControlNameArea" class="config-textbox-lang" style="display:none;">
                        <div class="text-success">
                            (<%= Resources.Lang.Article_txtControlName_Notice %>)
                        </div>
                        <span class="ctrl-label"><%= Resources.Lang.Article_lblControlName %>:</span>
                        <asp:TextBox ID="txtControlName" runat="server" MaxLength="100" Width="70%"></asp:TextBox><br />
                        <span class="ctrl-label"><%= Resources.Lang.Article_lblSubItemControlName %>:</span>
                        <asp:TextBox ID="txtSubItemControlName" runat="server" MaxLength="100" Width="70%"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <th><%= Resources.Lang.Col_HideArticle %></th>
                <td colspan="3">
                    <asp:CheckBox ID="chkIsHideSelf" runat="server" Text="隱藏本篇" />
                    <span class="text-success">(<%= Resources.Lang.Article_chkIsHideSelf_Notice %>)</span>
                    <br />
                    <asp:CheckBox ID="chkIsHideChild" runat="server" Text="隱藏子項目" />
                    <span class="text-success">(<%= Resources.Lang.Article_chkIsHideChild_Notice %>)</span>
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
    <div id="ContextEditor" class="sys-subtitle">
        <%= Resources.Lang.GroupLabel_ContextEditor %>
    </div>
    <ul class="nav nav-tabs context-tabs">
        <li id="ContextTabZhTwArea" runat="server" class="nav-item">
            <a class="nav-link active" data-toggle="tab" href="#pnlZhTw">正體中文</a>
        </li>
        <li id="ContextTabEnArea" runat="server" class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#pnlEn">English</a>
        </li>
    </ul>
    <div class="tab-content bg-white p-3 border border-top-0">
        <asp:PlaceHolder ID="ContextPnlZhTwArea" runat="server">
            <div class="tab-pane fade show active" id="pnlZhTw">
                <asp:TextBox ID="txtCkeContextZhTw" runat="server" TextMode="MultiLine" Rows="5" Width="90%" ClientIDMode="Static"></asp:TextBox>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="ContextPnlEnArea" runat="server">
            <div class="tab-pane fade" id="pnlEn">
                <asp:TextBox ID="txtCkeContextEn" runat="server" TextMode="MultiLine" Rows="5" Width="90%" ClientIDMode="Static"></asp:TextBox>
            </div>
        </asp:PlaceHolder>
    </div>
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
    <script src="ckeditor/ckeditor.js?t=20171204"></script>
    <script>
        //取得過濾掉多餘路徑的檔名
        function GetSievedFileName(fileUrl) {
            // exclude lang directory
            var iLangDir = fileUrl.indexOf("/");

            if (iLangDir == 1)
                fileUrl = fileUrl.substr(2, fileUrl.length - 2);

            return fileUrl;
        }

        function BannerSelected(data) {
            if (data == undefined || data == null) {
                return;
            }

            data = GetSievedFileName(data);

            $("#txtBannerPicFileName")
                .val(data)
                .blur();
        }

        function BrowseImages() {
            popWinOut("angularFileManager/Index.aspx?listtype=images&fnSelected=BannerSelected", 1000, 768);
        }

        // browse banner
        $("#btnBrowseBannerPic").click(function () {
            BrowseImages();
            return false;
        });

        function PreviewBannerPicFile(fileName) {
            var fileNameZhTw = "images/default.png";
            var fileNameEn = "images/default.png";

            if (fileName != "") {
                fileNameZhTw = "images/1/" + fileName;
                fileNameEn = "images/2/" + fileName;
            }

            $("#imgBannerPicZhTw").attr("src", fileNameZhTw);
            $("#imgBannerPicEn").attr("src", fileNameEn);
        }

        $("#txtBannerPicFileName").blur(function () {
            PreviewBannerPicFile(this.value);
        }).blur();

        // show type
        function setShowTypeDetailArea(showTypeId) {
            var $LinkUrlArea = $("#LinkUrlArea");
            var $ControlNameArea = $("#ControlNameArea");

            $LinkUrlArea.hide();
            $ControlNameArea.hide();

            if (showTypeId == 3) {
                $LinkUrlArea.slideDown("fast");
            } else if (showTypeId == 4) {
                $ControlNameArea.slideDown("fast");
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
        if ($("#txtCkeContextZhTw").length > 0) {
            CKEDITOR.replace("txtCkeContextZhTw", {
                allowedContent: true,
                width: "90%"
            });
        }

        if ($("#txtCkeContextEn").length > 0) {
            CKEDITOR.replace("txtCkeContextEn", {
                allowedContent: true,
                width: "90%"
            });
        }

        $(".context-tabs a:first").click();
    </script>
</asp:Content>


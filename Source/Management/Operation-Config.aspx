<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Operation-Config.aspx.cs" Inherits="Operation_Config" %>
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
                <th style="width:15%;"><span class="required-symbol">標題</span></th>
                <td style="width:35%;">
                    <asp:TextBox ID="txtOpSubject" runat="server" MaxLength="100" style="width:90%;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOpSubject" runat="server" ControlToValidate="txtOpSubject" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
                <th style="width:15%;"><span class="required-symbol">英文標題</span></th>
                <td style="width:35%;">
                    <asp:TextBox ID="txtEnglishSubject" runat="server" MaxLength="100" style="width:90%;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEnglishSubject" runat="server" ControlToValidate="txtEnglishSubject" CssClass="text-danger"
                        Display="Dynamic" ErrorMessage="*必填" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th>圖示</th>
                <td colspan="3">
                    <asp:TextBox ID="txtIconImageFile" runat="server" ClientIDMode="Static" MaxLength="255" style="width:25rem;"></asp:TextBox>
                    <a id="btnBrowseImage" href="#" class="btn btn-sm btn-secondary" title="瀏覽網站目錄"><i class="fa fa-folder-open"></i> 瀏覽</a>
                    <div class="text-success">
                        (請將圖示檔案放至網站目錄[ BPimages/icon ]中)
                    </div>
                    預覽: <img id="imgIconImageFile" src="BPimages/icon/data.gif" alt="*" style="width:32px; height:32px;" />
                </td>
            </tr>
            <tr>
                <th>超連結網址</th>
                <td colspan="3">
                    <asp:TextBox ID="txtLinkUrl" runat="server" Width="90%"></asp:TextBox>
                    <div class="mt-2">
                        <asp:CheckBox ID="chkIsNewWindow" runat="server" Text="開啟在新視窗" />
                    </div>
                    <div class="text-success">
                        ．{dns} = 帶入前台完整網址 e.g., http://SampleCMS.dev.com <br />
                        ．{backend_url} = 帶入後台完整網址 <br />　e.g., https://SampleCMS.dev.com/Management
                    </div>
                </td>
            </tr>
            <tr>
                <th>隱藏此項目</th>
                <td colspan="3" class="table-danger">
                    <asp:CheckBox ID="chkIsHideSelf" runat="server" Text="隱藏此項目" />
                </td>
            </tr>
            <tr>
                <th>單元共用元件</th>
                <td colspan="3">
                    <asp:TextBox ID="txtCommonClass" runat="server" ClientIDMode="Static" style="width:25rem;"></asp:TextBox>
                    <div class="mt-2">
                        <asp:DropDownList ID="ddlCommonClasses" runat="server" ClientIDMode="Static" style="width:30rem;"></asp:DropDownList>
                        <a id="btnBrowseClass" href="#" class="btn btn-sm btn-secondary" title="瀏覽並選擇元件"><i class="fa fa-bars"></i> 瀏覽並選擇元件</a>
                    </div>
                    <div class="text-success">
                        ．有區分大小寫！<br />
                        ．此設定為系統用來判斷權限的資料來源，若不確定時請勿更改！<br />
                        ．「網站架構管理」請設定為「ArticleCommonOfBackend」！<br />
                        ．超連結網址為 Article-Node.aspx 時，請設定為空白！
                    </div>
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
    <script>
        function PreviewIconImageFile(fileName, idImg) {
            if (fileName == "") {
                fileName = "data.gif";
            }

            $("#imgIconImageFile").attr("src", "BPimages/icon/" + fileName);
        }

        function BrowseOpIcons() {
            popWinOut("angularFileManager/Index.aspx", 990, 768);
        }

        $("#btnBrowseImage").click(function () {
            BrowseOpIcons();
            return false;
        });

        $("#ddlCommonClasses").hide();

        $("#btnBrowseClass").click(function () {
            $("#ddlCommonClasses").toggle("normal");
            return false;
        });

        $("#ddlCommonClasses").change(function () {
            if (this.selectedIndex == 0)
                return;

            var commonClass = $("#ddlCommonClasses").val();
            $("#txtCommonClass").val(commonClass);
            $(this).fadeOut("normal");
        });

        $("#txtIconImageFile").blur(function () {
            PreviewIconImageFile(this.value);
        }).blur();
    </script>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Article-Node.aspx.cs" Inherits="Article_Node" %>
<%@ MasterType TypeName="MasterMain" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <ul class="sys-info list-inline">
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">
                <%= Resources.Lang.Col_ValidationDate %>:
                <asp:Literal ID="ltrValidDateRange" runat="server"></asp:Literal></span>
        </li>
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">
                <%= Resources.Lang.Col_Modifier %>: 
                <asp:Literal ID="ltrMdfName" runat="server"></asp:Literal></span>
        </li>
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">
                <%= Resources.Lang.Col_ModifyDate %>: 
                <asp:Literal ID="ltrMdfDate" runat="server"></asp:Literal></span>
        </li>
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">
                <%= Resources.Lang.Col_PageShowType %>: 
                <asp:Literal ID="ltrShowTypeName" runat="server"></asp:Literal>
                <a id="btnShowTypeLinkUrl" runat="server" href="#" target="_blank" visible="false">超連結</a></span>
        </li>
    </ul>
    <div class="sys-subtitle">
        <%= Resources.Lang.GroupLabel_Subitems %>
    </div>
    <div class="sys-subtitle">
        <%= Resources.Lang.SearchPanel_Title %>
        <a id="btnExpandSearchPanel" href="#" class="btn btn-sm btn-block btn-light border" 
            style="display:none;" title='<%= Resources.Lang.SearchPanel_btnExpand_Hint %>'><i class="fa fa-expand"></i> <%= Resources.Lang.SearchPanel_btnExpand %></a>
        <div class="card bg-light search-panel">
            <div class="card-body sys-conditions pr-md-5">
                <div class="form-group form-row">
                    <label for='<%= txtKw.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblSubjectKw %></label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtKw" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-row">
                    <div class="col-md-2"></div>
                    <div class="col-md-10">
                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-secondary" OnClick="btnSearch_Click">
                            <i class="fa fa-search"></i> <%= Resources.Lang.SearchPanel_btnSearch %></asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-link btn-sm" OnClick="btnClear_Click">
                            <%= Resources.Lang.SearchPanel_btnClear %></asp:LinkButton>
                        <a id="btnCollapseSearchPanel" href="#" class="btn btn-sm btn-light border-secondary float-right mt-1"
                            title='<%= Resources.Lang.SearchPanel_btnCollapse_Hint %>'><i class="fa fa-compress"></i> <%= Resources.Lang.SearchPanel_btnCollapse %></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <table class="table table-responsive-md table-bordered table-sm table-striped table-hover bg-white subitem-list">
        <thead>
            <tr>
                <th title="<%= Resources.Lang.Col_Seqno_Hint %>" style="width:3%">&nbsp;</th>
                <th title="<%= Resources.Lang.Col_AdjustSortNo_Hint %>" style="width:6%" colspan="2"><%= Resources.Lang.Col_AdjustSortNo %></th>
                <th title="<%= Resources.Lang.Col_Subject_Hint %>">
                    <asp:LinkButton ID="btnSortArticleSubject" runat="server" CommandArgument="ArticleSubject" Text="標題" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortArticleSubject" runat="server" Visible="false" Text="標題"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Language_Hint %>" style="width:14%"><%= Resources.Lang.Col_Language %></th>
                <th title="<%= Resources.Lang.Col_SortNo_Hint %>" style="width:10%">
                    <asp:LinkButton ID="btnSortSortNo" runat="server" CommandArgument="SortNo" Text="排序編號" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortSortNo" runat="server" Visible="false" Text="排序編號"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Status_Hint %>" style="width:4%"><%= Resources.Lang.Col_Status %></th>
                <th title="<%= Resources.Lang.Col_ValidationDate_Hint %>" style="width:13%">
                    <asp:LinkButton ID="btnSortStartDate" runat="server" CommandArgument="StartDate" Text="上架日期" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortStartDate" runat="server" Visible="false" Text="上架日期"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_DeptName_Hint %>" style="width:9%">
                    <asp:LinkButton ID="btnSortPostDeptName" runat="server" CommandArgument="PostDeptName" Text="部門" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortPostDeptName" runat="server" Visible="false" Text="部門"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Management_Hint %>" style="width:20%"><%= Resources.Lang.Col_Management %></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptSubitems" runat="server" OnItemDataBound="rptSubitems_ItemDataBound" OnItemCommand="rptSubitems_ItemCommand">
            <ItemTemplate>
                <tr id="ItemArea" runat="server">
                    <td>
                        <%# EvalToSafeStr("RowNum") %>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnMoveDown" runat="server" ToolTip="往下" CommandName="MoveDown" CommandArgument='<%# EvalToSafeStr("ArticleId") %>'>
                            <span class="fa fa-arrow-down fa-lg text-secondary"></span></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnMoveUp" runat="server" ToolTip="往上" CommandName="MoveUp" CommandArgument='<%# EvalToSafeStr("ArticleId") %>'>
                            <span class="fa fa-arrow-up fa-lg text-info"></span></asp:LinkButton>
                    </td>
                    <td>
                        <a id="btnItem" runat="server" href="#"></a>
                    </td>
                    <td>
                        <span id="ctlIsShowInLangZhTw" runat="server" class="badge badge-light text-secondary">中</span>
                        <span id="ctlIsShowInLangEn" runat="server" class="badge badge-light text-secondary">Eng</span>
                        <%--
                            on: badge badge-light text-info border border-info
                            off: badge badge-light text-secondary
                            --%>
                    </td>
                    <td>
                        <%# EvalToSafeStr("SortNo") %>
                    </td>
                    <td>
                        <span id="ctlArticleState" runat="server" class="fa fa-thumbs-up fa-lg text-success" title="online"></span>
                        <%--
                            online: fa fa-thumbs-up fa-lg text-success
                            offline: fa fa-ban fa-lg text-danger
                            on schedule: fa fa-hourglass-start fa-lg text-info
                            --%>
                    </td>
                    <td>
                        <span class="small"><asp:Literal ID="ltrValidDateRange" runat="server"></asp:Literal></span>
                    </td>
                    <td>
                        <span class="small"><%# EvalToSafeStr("PostDeptName") %></span>
                    </td>
                    <td>
                        <a id="btnEdit" runat="server" href="#" class="btn btn-sm btn-success">
                            <i class="fa fa-pencil-square-o"></i> <asp:Literal ID="ltrEdit" runat="server" Text="修改"></asp:Literal>
                        </a>
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Del">
                            <i class="fa fa-trash-o"></i> 刪除
                        </asp:LinkButton>
                        <span id="ctlDontDelete" runat="server" visible="false"
                            class="badge badge-warning text-white" title="禁止刪除"><i class="fa fa-lock fa-lg"></i></span>
                    </td>
                </tr>
            </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <uc1:wucDataPager ID="ucDataPager" runat="server" />

    <hr class="content-divider" />
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2"><%= Resources.Lang.GroupLabel_Attachments %></div>
            <div class="col-sm-2">
                <a id="btnUploadAttachFile" runat="server" href="#" class="btn btn-sm btn-secondary">
                    <i class="fa fa-upload"></i> <asp:Literal ID="ltrUploadAttachFile" runat="server" Text="上傳"></asp:Literal></a>
            </div>
        </div>
    </div>
    <table class="table table-responsive-md table-bordered table-sm table-striped table-hover bg-white subitem-list">
        <thead>
            <tr>
                <th title="<%= Resources.Lang.Col_Seqno_Hint %>" style="width:3%">&nbsp;</th>
                <th title="<%= Resources.Lang.Col_AdjustSortNo_Hint %>" style="width:6%" colspan="2"><%= Resources.Lang.Col_AdjustSortNo %></th>
                <th title="<%= Resources.Lang.Col_Subject_Hint %>"><%= Resources.Lang.Col_Subject %></th>
                <th title="<%= Resources.Lang.Col_FileName_Hint %>" style="width:20%"><%= Resources.Lang.Col_FileName %></th>
                <th title="<%= Resources.Lang.Col_Language_Hint %>" style="width:10%"><%= Resources.Lang.Col_Language %></th>
                <th title="<%= Resources.Lang.Col_MdfAccount_Hint %>" style="width:10%"><%= Resources.Lang.Col_MdfAccount %></th>
                <th title="<%= Resources.Lang.Col_MdfDate_Hint %>" style="width:12%"><%= Resources.Lang.Col_MdfDate %></th>
                <th title="<%= Resources.Lang.Col_FileType_Hint %>" style="width:6%"><%= Resources.Lang.Col_FileType %></th>
                <th title="<%= Resources.Lang.Col_Management_Hint %>" style="width:20%"><%= Resources.Lang.Col_Management %></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptAttachFiles" runat="server" OnItemDataBound="rptAttachFiles_ItemDataBound" OnItemCommand="rptAttachFiles_ItemCommand">
            <ItemTemplate>
                <tr>
                    <td>
                        <%# EvalToSafeStr("RowNum") %>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnMoveDown" runat="server" ToolTip="往下" CommandName="MoveDown" CommandArgument='<%# EvalToSafeStr("AttId") %>'>
                            <span class="fa fa-arrow-down fa-lg text-secondary"></span></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnMoveUp" runat="server" ToolTip="往上" CommandName="MoveUp" CommandArgument='<%# EvalToSafeStr("AttId") %>'>
                            <span class="fa fa-arrow-up fa-lg text-info"></span></asp:LinkButton>
                    </td>
                    <td>
                        <%# EvalToSafeStr("AttSubject") %>
                    </td>
                    <td>
                        <%# EvalToSafeStr("FileSavedName") %>
                    </td>
                    <td>
                        <span id="ctlIsShowInLangZhTw" runat="server" class="badge badge-light text-secondary">中</span>
                        <span id="ctlIsShowInLangEn" runat="server" class="badge badge-light text-secondary">Eng</span>
                    </td>
                    <td>
                        <asp:Literal ID="ltrMdfAccount" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <span class="small"><asp:Literal ID="ltrMdfDate" runat="server"></asp:Literal></span>
                    </td>
                    <td>
                        <img id="imgExt" runat="server" src="BPimages/FileExtIcon/generic.png" alt="*" style="width:24px;" />
                    </td>
                    <td>
                        <a id="btnEdit" runat="server" href="#" class="btn btn-sm btn-success">
                            <i class="fa fa-pencil-square-o"></i> <asp:Literal ID="ltrEdit" runat="server" Text="修改"></asp:Literal>
                        </a>
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Del">
                            <i class="fa fa-trash-o"></i> 刪除
                        </asp:LinkButton>
                        <span id="ctlDontDelete" runat="server" visible="false"
                            class="badge badge-warning text-white" title="禁止刪除"><i class="fa fa-lock fa-lg"></i></span>
                        <a id="btnDownloadAtt" runat="server" href="#" target="_blank" class="btn btn-sm btn-info">
                            <i class="fa fa-download"></i> <asp:Literal ID="ltrDownloadAtt" runat="server" Text="下載"></asp:Literal></a>
                    </td>
                </tr>
            </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <asp:Literal ID="hidParentId" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="hidArticleLevelNo" runat="server" Visible="false"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Article-Node.aspx.cs" Inherits="Article_Node" %>
<%@ MasterType TypeName="MasterMain" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <link href="/Management/Common/slick/slick.css" rel="stylesheet" />
    <link href="/Management/Common/slick/slick-theme.css" rel="stylesheet" />
    <style type="text/css">
        .contextArea img {
            max-width:100%;
        }
        /*
        @media (max-width: 767px) {
        }
        */
    </style>
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
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2"><%= Resources.Lang.GroupLabel_Subitems %></div>
            <div class="col-sm-4">
                <span class="article-content-setting IsListAreaShowInFrontStageArea">
                    <span class="title"><%= Resources.Lang.Article_lblShowInFrontStage %>:</span>
                    <span id="ctlIsListAreaShowInFrontStageStatus" runat="server" class="status text-muted">OFF</span>
                    <asp:HiddenField ID="hidIsListAreaShowInFrontStage" runat="server" ClientIDMode="Static" Value="False" />
                    <a id="btnSwitchIsListAreaShowInFrontStage" runat="server" href="#" class="btn btn-sm btn-light border-secondary"><%= Resources.Lang.Article_btnSwitchAreaShowInFrontStage %></a>
                    <span class="tran-result tran-ok text-success"><i class="fa fa-check"></i> <%= Resources.Lang.TranResult_saved %></span>
                    <span class="tran-result tran-loading text-info"><i class="fa fa-spinner fa-pulse"></i> <%= Resources.Lang.TranResult_sending %></span>
                    <span class="tran-result tran-failed text-danger"><i class="fa fa-times"></i> <%= Resources.Lang.TranResult_failed %></span>
                </span>
            </div>
            <div class="col-sm-6 mt-sm-0 mt-1">
                <span class="article-content-setting SortFieldOfFrontStageArea">
                    <span class="title"><%= Resources.Lang.Article_lblSortFieldOfFrontStage %>:</span>
                    <asp:DropDownList ID="ddlSortFieldOfFrontStage" runat="server" ClientIDMode="Static"></asp:DropDownList>
                    <asp:DropDownList ID="ddlIsSortDescOfFrontStage" runat="server" ClientIDMode="Static"></asp:DropDownList>
                    <asp:Literal ID="ltrSortInfoOfFrontStage" runat="server" Visible="false"></asp:Literal>
                    <span class="tran-result tran-ok text-success"><i class="fa fa-check"></i> <%= Resources.Lang.TranResult_saved %></span>
                    <span class="tran-result tran-loading text-info"><i class="fa fa-spinner fa-pulse"></i> <%= Resources.Lang.TranResult_sending %></span>
                    <span class="tran-result tran-failed text-danger"><i class="fa fa-times"></i> <%= Resources.Lang.TranResult_failed %></span>
                </span>
            </div>
        </div>
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
            <div class="col-sm-4">
                <span class="article-content-setting IsAttAreaShowInFrontStageArea">
                    <span class="title"><%= Resources.Lang.Article_lblShowInFrontStage %>:</span>
                    <span id="ctlIsAttAreaShowInFrontStageStatus" runat="server" class="status text-muted">OFF</span>
                    <asp:HiddenField ID="hidIsAttAreaShowInFrontStage" runat="server" ClientIDMode="Static" Value="False" />
                    <a id="btnSwitchIsAttAreaShowInFrontStage" runat="server" href="#" class="btn btn-sm btn-light border-secondary"><%= Resources.Lang.Article_btnSwitchAreaShowInFrontStage %></a>
                    <span class="tran-result tran-ok text-success"><i class="fa fa-check"></i> <%= Resources.Lang.TranResult_saved %></span>
                    <span class="tran-result tran-loading text-info"><i class="fa fa-spinner fa-pulse"></i> <%= Resources.Lang.TranResult_sending %></span>
                    <span class="tran-result tran-failed text-danger"><i class="fa fa-times"></i> <%= Resources.Lang.TranResult_failed %></span>
                </span>
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
                <th title="<%= Resources.Lang.Col_SortNo_Hint %>" style="width:10%"><%= Resources.Lang.Col_SortNo %></th>
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
                        <%# EvalToSafeStr("SortNo") %>
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
    
    <hr class="content-divider" />
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2"><%= Resources.Lang.GroupLabel_ArticlePictures %></div>
            <div class="col-sm-2">
                <a id="btnUploadPicture" runat="server" href="#" class="btn btn-sm btn-secondary">
                    <i class="fa fa-upload"></i> <asp:Literal ID="ltrUploadPicture" runat="server" Text="上傳"></asp:Literal></a>
            </div>
            <div class="col-sm-4">
                <span class="article-content-setting IsPicAreaShowInFrontStageArea">
                    <span class="title"><%= Resources.Lang.Article_lblShowInFrontStage %>:</span>
                    <span id="ctlIsPicAreaShowInFrontStageStatus" runat="server" class="status text-muted">OFF</span>
                    <asp:HiddenField ID="hidIsPicAreaShowInFrontStage" runat="server" ClientIDMode="Static" Value="False" />
                    <a id="btnSwitchIsPicAreaShowInFrontStage" runat="server" href="#" class="btn btn-sm btn-light border-secondary"><%= Resources.Lang.Article_btnSwitchAreaShowInFrontStage %></a>
                    <span class="tran-result tran-ok text-success"><i class="fa fa-check"></i> <%= Resources.Lang.TranResult_saved %></span>
                    <span class="tran-result tran-loading text-info"><i class="fa fa-spinner fa-pulse"></i> <%= Resources.Lang.TranResult_sending %></span>
                    <span class="tran-result tran-failed text-danger"><i class="fa fa-times"></i> <%= Resources.Lang.TranResult_failed %></span>
                </span>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row pic-carousel">
            <asp:Repeater ID="rptArticlePictures" runat="server" OnItemDataBound="rptArticlePictures_ItemDataBound" OnItemCommand="rptArticlePictures_ItemCommand">
            <ItemTemplate>
                <div class="col-md-3 col-sm-4">
                    <div class="card PicThumbnail">
                        <a id="btnView" runat="server" href="#" target="_blank">
                            <img id="imgPic" runat="server" class="card-img-top" alt="*" />
                            <div class="PicLinkText">
                                <i class="fa fa-share-square-o text-secondary"></i> <%= Resources.Lang.Main_btnClickToOpenInNewWin %></div>
                        </a>
                        <div class="text-center">
                            <span id="ctlIsShowInLangZhTw" runat="server" class="badge badge-light text-secondary">中</span>
                            <span id="ctlIsShowInLangEn" runat="server" class="badge badge-light text-secondary">Eng</span>
                        </div>
                        <div class="card-body">
                            <h5 class="card-title"><%# EvalToSafeStr("PicSubject") %></h5>
                            <p class="mb-1">
                                <a id="btnEdit" runat="server" href="#" class="btn btn-sm btn-success">
                                    <i class="fa fa-pencil-square-o"></i> <asp:Literal ID="ltrEdit" runat="server" Text="修改"></asp:Literal>
                                </a>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Del">
                                    <i class="fa fa-trash-o"></i> 刪除
                                </asp:LinkButton>
                            </p>
                            <div class="text-warning">
                                <%= Resources.Lang.Col_SortNo %>: <%# EvalToSafeStr("SortNo") %>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
        
    <hr class="content-divider" />
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2"><%= Resources.Lang.GroupLabel_ArticleVideos %></div>
            <div class="col-sm-2">
                <a id="btnAddVideo" runat="server" href="#" class="btn btn-sm btn-secondary">
                    <i class="fa fa-plus"></i> <asp:Literal ID="ltrAddVideo" runat="server" Text="新增"></asp:Literal></a>
            </div>
            <div class="col-sm-4">
                <span class="article-content-setting IsVideoAreaShowInFrontStageArea">
                    <span class="title"><%= Resources.Lang.Article_lblShowInFrontStage %>:</span>
                    <span id="ctlIsVideoAreaShowInFrontStageStatus" runat="server" class="status text-muted">OFF</span>
                    <asp:HiddenField ID="hidIsVideoAreaShowInFrontStage" runat="server" ClientIDMode="Static" Value="False" />
                    <a id="btnSwitchIsVideoAreaShowInFrontStage" runat="server" href="#" class="btn btn-sm btn-light border-secondary"><%= Resources.Lang.Article_btnSwitchAreaShowInFrontStage %></a>
                    <span class="tran-result tran-ok text-success"><i class="fa fa-check"></i> <%= Resources.Lang.TranResult_saved %></span>
                    <span class="tran-result tran-loading text-info"><i class="fa fa-spinner fa-pulse"></i> <%= Resources.Lang.TranResult_sending %></span>
                    <span class="tran-result tran-failed text-danger"><i class="fa fa-times"></i> <%= Resources.Lang.TranResult_failed %></span>
                </span>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row vid-carousel">
            <asp:Repeater ID="rptArticleVideos" runat="server" OnItemDataBound="rptArticleVideos_ItemDataBound" OnItemCommand="rptArticleVideos_ItemCommand">
            <ItemTemplate>
                <div class="col-md-4 col-sm-6">
                    <div class="card PicThumbnail">
                        <a id="btnView" runat="server" href="#" target="_blank">
                            <img id="imgPic" runat="server" class="card-img-top" alt="*" />
                            <div class="PicLinkText">
                                <i class="fa fa-share-square-o text-secondary"></i> <%= Resources.Lang.Main_btnClickToOpenInNewWin %></div>
                        </a>
                        <div class="text-center">
                            <span id="ctlIsShowInLangZhTw" runat="server" class="badge badge-light text-secondary">中</span>
                            <span id="ctlIsShowInLangEn" runat="server" class="badge badge-light text-secondary">Eng</span>
                        </div>
                        <div class="card-body">
                            <h5 class="card-title"><%# EvalToSafeStr("VidSubject") %></h5>
                            <p class="mb-1">
                                <span class="small text-info descText"><%# EvalToSafeStr("VidDesc") %></span>
                                <a id="btnEdit" runat="server" href="#" class="btn btn-sm btn-success">
                                    <i class="fa fa-pencil-square-o"></i> <asp:Literal ID="ltrEdit" runat="server" Text="修改"></asp:Literal>
                                </a>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Del">
                                    <i class="fa fa-trash-o"></i> 刪除
                                </asp:LinkButton>
                            </p>
                            <div class="text-warning">
                                <%= Resources.Lang.Col_SortNo %>: <%# EvalToSafeStr("SortNo") %>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <hr class="content-divider" />
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2"><%= Resources.Lang.GroupLabel_Context %></div>
            <div class="col-sm-2">
                <a id="btnEditContext" runat="server" href="#" class="btn btn-sm btn-secondary">
                    <i class="fa fa-pencil-square-o"></i> <%= Resources.Lang.Article_btnEditContext %></a>
            </div>
        </div>
    </div>
    <ul class="nav nav-tabs context-tabs">
        <li id="ContextTabZhTwArea" runat="server" class="nav-item">
            <a class="nav-link active" data-toggle="tab" href="#pnlZhTw">正體中文</a>
        </li>
        <li id="ContextTabEnArea" runat="server" class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#pnlEn">English</a>
        </li>
    </ul>
    <div class="tab-content bg-white p-3 border border-top-0 contextArea">
        <asp:PlaceHolder ID="ContextPnlZhTwArea" runat="server">
            <div class="tab-pane fade show active" id="pnlZhTw">
                <asp:Literal ID="ltrContextZhTw" runat="server"></asp:Literal>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="ContextPnlEnArea" runat="server">
            <div class="tab-pane fade" id="pnlEn">
                <asp:Literal ID="ltrContextEn" runat="server"></asp:Literal>
            </div>
        </asp:PlaceHolder>
    </div>

    <asp:Literal ID="hidParentId" runat="server" Visible="false"></asp:Literal>
    <asp:Literal ID="hidArticleLevelNo" runat="server" Visible="false"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
    <script src="Common/slick/slick.min.js"></script>
    <script src="Common/js/dao.js"></script>
    <script>
        langNo = '<%= c.seLangNoOfBackend %>';
        serviceUrl = "jsonService.ashx?l=" + langNo;
        var authToken = '<%= GetClientAjaxAuthToken() %>';

        $(function () {
            var winWidth = window.innerWidth;

            // pic-carousel
            var usePicCarousel = true;

            if (winWidth > 767 && $(".pic-carousel>div").length <= 4
                || winWidth > 575 && $(".pic-carousel>div").length <= 3
                || winWidth <= 575 && $(".pic-carousel>div").length == 0) {
                usePicCarousel = false;
            }
            
            if (usePicCarousel) {
                $(".pic-carousel").slick({
                    dots: true,
                    infinite: true,
                    slidesToShow: 4,
                    slidesToScroll: 4,
                    responsive: [
                        {
                            breakpoint: 767,
                            settings: {
                                slidesToShow: 3,
                                slidesToScroll: 3,
                            }
                        },
                        {
                            breakpoint: 575,
                            settings: {
                                slidesToShow: 1,
                                slidesToScroll: 1,
                            }
                        }
                    ]
                });
            }

            // vid-carousel
            var useVidCarousel = true;

            if (winWidth > 767 && $(".vid-carousel>div").length <= 3
                || winWidth > 575 && $(".vid-carousel>div").length <= 2
                || winWidth <= 575 && $(".vid-carousel>div").length == 0) {
                useVidCarousel = false;
            }

            if (useVidCarousel) {
                $(".vid-carousel").slick({
                    dots: true,
                    infinite: true,
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    responsive: [
                        {
                            breakpoint: 767,
                            settings: {
                                slidesToShow: 2,
                                slidesToScroll: 2,
                            }
                        },
                        {
                            breakpoint: 575,
                            settings: {
                                slidesToShow: 1,
                                slidesToScroll: 1,
                            }
                        }
                    ]
                });
            }
        });

        // article-content-setting
        var artId = '<%= c.qsArtId %>';

        // article-content-setting / subitems-sort
        var $ddlSortFieldOfFrontStage = $("#ddlSortFieldOfFrontStage");
        var $ddlIsSortDescOfFrontStage = $("#ddlIsSortDescOfFrontStage");

        function updateSortFieldOfFrontStage() {
            var sortField = $ddlSortFieldOfFrontStage.val();
            var isSortDesc = $ddlIsSortDescOfFrontStage.val();
            var rootCssClass = "SortFieldOfFrontStageArea";
            setupTranResult(rootCssClass, "loading");
            dao.UpdateArticleSortFieldOfFrontStage(artId, sortField, isSortDesc, authToken, function (cr) {
                if (cr.b) {
                    $ddlSortFieldOfFrontStage.val(cr.o.sortField);
                    $ddlIsSortDescOfFrontStage.val(cr.o.isSortDesc);

                    setupTranResult(rootCssClass, "ok");
                } else {
                    setupTranResult(rootCssClass, "failed");
                }
            });
        }

        $ddlSortFieldOfFrontStage.change(function () {
            updateSortFieldOfFrontStage();
        });

        $ddlIsSortDescOfFrontStage.change(function () {
            updateSortFieldOfFrontStage();
        });

        function setupTranResult(rootCssClass, state) {
            $("." + rootCssClass + " .tran-result").hide();
            $("." + rootCssClass + " .tran-" + state).show();
        }

        function setupStatusOfAreaShowInFrontStage(hidCtlId, rootCssClass, isShow) {
            if (isShow) {
                $("#" + hidCtlId).val("True");
                $("." + rootCssClass + " .status")
                    .removeClass("text-muted")
                    .addClass("text-success")
                    .html("ON");
            } else {
                $("#" + hidCtlId).val("False");
                $("." + rootCssClass + " .status")
                    .removeClass("text-success")
                    .addClass("text-muted")
                    .html("OFF");
            }
        }

        // article-content-setting / subitems
        $("#btnSwitchIsListAreaShowInFrontStage").click(function () {
            var isShow = !($("#hidIsListAreaShowInFrontStage").val() == "True");
            var rootCssClass = "IsListAreaShowInFrontStageArea";
            setupTranResult(rootCssClass, "loading");
            dao.UpdateArticleIsAreaShowInFrontStage(artId, "ListArea", isShow, authToken, function (cr) {
                if (cr.b) {
                    setupStatusOfAreaShowInFrontStage("hidIsListAreaShowInFrontStage", rootCssClass, isShow);
                    setupTranResult(rootCssClass, "ok");
                } else {
                    setupTranResult(rootCssClass, "failed");
                }
            });

            return false;
        });

        // article-content-setting / attachments
        $("#btnSwitchIsAttAreaShowInFrontStage").click(function () {
            var isShow = !($("#hidIsAttAreaShowInFrontStage").val() == "True");
            var rootCssClass = "IsAttAreaShowInFrontStageArea";
            setupTranResult(rootCssClass, "loading");
            dao.UpdateArticleIsAreaShowInFrontStage(artId, "AttArea", isShow, authToken, function (cr) {
                if (cr.b) {
                    setupStatusOfAreaShowInFrontStage("hidIsAttAreaShowInFrontStage", rootCssClass, isShow);
                    setupTranResult(rootCssClass, "ok");
                } else {
                    setupTranResult(rootCssClass, "failed");
                }
            });

            return false;
        });

        // article-content-setting / pictures
        $("#btnSwitchIsPicAreaShowInFrontStage").click(function () {
            var isShow = !($("#hidIsPicAreaShowInFrontStage").val() == "True");
            var rootCssClass = "IsPicAreaShowInFrontStageArea";
            setupTranResult(rootCssClass, "loading");
            dao.UpdateArticleIsAreaShowInFrontStage(artId, "PicArea", isShow, authToken, function (cr) {
                if (cr.b) {
                    setupStatusOfAreaShowInFrontStage("hidIsPicAreaShowInFrontStage", rootCssClass, isShow);
                    setupTranResult(rootCssClass, "ok");
                } else {
                    setupTranResult(rootCssClass, "failed");
                }
            });

            return false;
        });

        // article-content-setting / videos
        $("#btnSwitchIsVideoAreaShowInFrontStage").click(function () {
            var isShow = !($("#hidIsVideoAreaShowInFrontStage").val() == "True");
            var rootCssClass = "IsVideoAreaShowInFrontStageArea";
            setupTranResult(rootCssClass, "loading");
            dao.UpdateArticleIsAreaShowInFrontStage(artId, "VideoArea", isShow, authToken, function (cr) {
                if (cr.b) {
                    setupStatusOfAreaShowInFrontStage("hidIsVideoAreaShowInFrontStage", rootCssClass, isShow);
                    setupTranResult(rootCssClass, "ok");
                } else {
                    setupTranResult(rootCssClass, "failed");
                }
            });

            return false;
        });

        // context area
        $(".context-tabs a:first").click();
    </script>
</asp:Content>


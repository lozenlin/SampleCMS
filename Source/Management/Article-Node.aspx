﻿<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Article-Node.aspx.cs" Inherits="Article_Node" %>
<%@ MasterType TypeName="MasterMain" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <ul class="sys-info list-inline">
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">有效日期: 2010-07-26 ~ 2020-07-26</span>
        </li>
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">更新人員: 管理者</span>
        </li>
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">更新日期: 2014-12-29</span>
        </li>
        <li class="list-inline-item">
            <span class="badge badge-secondary font-weight-normal">頁面屬性: <a href="#">超連結</a></span>
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
                    <label for='<%= txtKw.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblKw %></label>
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
                <th title="語言" style="width:14%">語言</th>
                <th title="<%= Resources.Lang.Col_SortNo_Hint %>" style="width:10%">
                    <asp:LinkButton ID="btnSortSortNo" runat="server" CommandArgument="SortNo" Text="排序編號" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortSortNo" runat="server" Visible="false" Text="排序編號"></asp:Literal>
                </th>
                <th title="狀態" style="width:4%">狀態</th>
                <th title="<%= Resources.Lang.Col_ValidationDate_Hint %>" style="width:13%">
                    <asp:LinkButton ID="btnSortStartDate" runat="server" CommandArgument="StartDate" Text="上架日期" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortStartDate" runat="server" Visible="false" Text="上架日期"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_DeptName_Hint %>" style="width:9%">
                    <asp:LinkButton ID="btnSortDeptName" runat="server" CommandArgument="DeptName" Text="部門" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortDeptName" runat="server" Visible="false" Text="部門"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Management_Hint %>" style="width:20%"><%= Resources.Lang.Col_Management_Hint %></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>1</td>
                <td>
                    <a href="#" title="往下">
                        <span class="fa fa-arrow-down fa-lg text-secondary"></span></a>
                </td>
                <td>
                    <a href="#" title="往上">
                        <span class="fa fa-arrow-up fa-lg text-info"></span></a>
                </td>
                <td>
                    <a href="#">子項目</a>
                </td>
                <td>
                    <span class="badge badge-light text-info border border-info">中</span>
                    <span class="badge badge-light text-info border border-info">Eng</span>
                    <span class="badge badge-light text-secondary">日</span>
                </td>
                <td>10</td>
                <td>
                    <span class="fa fa-thumbs-up fa-lg text-success" title="online"></span>
                </td>
                <td>
                    <a href="#" class="small">2017-10-18</a>
                </td>
                <td>
                    <span class="small">部門名稱</span>
                </td>
                <td>
                    <a href="#" class="btn btn-sm btn-success"><i class="fa fa-pencil-square-o"></i> 修改</a>
                    <a href="#" class="btn btn-sm btn-danger"><i class="fa fa-trash-o"></i> 刪除</a>
                </td>
            </tr>
        </tbody>
    </table>

    <uc1:wucDataPager ID="ucDataPager" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>

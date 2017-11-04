﻿<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Account-List.aspx.cs" Inherits="Account_List" %>
<%@ MasterType TypeName="MasterMain" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        搜尋條件
        <a id="btnExpandSearchPanel" href="#" class="btn btn-sm btn-block btn-light border" 
            style="display:none;"><i class="fa fa-expand"></i> 變更搜尋條件</a>
        <div class="card bg-light search-panel">
            <div class="card-body sys-conditions pr-md-5">
                <div class="form-group form-row">
                    <label for="txtKw" class="col-md-2 col-form-label text-md-right">帳號狀態</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlListMode" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <label class="col-md-2 col-form-label text-md-right">部門</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-row">
                    <label class="col-md-2 col-form-label text-md-right">關鍵字</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtKw" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group form-row">
                    <div class="col-md-2"></div>
                    <div class="col-md-10">
                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-secondary" OnClick="btnSearch_Click">
                            <i class="fa fa-search"></i> 查詢</asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-link btn-sm" OnClick="btnClear_Click">
                            清除條件</asp:LinkButton>
                        <a id="btnCollapseSearchPanel" runat="server" href="#" 
                            class="btn btn-sm btn-light border-secondary float-right mt-1"><i class="fa fa-compress"></i> 收合</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <table class="table table-responsive-md table-bordered table-sm table-striped table-hover bg-white subitem-list">
        <thead>
            <tr>
                <th title="序號" style="width:3%">&nbsp;</th>
                <th title="部門" style="width:6%">
                    <asp:LinkButton ID="btnSortDeptName" runat="server" CommandArgument="DeptName" Text="部門" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortDeptName" runat="server" Visible="false" Text="部門"></asp:Literal>
                </th>
                <th title="身分" style="width:13%">
                    <asp:LinkButton ID="btnSortRoleSortNo" runat="server" CommandArgument="RoleSortNo" Text="身分" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortRoleSortNo" runat="server" Visible="false" Text="身分"></asp:Literal>
                </th>
                <th title="姓名">
                    <asp:LinkButton ID="btnSortEmpName" runat="server" CommandArgument="EmpName" Text="姓名" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortEmpName" runat="server" Visible="false" Text="姓名"></asp:Literal>
                    <%--<a href="#">姓名<span class="fa fa-chevron-up text-dark"></span></a>--%>
                </th>
                <th title="帳號" style="width:9%">
                    <asp:LinkButton ID="btnSortEmpAccount" runat="server" CommandArgument="EmpAccount" Text="帳號" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortEmpAccount" runat="server" Visible="false" Text="帳號"></asp:Literal>
                </th>
                <th title="停權" style="width:6%">停權</th>
                <th title="狀態" style="width:6%">狀態</th>
                <th title="上架日期" style="width:13%">
                    <asp:LinkButton ID="btnSortStartDate" runat="server" CommandArgument="StartDate" Text="上架日期" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortStartDate" runat="server" Visible="false" Text="上架日期"></asp:Literal>
                </th>
                <th title="備註" style="width:6%;">備註</th>
                <th title="管理功能" style="width:20%">管理功能</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>1</td>
                <td>
                    <span class="small">部門名稱</span>
                </td>
                <td>
                    身分
                </td>
                <td>
                    姓名
                </td>
                <td>
                    帳號
                </td>
                <td>
                    <span class="badge badge-danger" title="已停權"><i class="fa fa-ban"></i></span>
                </td>
                <td>
                    <span class="fa fa-thumbs-up fa-lg text-success" title="online"></span>
                </td>
                <td>
                    <a href="#" class="small">2017-10-18</a>
                </td>
                <td>
                    <span class="badge badge-info"><i class="fa fa-comment"></i></span>
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


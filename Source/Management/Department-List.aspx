<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Department-List.aspx.cs" Inherits="Department_List" %>
<%@ MasterType TypeName="MasterMain" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
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
                <th title='<%= Resources.Lang.Col_DeptName_Hint %>'>
                    <asp:LinkButton ID="btnSortDeptName" runat="server" CommandArgument="DeptName" Text="部門名稱" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortDeptName" runat="server" Visible="false" Text="部門名稱"></asp:Literal>
                </th>
                <th title='<%= Resources.Lang.Col_SortNo_Hint %>' style="width:9%;">
                    <asp:LinkButton ID="btnSortSortNo" runat="server" CommandArgument="SortNo" Text="排序編號" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortSortNo" runat="server" Visible="false" Text="排序編號"></asp:Literal>
                </th>
                <th title='<%= Resources.Lang.Col_EmpTotal_Hint %>' style="width:9%;">
                    <asp:LinkButton ID="btnSortEmpTotal" runat="server" CommandArgument="EmpTotal" Text="帳號總數" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortEmpTotal" runat="server" Visible="false" Text="帳號總數"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Management_Hint %>" style="width:26%"><%= Resources.Lang.Col_Management %></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptDepartments" runat="server" OnItemDataBound="rptDepartments_ItemDataBound" OnItemCommand="rptDepartments_ItemCommand">
            <ItemTemplate>
                <tr>
                    <td><%# EvalToSafeStr("RowNum") %></td>
                    <td>
                        <%# EvalToSafeStr("DeptName") %>
                    </td>
                    <td>
                        <%# EvalToSafeStr("SortNo") %>
                    </td>
                    <td>
                        <%# EvalToSafeStr("EmpTotal") %>
                    </td>
                    <td>
                        <a id="btnEdit" runat="server" href="#" class="btn btn-sm btn-success">
                            <i class="fa fa-pencil-square-o"></i> <asp:Literal ID="ltrEdit" runat="server" Text="修改"></asp:Literal>
                        </a>
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Del">
                            <i class="fa fa-trash-o"></i> 刪除
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <uc1:wucDataPager ID="ucDataPager" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


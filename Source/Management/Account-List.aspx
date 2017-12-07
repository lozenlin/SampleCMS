<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Account-List.aspx.cs" Inherits="Account_List" %>
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
                    <label for='<%= ddlEmpRange.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblEmpRange %></label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlEmpRange" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <label for='<%= ddlDept.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblDept %></label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
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
                <th title="<%= Resources.Lang.Col_DeptName_Hint %>" style="width:6%">
                    <asp:LinkButton ID="btnSortDeptName" runat="server" CommandArgument="DeptName" Text="部門" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortDeptName" runat="server" Visible="false" Text="部門"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Role_Hint %>" style="width:11%">
                    <asp:LinkButton ID="btnSortRoleSortNo" runat="server" CommandArgument="RoleSortNo" Text="身分" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortRoleSortNo" runat="server" Visible="false" Text="身分"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_EmpName_Hint %>">
                    <asp:LinkButton ID="btnSortEmpName" runat="server" CommandArgument="EmpName" Text="姓名" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortEmpName" runat="server" Visible="false" Text="姓名"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_EmpAccount_Hint %>" style="width:9%">
                    <asp:LinkButton ID="btnSortEmpAccount" runat="server" CommandArgument="EmpAccount" Text="帳號" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortEmpAccount" runat="server" Visible="false" Text="帳號"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_AccessDenied_Hint %>" style="width:6%"><%= Resources.Lang.Col_AccessDenied_Hint %></th>
                <th title="<%= Resources.Lang.Col_Status_Hint %>" style="width:6%"><%= Resources.Lang.Col_Status_Hint %></th>
                <th title="<%= Resources.Lang.Col_ValidationDate_Hint %>" style="width:13%">
                    <asp:LinkButton ID="btnSortStartDate" runat="server" CommandArgument="StartDate" Text="上架日期" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortStartDate" runat="server" Visible="false" Text="上架日期"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Remark_Hint %>" style="width:6%;"><%= Resources.Lang.Col_Remark_Hint %></th>
                <th title="<%= Resources.Lang.Col_OwnerName_Hint %>" style="width:9%">
                    <asp:LinkButton ID="btnSortOwnerName" runat="server" CommandArgument="OwnerName" Text="擁有者" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortOwnerName" runat="server" Visible="false" Text="擁有者"></asp:Literal>
                </th>
                <th title="<%= Resources.Lang.Col_Management_Hint %>" style="width:20%"><%= Resources.Lang.Col_Management %></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptAccounts" runat="server" OnItemDataBound="rptAccounts_ItemDataBound" OnItemCommand="rptAccounts_ItemCommand">
            <ItemTemplate>
                <tr id="EmpArea" runat="server">
                    <td><%# EvalToSafeStr("RowNum") %></td>
                    <td>
                        <span class="small"><%# EvalToSafeStr("DeptName") %></span>
                    </td>
                    <td>
                        <span id="ctlRoleDisplayName" runat="server"></span>
                    </td>
                    <td>
                        <%# EvalToSafeStr("EmpName") %>
                    </td>
                    <td>
                        <%# EvalToSafeStr("EmpAccount") %>
                    </td>
                    <td>
                        <span id="ctlIsAccessDenied" runat="server" class="badge badge-danger" title="已停權" visible="false"><i class="fa fa-ban"></i></span>
                    </td>
                    <td>
                        <span id="ctlAccountState" runat="server" class="fa fa-thumbs-up fa-lg text-success" title="online"></span>
                    </td>
                    <td>
                        <span class="small"><asp:Literal ID="ltrValidDateRange" runat="server"></asp:Literal></span>
                    </td>
                    <td>
                        <span id="ctlRemarks" runat="server" class="badge badge-info emp-comment" visible="false"><i class="fa fa-comment"></i></span>
                    </td>
                    <td>
                        <%# EvalToSafeStr("OwnerName") %>
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
    <script>
        $(".emp-comment").tooltip();
    </script>
</asp:Content>


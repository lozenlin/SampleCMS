<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Back-End-Log.aspx.cs" Inherits="Back_End_Log" %>
<%@ MasterType TypeName="MasterMain" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        .ui-widget-header .ui-icon {
            background-image: url("Common/jquery-ui-1.12.1/themes/south-street/images/ui-icons_847e71_256x240.png")
        }

        .page-item.active .page-link {
            z-index:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        <%= Resources.Lang.SearchPanel_Title %>
        <a id="btnExpandSearchPanel" href="#" class="btn btn-sm btn-block btn-light border" 
            style="display:none;" title='<%= Resources.Lang.SearchPanel_btnExpand_Hint %>'><i class="fa fa-expand"></i> <%= Resources.Lang.SearchPanel_btnExpand %></a>
        <div class="card bg-light search-panel">
            <div class="card-body sys-conditions pr-md-5">
                <div class="form-group form-row">
                    <label for='<%= txtStartDate.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblOpDateStart %></label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control d-inline-block datepicker" Width="100px"></asp:TextBox>
                        <asp:DropDownList ID="ddlHourStart" runat="server" CssClass="form-control d-inline-block" Width="70px"></asp:DropDownList> :
                        <asp:DropDownList ID="ddlMinStart" runat="server" CssClass="form-control d-inline-block" Width="70px"></asp:DropDownList> :
                        <asp:DropDownList ID="ddlSecStart" runat="server" CssClass="form-control d-inline-block" Width="70px"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-row">
                    <label for='<%= txtEndDate.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblOpDateEnd %></label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control d-inline-block datepicker" Width="100px"></asp:TextBox>
                        <asp:DropDownList ID="ddlHourEnd" runat="server" CssClass="form-control d-inline-block" Width="70px"></asp:DropDownList> :
                        <asp:DropDownList ID="ddlMinEnd" runat="server" CssClass="form-control d-inline-block" Width="70px"></asp:DropDownList> :
                        <asp:DropDownList ID="ddlSecEnd" runat="server" CssClass="form-control d-inline-block" Width="70px"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-row">
                    <label for='<%= txtAccount.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblAccount %></label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                        <label class="form-check form-check-label">
                            <asp:CheckBox ID="chkIsAccKw" runat="server" CssClass="form-check-input" /> <%= Resources.Lang.SearchPanel_lblIsKeyword %>
                        </label>
                    </div>
                    <label for='<%= txtIP.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblIP %></label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIP" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        <label class="form-check form-check-label">
                            <asp:CheckBox ID="chkIsIpHeadKw" runat="server" CssClass="form-check-input" /> <%= Resources.Lang.SearchPanel_lblIsKeywordOfHead %>
                        </label>
                    </div>
                </div>
                <div class="form-group form-row">
                    <label for='<%= txtDescKw.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblKeywordOfLog %></label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDescKw" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                    </div>
                    <label for='<%= ddlRangeMode.ClientID %>' class="col-md-2 col-form-label text-md-right"><%= Resources.Lang.SearchPanel_lblRangeMode %></label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRangeMode" runat="server" CssClass="form-control"></asp:DropDownList>
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
                <th title='<%= Resources.Lang.Col_OpDate_Hint %>' style="width:20%;">
                    <asp:LinkButton ID="btnSortOpDate" runat="server" CommandArgument="OpDate" Text="日期時間" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortOpDate" runat="server" Visible="false" Text="日期時間"></asp:Literal>
                </th>
                <th title='<%= Resources.Lang.Col_IP_Hint %>' style="width:15%;">
                    <asp:LinkButton ID="btnSortIP" runat="server" CommandArgument="IP" Text="IP" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortIP" runat="server" Visible="false" Text="IP"></asp:Literal>
                </th>
                <th title='<%= Resources.Lang.Col_EmpNameOfLog_Hint %>' style="width:15%;">
                    <asp:LinkButton ID="btnSortEmpName" runat="server" CommandArgument="EmpName" Text="姓名(帳號)" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortEmpName" runat="server" Visible="false" Text="姓名(帳號)"></asp:Literal>
                </th>
                <th title='<%= Resources.Lang.Col_LogRecord_Hint %>'>
                    <asp:LinkButton ID="btnSortDescription" runat="server" CommandArgument="Description" Text="操作紀錄" OnClick="btnSort_Click"></asp:LinkButton>
                    <asp:Literal ID="hidSortDescription" runat="server" Visible="false" Text="操作紀錄"></asp:Literal>
                </th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptLogs" runat="server" OnItemDataBound="rptLogs_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td>
                        <%# EvalToSafeStr("RowNum") %>
                    </td>
                    <td>
                        <%# EvalToSafeStr("OpDate", "{0:yyyy-MM-dd HH:mm:ss}") %>
                    </td>
                    <td>
                        <%# EvalToSafeStr("IP") %>
                    </td>
                    <td>
                        <asp:Literal ID="ltrEmpName" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="ltrDescription" runat="server"></asp:Literal>
                    </td>
                </tr>
            </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <uc1:wucDataPager ID="ucDataPager" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
    <script src="Common/jquery-ui-1.12.1.Datepicker/jquery-ui.min.js"></script>
    <asp:Literal ID="ltrDatepickerJsTW" runat="server" Text="<script src='Common/jquery-ui-1.12.1/i18n/datepicker-zh-TW.js'></script>"></asp:Literal>
    <script>
        //設定所有日期選擇小工具
        $(".datepicker").datepicker({
            showOn: 'both',
            buttonImage: 'BPimages/icon/calendar.png',
            buttonImageOnly: true,
            dateFormat: 'yy-mm-dd',
            changeYear: true,
            changeMonth: false,
            showButtonPanel: true
        });
    </script>
</asp:Content>


﻿<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Operation-Node.aspx.cs" Inherits="Operation_Node" %>
<%@ MasterType TypeName="MasterMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <asp:PlaceHolder ID="SubitemArea" runat="server" Visible="false">
        <div class="sys-subtitle">
            子項目
        </div>
        <table class="table table-responsive-md table-bordered table-sm table-striped table-hover bg-white subitem-list">
            <thead>
                <tr>
                    <th title="<%= Resources.Lang.Col_Seqno_Hint %>" style="width:3%">&nbsp;</th>
                    <th title="調整順序" style="width:6%" colspan="2">順序</th>
                    <th title='標題'>
                        <asp:LinkButton ID="btnSortSubject" runat="server" CommandArgument="Subject" Text="標題" OnClick="btnSort_Click"></asp:LinkButton>
                        <asp:Literal ID="hidSortSubject" runat="server" Visible="false" Text="標題"></asp:Literal>
                    </th>
                    <th title='在新視窗' style="width:9%">
                        <asp:LinkButton ID="btnSortIsNewWindow" runat="server" CommandArgument="IsNewWindow" Text="在新視窗" OnClick="btnSort_Click"></asp:LinkButton>
                        <asp:Literal ID="hidSortIsNewWindow" runat="server" Visible="false" Text="在新視窗"></asp:Literal>
                    </th>
                    <th title='權限元件' style="width:9%;">
                        <asp:LinkButton ID="btnSortCommonClass" runat="server" CommandArgument="CommonClass" Text="權限元件" OnClick="btnSort_Click"></asp:LinkButton>
                        <asp:Literal ID="hidSortCommonClass" runat="server" Visible="false" Text="權限元件"></asp:Literal>
                    </th>
                    <th title='<%= Resources.Lang.Col_SortNo_Hint %>' style="width:9%;">
                        <asp:LinkButton ID="btnSortSortNo" runat="server" CommandArgument="SortNo" Text="排序編號" OnClick="btnSort_Click"></asp:LinkButton>
                        <asp:Literal ID="hidSortSortNo" runat="server" Visible="false" Text="排序編號"></asp:Literal>
                    </th>
                    <th title="<%= Resources.Lang.Col_Management_Hint %>" style="width:26%"><%= Resources.Lang.Col_Management_Hint %></th>
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
                            <asp:LinkButton ID="btnMoveDown" runat="server" ToolTip="往下" CommandName="MoveDown" CommandArgument='<%# EvalToSafeStr("OpId") %>'>
                                <span class="fa fa-arrow-down fa-lg text-secondary"></span></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="btnMoveUp" runat="server" ToolTip="往上" CommandName="MoveUp" CommandArgument='<%# EvalToSafeStr("OpId") %>'>
                                <span class="fa fa-arrow-up fa-lg text-info"></span></asp:LinkButton>
                        </td>
                        <td>
                            <a id="btnItem" runat="server" href="#">
                                <img id="imgItem" runat="server" src="BPimages/icon/data.gif" align="absmiddle" style="width:16px; height:16px;" alt="*"/>
                                <asp:Literal ID="ltrSubject" runat="server"></asp:Literal></a>
                        </td>
                        <td>
                            <asp:Literal ID="ltrIsNewWindow" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ltrCommonClass" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <%# EvalToSafeStr("SortNo") %>
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

    </asp:PlaceHolder>
    <asp:PlaceHolder ID="PropertyArea" runat="server" Visible="false">

    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


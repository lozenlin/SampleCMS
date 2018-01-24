<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>
<%@ MasterType TypeName="MasterMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        <i class="fa fa-info-circle"></i> <%= Resources.Lang.Dashboard_Subtitle %>
    </div>
    <div>
        <h6 class="text-info font-weight-bold my-3"><%= Resources.Lang.Dashboard_Greeting %></h6>
        <%= Resources.Lang.Dashboard_LoginInfoTitle %>:
        <table class="table table-responsive-sm table-bordered table-sm bg-white">
            <tbody>
                <tr>
                    <th style="width:20%;"><%= Resources.Lang.Dashboard_AccountTitle %></th>
                    <td colspan="3">
                        <asp:Literal ID="ltrEmpAccount" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th style="width:20%;"><%= Resources.Lang.Dashboard_ThisLoginTimeTitle %></th>
                    <td style="width:30%;">
                        <asp:Literal ID="ltrThisLoginTime" runat="server"></asp:Literal>
                    </td>
                    <th style="width:20%;"><%= Resources.Lang.Dashboard_IpTitle %></th>
                    <td style="width:30%;">
                        <asp:Literal ID="ltrThisLoginIP" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th><%= Resources.Lang.Dashboard_LastLoginTimeTitle %></th>
                    <td>
                        <asp:Literal ID="ltrLastLoginTime" runat="server"></asp:Literal>
                    </td>
                    <th><%= Resources.Lang.Dashboard_IpTitle %></th>
                    <td>
                        <asp:Literal ID="ltrLastLoginIP" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th><small>Management System Ver.</small></th>
                    <td>
                        <small><asp:Literal ID="ltrSystemVersion" runat="server"></asp:Literal></small>
                    </td>
                    <th><small>Common.LogicObject Ver.</small></th>
                    <td>
                        <small><asp:Literal ID="ltrLogicObjectVersion" runat="server"></asp:Literal></small>
                    </td>
                </tr>
                <tr>
                    <th><small>Common.Utility Ver.</small></th>
                    <td>
                        <small><asp:Literal ID="ltrUtilityVersion" runat="server"></asp:Literal></small>
                    </td>
                    <th><small>Common.DataAccess Ver.</small></th>
                    <td>
                        <small><asp:Literal ID="ltrDataAccessVersion" runat="server"></asp:Literal></small>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle">
        <i class="fa fa-info-circle"></i> 訊息 Information
    </div>
    <div>
        <h6 class="text-info font-weight-bold my-3">您已成功登入後端管理系統！</h6>
        登入資訊:
        <table class="table table-bordered table-sm bg-white">
            <tbody>
                <tr>
                    <th class="w-25">帳　號</th>
                    <td>
                        Account name
                    </td>
                </tr>
                <tr>
                    <th>日　期</th>
                    <td>
                        2017-10-16 21:23
                    </td>
                </tr>
                <tr>
                    <th>IP 位置</th>
                    <td>
                        127.0.0.1
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


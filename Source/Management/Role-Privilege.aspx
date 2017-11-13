<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Role-Privilege.aspx.cs" Inherits="Role_Privilege" %>
<%@ MasterType TypeName="MasterConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <link href="Common/noUiSlider/nouislider.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2">作業選項名單</div>
            <div class="col-sm-6">
                <span class="text-primary">
                    <i class="fa fa-user-circle-o"></i>
                    <%= Resources.Lang.Main_RoleTitle %>: 
                    <asp:Literal ID="ltrRoleDisplayName" runat="server"></asp:Literal>
                    (<span id="roleName"><asp:Literal ID="ltrRoleName" runat="server"></asp:Literal></span>)
                </span>
            </div>
        </div>
    </div>
    <table class="table table-responsive-sm table-bordered table-sm table-striped table-hover bg-white subitem-list privileges-header">
        <thead>
            <tr>
                <th title="序號" class="seqno">&nbsp;</th>
                <th title="作業選項名稱" class="op-name">作業選項名稱</th>
                <th title="授權作業選項" class="item">作業選項</th>
                <th title="授權擁有的子項目" class="subitem-self">擁有的子項目</th>
                <th title="授權部門的子項目" class="subitem-crew">部門的子項目</th>
                <th title="授權所有的子項目" class="subitem-others">所有的子項目</th>
                <th title="編輯狀態" class="status">編輯狀態</th>
            </tr>
        </thead>
    </table>
    <div class="privileges-body-container">
        <table class="table table-responsive-sm table-bordered table-sm table-striped table-hover bg-white subitem-list privileges-body">
            <tbody>
            <asp:Repeater ID="rptOperations" runat="server" OnItemDataBound="rptOperations_ItemDataBound">
            <ItemTemplate>
                <tr id="OpArea" runat="server" class="op-area">
                    <td class="seqno">
                        <asp:Literal ID="ltrSeqno" runat="server"></asp:Literal>
                    </td>
                    <td class="op-name">
                        <img id="imgOpItem" runat="server" src="BPimages/icon/vectory_mini/basic/028.png" align="absmiddle" style="width:16px; height:16px;" alt="*"/> 
                        <span class="name"><asp:Literal ID="ltrOpItemSubject" runat="server"></asp:Literal></span>
                    </td>
                    <td class="item">
                        <div class="tags"><asp:Literal ID="ltrPvgOfItem" runat="server"></asp:Literal></div>
                        <input id="hidPvgOfItem" runat="server" type="hidden" value="0" class="hidPvgOfItem" />
                    </td>
                    <td class="subitem-self">
                        <div class="tags"><asp:Literal ID="ltrPvgOfSubitemSelf" runat="server"></asp:Literal></div>
                        <input id="hidPvgOfSubitemSelf" runat="server" type="hidden" value="0" class="hidPvgOfSubitemSelf" />
                    </td>
                    <td class="subitem-crew">
                        <div class="tags"><asp:Literal ID="ltrPvgOfSubitemCrew" runat="server"></asp:Literal></div>
                        <input id="hidPvgOfSubitemCrew" runat="server" type="hidden" value="0" class="hidPvgOfSubitemCrew" />
                    </td>
                    <td class="subitem-others">
                        <div class="tags"><asp:Literal ID="ltrPvgOfSubitemOthers" runat="server"></asp:Literal></div>
                        <input id="hidPvgOfSubitemOthers" runat="server" type="hidden" value="0" class="hidPvgOfSubitemOthers" />
                    </td>
                    <td class="status">
                        
                    </td>
                </tr>
                <asp:Repeater ID="rptSubOperations" runat="server" OnItemDataBound="rptOperations_ItemDataBound">
                <ItemTemplate>
                    <tr id="OpArea" runat="server" class="op-area">
                        <td class="seqno">
                            <asp:Literal ID="ltrSeqno" runat="server"></asp:Literal>
                        </td>
                        <td class="op-name">
                            <img id="imgOpItem" runat="server" src="BPimages/icon/vectory_mini/basic/028.png" align="absmiddle" style="width:16px; height:16px;" alt="*"/> 
                            <asp:Literal ID="ltrOpItemSubject" runat="server"></asp:Literal>
                        </td>
                        <td class="item">
                            <div class="tags"><asp:Literal ID="ltrPvgOfItem" runat="server"></asp:Literal></div>
                            <input id="hidPvgOfItem" runat="server" type="hidden" value="0" class="hidPvgOfItem" />
                        </td>
                        <td class="subitem-self">
                            <div class="tags"><asp:Literal ID="ltrPvgOfSubitemSelf" runat="server"></asp:Literal></div>
                            <input id="hidPvgOfSubitemSelf" runat="server" type="hidden" value="0" class="hidPvgOfSubitemSelf" />
                        </td>
                        <td class="subitem-crew">
                            <div class="tags"><asp:Literal ID="ltrPvgOfSubitemCrew" runat="server"></asp:Literal></div>
                            <input id="hidPvgOfSubitemCrew" runat="server" type="hidden" value="0" class="hidPvgOfSubitemCrew" />
                        </td>
                        <td class="subitem-others">
                            <div class="tags"><asp:Literal ID="ltrPvgOfSubitemOthers" runat="server"></asp:Literal></div>
                            <input id="hidPvgOfSubitemOthers" runat="server" type="hidden" value="0" class="hidPvgOfSubitemOthers" />
                        </td>
                        <td class="status">
                        
                        </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
            </asp:Repeater>
            </tbody>
        </table>
    </div>
    <hr class="content-divider" />
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-6">調整選擇的作業選項權限</div>
            <div class="col-sm-6">
            </div>
        </div>
    </div>
    <div class="card bg-light privilege-config-panel" style="display:none;">
        <div class="card-body p-1">
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right item-label font-weight-bold">
                    <span class="seqno"></span><span class="op-name"></span> (作業選項)
                </label>
                <div class="col-md-6 item slider-container">
                    <div id="ctlPvgOfItem" style="width:60%;"></div>
                </div>
            </div>
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right subitem-self-label">
                    擁有的子項目
                </label>
                <div class="col-md-6 subitem-self slider-container">
                    <div id="ctlPvgOfSubitemSelf" style="width:90%;"></div>
                </div>
                <div class="col-md-2 subitem-self-ext py-md-3">
                    <input type="checkbox" id="chkAdd" /><label for="chkAdd" class="badge badge-info">可新增</label>
                </div>
            </div>
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right">
                    部門的子項目
                </label>
                <div class="col-md-6 slider-container">
                    <div id="ctlPvgOfSubitemCrew" style="width:90%;"></div>
                </div>
            </div>
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right subitem-crew-label">
                    所有的子項目
                </label>
                <div class="col-md-6 subitem-crew slider-container">
                    <div id="ctlPvgOfSubitemOthers" style="width:90%;"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActionButtons" Runat="Server">
    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success" ValidationGroup="g" Visible="false"
        OnClick="btnSave_Click"><i class="fa fa-check-circle"></i> <%= Resources.Lang.ConfigForm_btnSave %></asp:LinkButton>
    <a href="#" class="btn btn-light" onclick="closeThisForm(); return false;"><%= Resources.Lang.ConfigForm_btnCancel %></a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
    <script src="Common/noUiSlider/nouislider.js"></script>
    <script>
        $(".pvg-badge").tooltip();
        var ctlPvgOfItem = document.getElementById("ctlPvgOfItem");
        noUiSlider.create(ctlPvgOfItem, {
            start: 0,
            step: 1,
            connect: [true, false],
            range: {
                "min": 0,
                "max": 2
            }
        });

        var ctlPvgOfSubitemSelf = document.getElementById("ctlPvgOfSubitemSelf");
        noUiSlider.create(ctlPvgOfSubitemSelf, {
            start: 0,
            step: 1,
            connect: [true, false],
            range: {
                "min": 0,
                "max": 3
            }
        });

        var ctlPvgOfSubitemCrew = document.getElementById("ctlPvgOfSubitemCrew");
        noUiSlider.create(ctlPvgOfSubitemCrew, {
            start: 0,
            step: 1,
            connect: [true, false],
            range: {
                "min": 0,
                "max": 3
            }
        });

        var ctlPvgOfSubitemOthers = document.getElementById("ctlPvgOfSubitemOthers");
        noUiSlider.create(ctlPvgOfSubitemOthers, {
            start: 0,
            step: 1,
            connect: [true, false],
            range: {
                "min": 0,
                "max": 3
            }
        });

    </script>
    <script src="Common/js/Role-Privilege.js"></script>
</asp:Content>


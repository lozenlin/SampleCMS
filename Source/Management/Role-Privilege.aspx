<%@ Page Language="C#" MasterPageFile="~/MasterConfig.master" AutoEventWireup="true" CodeFile="Role-Privilege.aspx.cs" Inherits="Role_Privilege" %>
<%@ MasterType TypeName="MasterConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <link href="Common/noUiSlider/nouislider.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="sys-subtitle container-fluid">
        <div class="form-row">
            <div class="col-sm-2"><%= Resources.Lang.GroupLabel_OperationList %></div>
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
                <th title='<%= Resources.Lang.Col_Seqno_Hint %>' class="seqno">&nbsp;</th>
                <th title='<%= Resources.Lang.Col_OperationName_Hint %>' class="op-name"><%= Resources.Lang.Col_OperationName %></th>
                <th title='<%= Resources.Lang.Col_PrivilegeOfItem_Hint %>' class="item"><%= Resources.Lang.Col_PrivilegeOfItem %></th>
                <th title='<%= Resources.Lang.Col_PrivilegeOfSubitemSelf_Hint %>' class="subitem-self"><%= Resources.Lang.Col_PrivilegeOfSubitemSelf %></th>
                <th title='<%= Resources.Lang.Col_PrivilegeOfSubitemCrew_Hint %>' class="subitem-crew"><%= Resources.Lang.Col_PrivilegeOfSubitemCrew %></th>
                <th title='<%= Resources.Lang.Col_PrivilegeOfSubitemOthers_Hint %>' class="subitem-others"><%= Resources.Lang.Col_PrivilegeOfSubitemOthers %></th>
                <th title='<%= Resources.Lang.Col_StatusOfEditing_Hint %>' class="status"><%= Resources.Lang.Col_StatusOfEditing %></th>
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
                        <span class="subject">
                            <img id="imgOpItem" runat="server" src="BPimages/icon/vectory_mini/basic/028.png" align="absmiddle" style="width:16px; height:16px;" alt="*"/> 
                            <asp:Literal ID="ltrOpItemSubject" runat="server"></asp:Literal>
                        </span>
                        <asp:Literal ID="ltrLastUpdateInfo" runat="server"></asp:Literal>
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
                            <span class="subject">
                                <img id="imgOpItem" runat="server" src="BPimages/icon/vectory_mini/basic/028.png" align="absmiddle" style="width:16px; height:16px;" alt="*"/> 
                                <asp:Literal ID="ltrOpItemSubject" runat="server"></asp:Literal>
                            </span>
                            <asp:Literal ID="ltrLastUpdateInfo" runat="server"></asp:Literal>
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
            <div class="col-sm-6"><%= Resources.Lang.GroupLabel_OperationPrivilegeConfig %></div>
            <div class="col-sm-6">
            </div>
        </div>
    </div>
    <div class="card bg-light privilege-config-panel" style="display:none;">
        <div class="card-body p-1">
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right item-label font-weight-bold" title='<%= Resources.Lang.Col_PrivilegeOfItem_Hint %>'>
                    <span class="seqno"></span><span class="op-name"></span> (<%= Resources.Lang.Col_OperationName %>)
                </label>
                <div class="col-md-6 item slider-container">
                    <div id="ctlPvgOfItem" style="width:60%;"></div>
                    <div class="scale-label" style="width:30%;margin-left:-23px;"><span class='badge badge-secondary ' title='<%= Resources.Lang.Privilege_NotAllowed_Hint %>'><%= Resources.Lang.Privilege_NotAllowed %></span></div>
                    <div class="scale-label" style="width:28%;"><span class='badge badge-warning text-white ' title='<%= Resources.Lang.Privilege_Read_Hint %>'><%= Resources.Lang.Privilege_Read %></span></div>
                    <div class="scale-label"><span class='badge badge-success ' title='<%= Resources.Lang.Privilege_Edit_Hint %>'><%= Resources.Lang.Privilege_Edit %></span></div>
                </div>
            </div>
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right subitem-self-label" title='<%= Resources.Lang.Col_PrivilegeOfSubitemSelf_Hint %>'>
                    <%= Resources.Lang.Col_PrivilegeOfSubitemSelf %>
                </label>
                <div class="col-md-6 subitem-self slider-container">
                    <div id="ctlPvgOfSubitemSelf" style="width:90%;"></div>
                    <div class="scale-label" style="width:30%;margin-left:-23px;"><span class='badge badge-secondary ' title='<%= Resources.Lang.Privilege_NotAllowed_Hint %>'><%= Resources.Lang.Privilege_NotAllowed %></span></div>
                    <div class="scale-label" style="width:28%;"><span class='badge badge-warning text-white ' title='<%= Resources.Lang.Privilege_Read_Hint %>'><%= Resources.Lang.Privilege_Read %></span></div>
                    <div class="scale-label" style="width:28%;"><span class='badge badge-success ' title='<%= Resources.Lang.Privilege_Edit_Hint %>'><%= Resources.Lang.Privilege_Edit %></span></div>
                    <div class="scale-label"><span class='badge badge-primary ' title='<%= Resources.Lang.Privilege_Delete_Hint %>'><%= Resources.Lang.Privilege_Delete %></span></div>
                </div>
                <div class="col-md-2 subitem-self-ext py-md-3">
                    <input type="checkbox" id="chkAdd" /><label for="chkAdd" class="badge badge-info" title='<%= Resources.Lang.Privilege_Add_Hint %>'><%= Resources.Lang.Privilege_Add %></label>
                </div>
            </div>
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right" title='<%= Resources.Lang.Col_PrivilegeOfSubitemCrew_Hint %>'>
                    <%= Resources.Lang.Col_PrivilegeOfSubitemCrew %>
                </label>
                <div class="col-md-6 slider-container">
                    <div id="ctlPvgOfSubitemCrew" style="width:90%;"></div>
                    <div class="scale-label" style="width:30%;margin-left:-23px;"><span class='badge badge-secondary ' title='<%= Resources.Lang.Privilege_NotAllowed_Hint %>'><%= Resources.Lang.Privilege_NotAllowed %></span></div>
                    <div class="scale-label" style="width:28%;"><span class='badge badge-warning text-white ' title='<%= Resources.Lang.Privilege_Read_Hint %>'><%= Resources.Lang.Privilege_Read %></span></div>
                    <div class="scale-label" style="width:28%;"><span class='badge badge-success ' title='<%= Resources.Lang.Privilege_Edit_Hint %>'><%= Resources.Lang.Privilege_Edit %></span></div>
                    <div class="scale-label"><span class='badge badge-primary ' title='<%= Resources.Lang.Privilege_Delete_Hint %>'><%= Resources.Lang.Privilege_Delete %></span></div>
                </div>
            </div>
            <div class="form-group form-row">
                <label class="col-md-4 col-form-label text-md-right subitem-crew-label" title='<%= Resources.Lang.Col_PrivilegeOfSubitemOthers_Hint %>'>
                    <%= Resources.Lang.Col_PrivilegeOfSubitemOthers %>
                </label>
                <div class="col-md-6 subitem-crew slider-container">
                    <div id="ctlPvgOfSubitemOthers" style="width:90%;"></div>
                    <div class="scale-label" style="width:30%;margin-left:-23px;"><span class='badge badge-secondary ' title='<%= Resources.Lang.Privilege_NotAllowed_Hint %>'><%= Resources.Lang.Privilege_NotAllowed %></span></div>
                    <div class="scale-label" style="width:28%;"><span class='badge badge-warning text-white ' title='<%= Resources.Lang.Privilege_Read_Hint %>'><%= Resources.Lang.Privilege_Read %></span></div>
                    <div class="scale-label" style="width:28%;"><span class='badge badge-success ' title='<%= Resources.Lang.Privilege_Edit_Hint %>'><%= Resources.Lang.Privilege_Edit %></span></div>
                    <div class="scale-label"><span class='badge badge-primary ' title='<%= Resources.Lang.Privilege_Delete_Hint %>'><%= Resources.Lang.Privilege_Delete %></span></div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidRoleId" runat="server" Value="" ClientIDMode="Static" />
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
        $(".mdf-info").tooltip();
    </script>
    <script src="Common/js/dao.js"></script>
    <script>
        langNo = '<%= c.seLangNoOfBackend %>';
        serviceUrl = "jsonService.ashx?l=" + langNo;
        var tagHtmlNotAllowed = '<%= GetTagHtml(PvgTagNameEnum.NotAllowed) %>';
        var tagHtmlRead = '<%= GetTagHtml(PvgTagNameEnum.Read) %>';
        var tagHtmlEdit = '<%= GetTagHtml(PvgTagNameEnum.Edit) %>';
        var tagHtmlAdd = '<%= GetTagHtml(PvgTagNameEnum.Add) %>';
        var tagHtmlDelete = '<%= GetTagHtml(PvgTagNameEnum.Delete) %>';

        var status_sending = '<%= Resources.Lang.PvgStatus_Sending %>';
        var status_temporarily_stored = '<%= Resources.Lang.PvgStatus_TempStored %>';
        var status_sent_failed = '<%= Resources.Lang.PvgStatus_SentFailed %>';
    </script>
    <script src="Common/js/Role-Privilege.js"></script>
</asp:Content>


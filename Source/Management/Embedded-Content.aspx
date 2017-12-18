<%@ Page Language="C#" MasterPageFile="~/MasterMain.master" AutoEventWireup="true" CodeFile="Embedded-Content.aspx.cs" Inherits="Embedded_Content" %>
<%@ MasterType TypeName="MasterMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="SubControlsArea" runat="server">
        <a id="btnInNewWindow" runat="server" class="btn btn-sm btn-success" target="_blank" 
            title="在新視窗開啟內容"><i class="icon-share icon-white"></i> <%= Resources.Lang.Col_OpenInNewWindow_Hint %></a>
    </div>
    <iframe id="EmbeddedContent" runat="server" src="" frameborder="0" width="100%" height="1000" scrolling="yes" rightmargin="0" topmargin="0">
    </iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
    <script>
        function adjustContainerHeight() {
            var $EmbeddedContent = $("#<%= EmbeddedContent.ClientID %>");
            var newHeight = document.documentElement.clientHeight - 265;
            var iframeSrc = $EmbeddedContent.attr("src");

            if (newHeight < 50) {
                newHeight = 50;
            }

            if (iframeSrc == "" || iframeSrc == "http://" || iframeSrc == "https://") {
                newHeight = 0;
            }

            $EmbeddedContent.height(newHeight);
        }

        if (!isMobile) {
            adjustContainerHeight();
            $(window).resize(adjustContainerHeight);
        }
    </script>

</asp:Content>


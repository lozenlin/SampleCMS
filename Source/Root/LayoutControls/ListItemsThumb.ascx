<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListItemsThumb.ascx.cs" Inherits="LayoutControls_ListItemsThumb" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

    <asp:Repeater ID="rptSubitems" runat="server" OnItemDataBound="rptSubitems_ItemDataBound">
        <HeaderTemplate>
            <div class="list-thumb">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="row list-item-thumb">
                <div class="col-xs-3">
                    <a id="btnPic" runat="server" href="#" class="thumbnail">
                        <img id="imgPic" runat="server" src="/images/project_7.jpg" alt="*" class="img-responsive"/>
                    </a>
                </div>
                <div class="col-xs-9">
                    <h2><a id="btnItem" runat="server" href="#"><%# basePage.EvalToSafeStr("ArticleSubject") %></a></h2>
                    <p class="descText"><%# basePage.EvalToSafeStr("TextContext") %></p>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:PlaceHolder ID="LazyLoadingArea" runat="server" Visible="false">
        <div class="list-thumb">

        </div>
    </asp:PlaceHolder>
    <div id="LazyLoadingCtrlArea" runat="server" class="lazy-loading-ctrls" visible="false">
        <a href="#" class="btnLoad"><%= Resources.Lang.LazyLoading_btnLoad %></a>
        <span class="loadingIcon" style="display:none;"><img src="/images/Spinner.gif" alt='<%= Resources.Lang.LazyLoading_Loading %>' width="80" /></span>
        <span class="isLastData" style="display:none;"><%= Resources.Lang.LazyLoading_IsLast %></span>
    </div>
    <uc1:wucDataPager ID="ucDataPager" runat="server" />

    <script src="js/dao.js"></script>
    <script>
        var langNo = '<%= c.qsLangNo %>';
        var serviceUrl = "/jsonService.ashx?l=" + langNo;
        var artid = '<%= articleData.ArticleId.Value.ToString() %>';
        var curPageCode = 0;
        var pageTotal = 0;
        var isLoading = false;

        $(function () {
            $(".btnLoad").click(function () {
                //切換按鈕
                $(this).hide();
                $(".loadingIcon").show();
                isLoading = true;

                //載入下一頁
                curPageCode += 1;

                dao.Article_GetListWithThumb(artid, curPageCode, function (cr) {
                    if (cr.b) {
                        var itemList = cr.o.itemList;

                        for (var k in itemList) {
                            var data = itemList[k];

                            var articleSubject = data.ArticleSubject;

                            var textContext = "";

                            if (data.TextContext != null) {
                                textContext = data.TextContext;
                            }

                            var itemUrl = "Article.aspx?artid=" + data.ArticleId + "&l=" + langNo;

                            var imgUrl = "";
                            var imgAlt = "*";

                            if (data.PicId != "") {
                                imgUrl = "/FileArtPic.ashx?attid=" + data.PicId + "&w=640&h=480&l=" + langNo;

                                if (data.PicSubject != "") {
                                    imgAlt = data.PicSubject;
                                }
                            } else {
                                imgUrl = "/images/project_7.jpg";
                            }

                            var itemHtml =
                                "<div class='row list-item-thumb'>" +
                                "    <div class='col-xs-3'>" +
                                "        <a href='" + itemUrl + "' class='thumbnail' title='" + articleSubject + "'>" +
                                "            <img src='" + imgUrl + "' alt='" + imgAlt + "' class='img-responsive'/>" +
                                "        </a>" +
                                "    </div>" +
                                "    <div class='col-xs-9'>" +
                                "        <h2><a href='" + itemUrl + "' title='" + articleSubject + "'>" + articleSubject + "</a></h2>" +
                                "        <p class='descText'>" + textContext + "</p>" +
                                "    </div>" +
                                "</div>";

                            $(".list-thumb").append(itemHtml);
                        }

                        curPageCode = cr.o.pageCode;
                        pageTotal = cr.o.pageTotal;
                    } else {
                        alert('<%= Resources.Lang.ErrMsg_LoadListFailed %>');
                    }

                    isLoading = false;
                    refreshCtrls();
                });

                return false;
            });

            function refreshCtrls() {
                $(".btnLoad").hide();
                $(".loadingIcon").hide();
                $(".isLastData").hide();

                if (curPageCode >= pageTotal) {
                    //沒下一頁
                    $(".isLastData").show();
                } else {
                    $(".btnLoad").show();
                }
            }

            $(window).scroll(function () {
                if (isLoading || $(".btnLoad").length == 0) {
                    return;
                }

                //檢查載入鈕,進到畫面內就自動觸發
                var topOfBtn = $(".btnLoad").position().top;

                if ($(this).scrollTop() + $(this).height() > topOfBtn + 10) {
                    if ($(".btnLoad").is(":visible")) {
                        $(".btnLoad").click();
                    }
                }
            });

            //第一次載入
            $(".btnLoad").click();
        });

    </script>
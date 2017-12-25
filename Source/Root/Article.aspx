<%@ Page Language="C#" MasterPageFile="~/MasterArticle.master" AutoEventWireup="true" CodeFile="Article.aspx.cs" Inherits="Article" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <asp:PlaceHolder ID="SubitemsArea" runat="server" Visible="false">
        <div class="list-group item-list">
            <a href="#" class="list-group-item">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="item-date text-primary">2017-12-16</div>
                    </div>
                    <div class="col-sm-9">
                        <h2 class="list-group-item-heading"><span class="text-primary">List group item heading</span></h2>
                        <p class="list-group-item-text">Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.</p>
                    </div>
                </div>
            </a>
            <a href="#" class="list-group-item">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="item-date text-primary">2017-12-16</div>
                    </div>
                    <div class="col-sm-9">
                        <h2 class="list-group-item-heading"><span class="text-primary">List group item heading</span></h2>
                        <p class="list-group-item-text">Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.</p>
                    </div>
                </div>
            </a>
            <a href="#" class="list-group-item">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="item-date text-primary">2017-12-16</div>
                    </div>
                    <div class="col-sm-9">
                        <h2 class="list-group-item-heading"><span class="text-primary">List group item heading</span></h2>
                        <p class="list-group-item-text">Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.</p>
                    </div>
                </div>
            </a>
            <a href="#" class="list-group-item">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="item-date text-primary">2017-12-16</div>
                    </div>
                    <div class="col-sm-9">
                        <h2 class="list-group-item-heading"><span class="text-primary">List group item heading</span></h2>
                        <p class="list-group-item-text">Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.</p>
                    </div>
                </div>
            </a>
            <a href="#" class="list-group-item">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="item-date text-primary">2017-12-16</div>
                    </div>
                    <div class="col-sm-9">
                        <h2 class="list-group-item-heading"><span class="text-primary">List group item heading</span></h2>
                        <p class="list-group-item-text">Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.</p>
                    </div>
                </div>
            </a>
        </div>

        <div style="text-align:center;">
            <ul class="pagination">
                <li class="disabled"><a href="#" aria-label="Previous"><span aria-hidden="true">«</span></a></li>
                <li class="active"><a href="#">1 <span class="sr-only">(current)</span></a></li>
                <li><a href="#">2</a></li>
                <li><a href="#">3</a></li>
                <li><a href="#">4</a></li>
                <li><a href="#">5</a></li>
                <li><a href="#" aria-label="Next"><span aria-hidden="true">»</span></a></li>
            </ul>
        </div>
    </asp:PlaceHolder>
    <asp:Literal ID="ltrArticleContext" runat="server"></asp:Literal>
    <asp:PlaceHolder ID="ControlArea" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


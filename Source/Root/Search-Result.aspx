<%@ Page Language="C#" MasterPageFile="~/MasterArticle.master" AutoEventWireup="true" CodeFile="Search-Result.aspx.cs" Inherits="Search_Result" %>
<%@ Register Src="~/UserControls/wucDataPager.ascx" TagPrefix="uc1" TagName="wucDataPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="row">
        <div class="col-sm-8 col-sm-offset-1">
            <div class="form-group">
                <input type="text" class="form-control" placeholder="keyword" />
            </div>
        </div>
        <div class="col-sm-3">
            <div class="form-group">
                <a href="#" class="btn btn-primary btn-sm" style="margin-bottom:0;"><i class="glyphicon glyphicon-search"></i> Search</a>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-11 col-sm-offset-1">
            <b>Keyword:</b> <span class="text-success">one two three</span>
        </div>
        <div class="col-sm-11 col-sm-offset-1">
            <b>Total:</b> <span class="text-success">50</span>
        </div>
    </div>
	<div class="fh5co-spacer fh5co-spacer-sm"></div>
    <div class="row result-item">
        <div class="col-xs-12">
            <h2 class=""><a href="#">Sapiente voluptatem distinctio</a></h2>
            <p>Lorem ipsum dolor sit amet, <span class="label label-warning">one</span> consectetur adipisicing elit. Sapiente voluptatem distinctio neque, a, id debitis tenetur aspernatur? Natus, voluptate alias.</p>
            <ol class="breadcrumb item-route">
                <li>Position: </li>
                <li><a href="#">Library</a></li>
                <li><a href="#">Library</a></li>
                <li><a href="#">Library</a></li>
                <li><a href="#">Library</a></li>
                <li class="active">Data</li>
            </ol>
        </div>
    </div>
    <div class="row result-item">
        <div class="col-xs-12">
            <h2 class=""><a href="#">Sapiente voluptatem distinctio</a></h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente voluptatem distinctio neque, a, id debitis tenetur aspernatur? Natus, voluptate alias.</p>
            <ol class="breadcrumb item-route">
                <li>Position: </li>
                <li><a href="#">Library</a></li>
                <li class="active">Data</li>
            </ol>
        </div>
    </div>
    <div class="row result-item">
        <div class="col-xs-12">
            <h2 class=""><a href="#">Sapiente voluptatem distinctio</a></h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente voluptatem distinctio neque, a, id debitis tenetur aspernatur? Natus, voluptate alias.</p>
            <ol class="breadcrumb item-route">
                <li>Position: </li>
                <li><a href="#">Library</a></li>
                <li class="active">Data</li>
            </ol>
        </div>
    </div>
    <div class="row result-item">
        <div class="col-xs-12">
            <h2 class=""><a href="#">Sapiente voluptatem distinctio</a></h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente voluptatem distinctio neque, a, id debitis tenetur aspernatur? Natus, voluptate alias.</p>
            <ol class="breadcrumb item-route">
                <li>Position: </li>
                <li><a href="#">Library</a></li>
                <li class="active">Data</li>
            </ol>
        </div>
    </div>
    <div class="row result-item">
        <div class="col-xs-12">
            <h2 class=""><a href="#">Sapiente voluptatem distinctio</a></h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sapiente voluptatem distinctio neque, a, id debitis tenetur aspernatur? Natus, voluptate alias.</p>
            <ol class="breadcrumb item-route">
                <li>Position: </li>
                <li><a href="#">Library</a></li>
                <li class="active">Data</li>
            </ol>
        </div>
    </div>

    <uc1:wucDataPager ID="ucDataPager" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContentOfHomePage" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBeforeBodyTail" Runat="Server">
</asp:Content>


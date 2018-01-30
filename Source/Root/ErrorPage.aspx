<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>
<%@ Register Src="~/UserControls/wucSearchCondition.ascx" TagPrefix="uc1" TagName="wucSearchCondition" %>
<%@ OutputCache Location="Server" VaryByParam="l" Duration="1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title><%= Resources.Lang.ErrorPage_Subject %></title>
	<meta name="description" content="page not found" />
    <meta name="keywords" content="error, page not found" />

  	<!-- Facebook and Twitter integration -->
	<meta property="og:title" content=""/>
	<meta property="og:image" content=""/>
	<meta property="og:url" content=""/>
	<meta property="og:site_name" content=""/>
	<meta property="og:description" content=""/>
	<meta name="twitter:title" content="" />
	<meta name="twitter:image" content="" />
	<meta name="twitter:url" content="" />
	<meta name="twitter:card" content="" />

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <link rel="shortcut icon" href="favicon.ico"/>

    <!-- Google Webfont -->
	<link href='//fonts.googleapis.com/css?family=Lato:300,400|Crimson+Text' rel='stylesheet' type='text/css'/>
	<!-- Themify Icons -->
	<link rel="stylesheet" href="/css/themify-icons.css"/>
	<!-- Bootstrap -->
	<link rel="stylesheet" href="/css/bootstrap.css"/>
	<!-- Owl Carousel -->
	<link rel="stylesheet" href="/css/owl.carousel.min.css"/>
	<link rel="stylesheet" href="/css/owl.theme.default.min.css"/>
	<!-- Magnific Popup -->
	<link rel="stylesheet" href="/css/magnific-popup.css"/>
	<!-- Superfish -->
	<link rel="stylesheet" href="/css/superfish.css"/>
	<!-- Easy Responsive Tabs -->
	<link rel="stylesheet" href="/css/easy-responsive-tabs.css"/>

	<!-- Theme Style -->
	<link rel="stylesheet" href="/css/style.css"/>

	<!-- FOR IE9 below -->
	<!--[if lt IE 9]>
	<script src="js/modernizr-2.6.2.min.js"></script>
	<script src="js/respond.min.js"></script>
	<![endif]-->
    <style type="text/css">
        @media screen and (min-width: 769px) {
            #fh5co-main {
            margin-top: 150px !important;
            }
        }
    </style>
    <link href="/Common/jquery-ui-1.12.1.Autocomplete/jquery-ui.css" rel="stylesheet" />
    <link href="/css/FrontendPage.css" rel="stylesheet" />

	<!-- jQuery -->
	<!--<script src="js/jquery-1.10.2.min.js"></script>-->
	<script src="js/jquery-3.2.1.js"></script>
	<!--<script src="js/jquery-migrate-3.0.0.js"></script>-->
</head>
<body>
    <form id="form1" runat="server">
		<!-- START #fh5co-header -->
		<header id="fh5co-header-section" role="header" class="" >
			<div class="container">
					
				<!-- START #fh5co-logo -->
				<h1 id="fh5co-logo" class="pull-left"><a href="Index.aspx?l=<%= c.qsLangNo %>"><%= Resources.Lang.WebsiteName %></a></h1>
					

			</div>
		</header>
			
		<!-- START #fh5co-hero -->
		<aside id="fh5co-hero" style="background-image: url(images/hero2.jpg); height:150px;">
			<div class="container">
				<div class="row">
					<div class="col-md-8 col-md-offset-2">
						<div class="fh5co-hero-wrap">
							<div class="fh5co-hero-intro">
								<h1><%= Resources.Lang.ErrorPage_Subject %></h1>
							</div>
						</div>
					</div>
				</div>
			</div>
		</aside>

		<div id="fh5co-main">
			<!-- main start -->
			<section>
				<div class="container">
                    <div class="row">
                        <div class="col-md-8">

                        </div>
                        <div class="col-md-4">
                            <uc1:wucSearchCondition ID="ucSearchCondition" runat="server" />
                        </div>
                    </div>
					<div class="row">
						<div class="col-md-12">
                            <!-- context start -->
                            <div class="jumbotron text-center" style="max-width:600px;margin:1rem auto;">
                                <h2 class=""><%= Resources.Lang.ErrorPage_Subject %></h2>
                                <p class="lead"><%= Resources.Lang.ErrorPage_Context %></p>
                                <hr class="my-4"/>
                                <p><%= Resources.Lang.ErrorPage_VisitThe %></p>
                                <p class="lead">
                                    <a class="btn btn-primary" href="Index.aspx?l=<%= c.qsLangNo %>" role="button"><%= Resources.Lang.ErrorPage_btnHome %></a>
                                </p>
                            </div>                            <!-- context end -->
						</div>
					</div>

                    <div class="fh5co-spacer fh5co-spacer-lg"></div>
				</div>
			</section>
			<!-- main end -->
			<div class="fh5co-spacer fh5co-spacer-sm"></div>
			<footer id="fh5co-footer">
				<div class="container">
						
					<ul class="fh5co-social-icons">
						<li><a href="#"><i class="ti-twitter-alt"></i></a></li>
						<li><a href="#"><i class="ti-facebook"></i></a></li>
						<li><a href="#"><i class="ti-github"></i></a></li>
						<li><a href="#"><i class="ti-linkedin"></i></a></li>
					</ul>
					<p class="text-muted fh5co-no-margin-bottom text-center"><small>&copy; 2015 <a href="#">Display</a>. All rights reserved. Crafted with love <em>by</em> <a href="http://freehtml5.co" target="_blank">FREEHTML5.co</a> <br> Images by <a href="http://unsplash.com/" target="_blank">Unsplash</a></small></p>

				</div>
			</footer>
			
		
		</div>
			
			
		<!-- jQuery Easing -->
		<script src="js/jquery.easing.1.3.js"></script>
		<!-- Bootstrap -->
		<script src="js/bootstrap.js"></script>
		<!-- Owl carousel -->
		<script src="js/owl.carousel.min.js"></script>
		<!-- Magnific Popup -->
		<script src="js/jquery.magnific-popup.min.js"></script>
		<!-- Superfish -->
		<script src="js/hoverIntent.js"></script>
		<script src="js/superfish.js"></script>
		<!-- Easy Responsive Tabs -->
		<script src="js/easyResponsiveTabs.js"></script>
		<!-- FastClick for Mobile/Tablets -->
		<script src="js/fastclick.js"></script>
		<!-- Main JS -->
		<script src="js/main.js"></script>
        <script src="Common/jquery-ui-1.12.1.Autocomplete/jquery-ui.min.js"></script>
        <script src="js/dao.js"></script>
        <script>
            var langNo = '<%= c.qsLangNo %>';
            var serviceUrl = "/jsonService.ashx?l=" + langNo;

            $(function () {
                $("#fh5co-mobile-menu-btn").hide();
            });

            $(window).resize(function () {
                if ($("#fh5co-mobile-menu-btn").is(":visible")) {
                    $("#fh5co-mobile-menu-btn").hide();
                }
            });
        </script>
    </form>
</body>
</html>

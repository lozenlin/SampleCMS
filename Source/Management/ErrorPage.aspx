<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <title>PAGE NOT FOUND</title>
    <link href="/Management/Common/bootstrap-4/css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="jumbotron text-center" style="max-width:600px;margin:1rem auto;">
            <h1 class="display-4">PAGE NOT FOUND</h1>
            <p class="lead">The link you clicked may be broken or the page may have been removed.</p>
            <hr class="my-4"/>
            <p>visit the</p>
            <p class="lead">
                <a class="btn btn-primary" href="/Management/Dashboard.aspx" role="button">HOMEPAGE</a>
            </p>
        </div>
    </form>
</body>
</html>

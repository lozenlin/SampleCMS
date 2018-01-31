<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <title></title>
    <link href="/management/Common/bootstrap-4/css/bootstrap.css" rel="stylesheet" />
    <link href="/management/Common/css/BpBootstrapChanged.css" rel="stylesheet" />
    <link href="/management/Common/css/BackendPage.css" rel="stylesheet" />
    <link href="/management/Common/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
</head>
<body class="login-body">
    <form id="form1" runat="server" defaultbutton="btnLogin">
        <div class="container-fluid">
            <div class="row mt-md-5 no-gutters">
                <div class="col-md-8 mx-auto LoginFormContainer">
                    <div class="card bgtx-glass-diamond px-md-4 pb-md-3 LoginWell">
                        <div class="card-body">
                            <div class="Signboard">
                                <h1 class="SignboardText"><asp:Literal ID="ltrBackStageName" runat="server"></asp:Literal></h1>
                            </div>
                            <hr class="SignboardLine" />
                            <div id="ErrorMsgArea" runat="server" visible="false" class="alert alert-danger alert-dismissible fade show" role="alert">
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <asp:Literal ID="ltrErrMsg" runat="server"></asp:Literal>
                            </div>
                            <div class="card LoginForm border border-secondary ">
                                <div class="card-header bg-white">
                                    <%= Resources.Lang.Login_Subtitle %>
                                    <a id="btnChangLang" runat="server" href="#" class="float-right">English</a>
                                </div>
                                <div class="card-body pr-md-5">
                                    <div class="form-group form-row">
                                        <label for="txtAccount" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.Login_AccountTitle %></label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtAccount" runat="server" ClientIDMode="Static" CssClass="form-control" 
                                                placeholder="Your account" autocomplete="off" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ControlToValidate="txtAccount" CssClass="text-danger"
                                                Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group form-row">
                                        <label for="txtPassword" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.Login_PasswordTitle %></label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtPassword" runat="server" ClientIDMode="Static" TextMode="Password" 
                                                CssClass="form-control" placeholder="Password" autocomplete="off"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" CssClass="text-danger"
                                                Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div id="CheckCodeArea" runat="server" class="form-group form-row" visible="false">
                                        <label for="txtCheckCode" class="col-md-3 col-form-label text-md-right"><%= Resources.Lang.Login_CheckCodeTitle %></label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCheckCode" runat="server" ClientIDMode="Static" CssClass="form-control"
                                                placeholder="Verification code" autocomplete="off"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCheckCode" runat="server" ControlToValidate="txtCheckCode" CssClass="text-danger"
                                                Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="g" ></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cuvCheckCode" runat="server" ControlToValidate="txtCheckCode" CssClass="text-danger"
                                                Display="Dynamic" ErrorMessage="*錯誤" SetFocusOnError="true" ValidationGroup="g" OnServerValidate="cuvCheckCode_ServerValidate"></asp:CustomValidator>
                                        </div>
                                        <div class="col-md-5">
                                            <img id="imgCaptcha" src="captcha.ashx" alt="*" style="height:33px;" />
                                            <a id="btnRefreshCodePic" runat="server" href="#" >Refresh</a>
                                        </div>
                                    </div>
                                    <div class="form-group form-row align-items-end">
                                        <div class="col-md-3"></div>
                                        <div class="col-md-3 col-5">
                                            <asp:LinkButton ID="btnLogin" runat="server" CssClass="btn btn-primary" ValidationGroup="g"
                                                OnClick="btnLogin_Click"><i class="fa fa-check-circle" aria-hidden="true"></i> <%= Resources.Lang.Login_btnLogin %></asp:LinkButton>
                                        </div>
                                        <div class="col-md-6 col-7">
                                            <a id="btnChangePsw" runat="server" href="#" class="btn btn-secondary btn-sm"><%= Resources.Lang.Login_btnChangePsw %></a>
                                            <div class="m-1" style='<%= (c.qsLangNo == 1) ? "display:none;" : "" %>'></div>
                                            <a id="btnDontRememberPsw" runat="server" href="#" class="btn btn-info btn-sm"><%= Resources.Lang.Login_btnDontRememberPsw %></a>
                                        </div>
                                    </div>
                                    <div class="form-group form-row" runat="server" visible="false">
                                        <div class="col-md-3"></div>
                                        <div class="col-md-9">
                                            <div class="card border-info small mr-md-5">
                                                <div class="card-header">Guest account info.</div>
                                                <div class="card-body text-info">
                                                    <p class="card-text">
                                                        Account: <strong>guest</strong><br />
                                                        Password: <strong>guestguest</strong>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-footer">
                                    <small>
                                        <span class="text-muted"><%= Resources.Lang.Login_lblBackTo %></span>
                                        <a href='<%= ConfigurationManager.AppSettings["WebsiteUrl"] %>?l=<%= c.qsLangNo %>'><%= Resources.Lang.WebsiteName %></a>
                                    </small>
                                    <small class="text-success float-right"><%= Resources.Lang.Login_YourIpTitle %>: <asp:Literal ID="ltrClientIP" runat="server"></asp:Literal></small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script src="Common/js/jquery-3.2.1.min.js"></script>
        <!--<script src="Common/js/jquery-migrate-3.0.0.js"></script>-->
        <script src="Common/js/popper.min.js"></script>
        <script src="Common/bootstrap-4/js/bootstrap.min.js"></script>
        <script>
            $("#btnRefreshCodePic").click(function () {
                $("#imgCaptcha").attr("src", "captcha.ashx?" + (new Date()).valueOf());
                return false;
            });
        </script>
    </form>
    <span class="photo-author d-none">Photo belongs to xxx on yyy.</span>
</body>
</html>

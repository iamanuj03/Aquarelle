<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loginForm.aspx.cs" Inherits="Aquarelle.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" />
    <%-- <script src="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>--%>

    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
</head>
<body>

    <div class="alert alert-danger alert-dismissible fade show" role="alert" id="LoginAlert" runat="server" visible="false">
        <h4 class="alert-heading">Error!</h4>
        <b><label id="LoginErrorShortMessage" runat="server" /></b>
        <hr/>
        <h4 class="alert-heading">Possible actions</h4>
        <label class="mb-0" id="LoginErrorLongMessage" runat="server" />
        <a href="#" class="close" data-dismiss="alert" aria-label="Close">&times;</a>
    </div>

    <div id="login">
        <h3 class="text-center text-white pt-5">Aquarelle</h3>
        <div class="container">
            <div id="login-row" class="row justify-content-center align-items-center">
                <div id="login-column" class="col-md-6">
                    <div id="login-box" class="col-md-12">
                        <form id="loginForm" class="form" runat="server">
                            <h3 class="text-center text-info">Login</h3>
                            <div class="form-group">
                                <label for="username" class="text-info">Username:</label><br>
                                <input type="text" name="username" id="username" class="form-control" runat="server" />
                            </div>
                            <div class="form-group">
                                <label for="password" class="text-info">Password:</label><br>
                                <input type="text" name="password" id="password" class="form-control" runat="server" />
                            </div>
                            <div class="form-group">
                                <asp:Button class="btn btn-info btn-md"
                                    ID="loginBtn" runat="server"
                                    OnClick="loginBtn_click"
                                    Text="Submit" />
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

<style type="text/css">
    body {
        margin: 0;
        padding: 0;
        background-color: #17a2b8;
        height: 100vh;
    }

    #login .container #login-row #login-column #login-box {
        margin-top: 120px;
        max-width: 600px;
        height: 320px;
        border: 1px solid #9C9C9C;
        background-color: #EAEAEA;
    }

        #login .container #login-row #login-column #login-box #login-form {
            padding: 20px;
        }

            #login .container #login-row #login-column #login-box #login-form #register-link {
                margin-top: -85px;
            }
</style>




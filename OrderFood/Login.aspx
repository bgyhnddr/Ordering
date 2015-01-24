<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OrderFood.Login" %>

<!DOCTYPE>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="JS/core/jquery.js"></script>
    <script type="text/javascript">
        document.body.layout = function () {
            var padding = getComputedStyle(document.body).padding;
            padding = padding.replace("px");
            try {
                padding = parseInt(padding);
            }
            catch (e) {
                padding = 0;
            }
        };

    </script>
    <link href="CSS/Form.css" rel="stylesheet" />
    <style type="text/css">
        #mainlogin {
            position:absolute;
            top:50%;
            left:50%;
            transform:translateX(-50%) translateY(-50%);

        }
    </style>
</head>
<body class="form">
    <form id="mainlogin" runat="server">
        <div>
            <p>
                <label for="AccountInput">
                    帐号
                </label>
                <asp:TextBox ID="AccountInput" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="PasswordInput">
                    密码
                </label>
                <asp:TextBox ID="PasswordInput" runat="server" TextMode="Password"></asp:TextBox>
            </p>
            <asp:Button ID="btnLogin" runat="server" Text="登录" OnClick="btnLogin_Click" />
            <asp:Button ID="btnRegister" runat="server" Text="注册" OnClick="btnRegister_Click" />
        </div>
    </form>
</body>
</html>

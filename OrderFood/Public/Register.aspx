﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="OrderFood.Public.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/Form.css" rel="stylesheet" />
</head>
<body class="form">
    <form id="form1" runat="server">
        <p>
            <asp:Label ID="lbTitle" runat="server" Text="注册新帐号"></asp:Label>
        </p>
        <p>
            <label for="tbAccount">帐号</label>
            <asp:TextBox ID="tbAccount" runat="server"></asp:TextBox>
        </p>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbAccount"></asp:RequiredFieldValidator>
        <p>
            <label for="tbPassword">密码</label>
            <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="lbPassword2" runat="server" Text="确认密码"></asp:Label>
            <asp:TextBox ID="tbPassword2" runat="server" TextMode="Password"></asp:TextBox>
        </p>
        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="密码错误" ControlToCompare="tbPassword2" ControlToValidate="tbPassword"></asp:CompareValidator>
        <asp:Button ID="btnConfirm" runat="server" Text="注册"
            OnClick="btnConfirm_Click" />
        <asp:Button ID="btnCencel" runat="server" Text="取消" CausesValidation="False"
            OnClick="btnCencel_Click" />
    </form>
</body>
</html>

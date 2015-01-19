<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="OrderFood.Public.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 30%; text-align:center">
        <asp:Label ID="lbTitle" runat="server" Text="注册新帐号"></asp:Label></div>
    <div style="float:left; width: 10%; text-align:center">
        <asp:Label ID="lbAccount" runat="server" Text="帐号"></asp:Label>
    </div>
    <div style="float:left;width:90%">
        <asp:TextBox ID="tbAccount" runat="server" Width="180px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="tbAccount"></asp:RequiredFieldValidator>
        </div>
    <div style="float:left; width: 10%; text-align:center">
        <asp:Label ID="lbPassword" runat="server" Text="密码"></asp:Label>
    </div>
    <div style="float:left;width:90%">
        <asp:TextBox ID="tbPassword" runat="server" Width="180px" TextMode="Password"></asp:TextBox>
    </div>
    <div style="float:left; width: 10%; text-align:center">
        <asp:Label ID="lbPassword2" runat="server" Text="确认密码"></asp:Label>
    </div>
    <div style="float:left;width:90%">
        <asp:TextBox ID="tbPassword2" runat="server" Width="180px" TextMode="Password"></asp:TextBox>
        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="密码错误" ControlToCompare="tbPassword2" ControlToValidate="tbPassword"></asp:CompareValidator>
    </div>
    <div style="float:left; width: 10%; text-align:center">
        <asp:Button ID="btnConfirm" runat="server" Text="注册" 
            onclick="btnConfirm_Click" />
        </div>
        <div style="float:left; width: 10%; text-align:center">
        <asp:Button ID="btnCencel" runat="server" Text="取消" CausesValidation="False" 
                onclick="btnCencel_Click" />
        </div>
    </form>
</body>
</html>

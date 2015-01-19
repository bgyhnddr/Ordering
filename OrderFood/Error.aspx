<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="OrderFood.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="Image1" runat="server" />
        <asp:Label ID="lbError" runat="server" Text="请在点餐时间9点到11点正内点餐，并选择正确的餐类。"></asp:Label>
        <asp:Button ID="btTry" runat="server" onclick="btTry_Click" Text="重刷新" />
    </div>
    </form>
</body>
</html>

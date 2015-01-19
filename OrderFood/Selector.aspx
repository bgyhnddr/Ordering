<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Selector.aspx.cs" Inherits="OrderFood.Selector" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <script src="JS/jquery-1.9.1.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <asp:Label ID="lbHello" runat="server" Text="您好"></asp:Label><br />
            <asp:ListBox ID="ListBox1" runat="server" DataSourceID="ordersource"
                DataTextField="HasManager" DataValueField="OrderType" Height="200px" Width="183px"></asp:ListBox>
            <asp:Button ID="Button1" runat="server" Text="进入点餐" OnClick="Button1_Click" />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="登出" />
            <asp:SqlDataSource ID="ordersource" runat="server"
                ConnectionString="<%$ ConnectionStrings:orderfoodConnectionString %>"
                SelectCommand="SELECT OrderType + CASE WHEN (CONVERT (VARCHAR(10) , Date , 120) = CONVERT (VARCHAR(10) , GETDATE() , 120) AND OrderMan IS NOT NULL) THEN '(负责人' + OrderMan + ')' ELSE '(无负责人)' END AS HasManager,OrderType, OrderMan, Date FROM OrderType"></asp:SqlDataSource>
            <p>
                开始时间<asp:TextBox ID="DateBegin" runat="server" TextMode="Date"></asp:TextBox>
                结束时间<asp:TextBox ID="DateEnd" runat="server" TextMode="Date"></asp:TextBox>
                <asp:Button ID="search" runat="server" Text="查询费用" OnClick="search_Click" />
            </p>
                自我费用：<asp:GridView ID="GridView2" runat="server" style="margin:auto" AutoGenerateColumns="False" DataSourceID="castList">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="名字" SortExpression="Name" />
                        <asp:BoundField DataField="OrderFood" HeaderText="餐名" SortExpression="OrderFood" />
                        <asp:BoundField DataField="Number" HeaderText="数量" SortExpression="Number" />
                        <asp:BoundField DataField="Money" HeaderText="费用" SortExpression="Money" />
                        <asp:BoundField DataField="Date" HeaderText="日期" SortExpression="Date" />
                    </Columns>
                </asp:GridView>
                总费用：
        <asp:GridView ID="GridView1" runat="server" style="margin:auto"  AutoGenerateColumns="False"
            DataSourceID="OrderList">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="名字" SortExpression="Name" />
                <asp:BoundField DataField="Money" HeaderText="总费用" SortExpression="Money" ReadOnly="True" />
            </Columns>
        </asp:GridView>
            <asp:SqlDataSource ID="castList" runat="server" ConnectionString="<%$ ConnectionStrings:orderfoodConnectionString %>" SelectCommand="SELECT [Name], [OrderFood], [Number], [Money], [Date] FROM [OrderList]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="OrderList" runat="server"
                ConnectionString="<%$ ConnectionStrings:orderfoodConnectionString %>"
                SelectCommand="SELECT Name, SUM(Money) AS Money FROM OrderList GROUP BY Name"></asp:SqlDataSource>


        </div>
    </form>
</body>
</html>

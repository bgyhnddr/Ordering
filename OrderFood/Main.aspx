<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="OrderFood.Main" %>

<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="JS/jquery.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbUser" runat="server" Text="点餐人："></asp:Label>
            <asp:TextBox ID="tbUserName" runat="server" ReadOnly="True" Width="203px"></asp:TextBox>
            <asp:Label ID="lbOrderMan" runat="server" Text="负责人："></asp:Label>
            <asp:TextBox ID="tbOrderMan" runat="server"></asp:TextBox>
            <asp:Button ID="btBeOrderMan" runat="server" OnClick="btBeOrderMan_Click"
                Text="成为今天的点餐负责人" />
            <asp:Button ID="btChangeOrderMan" runat="server"
                OnClick="btChangeOrderMan_Click" Text="更改负责人" />
            <div style="float: left; width: 75%">
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh" />
                <asp:GridView ID="GridView1" runat="server" DataSourceID="orderfoodlist"
                    AutoGenerateColumns="False" DataKeyNames="id"
                    OnRowEditing="GridView1_RowEditing" OnRowDataBound="GridView1_RowDataBound"
                    OnRowCommand="GridView1_RowCommand" OnDataBinding="GridView1_DataBinding">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                            ReadOnly="True" SortExpression="id" Visible="False" />
                        <asp:BoundField DataField="Name" HeaderText="点餐人" SortExpression="Name"
                            InsertVisible="False" ReadOnly="True" />
                        <asp:BoundField DataField="Type" HeaderText="餐类"
                            SortExpression="Type" InsertVisible="False" ReadOnly="True" />
                        <asp:BoundField DataField="OrderFood" HeaderText="餐名"
                            SortExpression="OrderFood" />
                        <asp:BoundField DataField="Money" HeaderText="应付款" SortExpression="Money" />
                        <asp:BoundField DataField="Date" HeaderText="点餐时间" SortExpression="Date"
                            InsertVisible="False" ReadOnly="True" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lbType" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lbOrderFood" runat="server" Text="餐名："></asp:Label>
                <asp:TextBox ID="tbOrderFood" runat="server"></asp:TextBox>
                <asp:Label ID="lbNumber" runat="server" Text="数量："></asp:Label>
                <asp:TextBox ID="tbNumber" runat="server"></asp:TextBox>
                <asp:Label ID="lbPay" runat="server" Text="应付款："></asp:Label>
                <asp:TextBox ID="tbPay" runat="server" TextMode="Number"></asp:TextBox>
                <asp:Button ID="btOrder" runat="server" Text="点餐" OnClick="btOrder_Click" />
                <asp:SqlDataSource ID="orderfoodlist" runat="server"
                    ConnectionString="<%$ ConnectionStrings:orderfoodConnectionString %>"
                    SelectCommand="SELECT * FROM [OrderList]"
                    DeleteCommand="DELETE FROM [OrderList] WHERE [id] = @id"
                    UpdateCommand="UPDATE [OrderList] SET [OrderFood] = @OrderFood, [Number] = @Number, [Money] = @Money WHERE [id] = @id">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="OrderFood" Type="String" />
                        <asp:Parameter Name="Number" Type="Int32" />
                        <asp:Parameter Name="Money" Type="Decimal" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:TextBox ID="tbFoodType" runat="server" ReadOnly="True"></asp:TextBox>
                <asp:Button ID="btSetType" runat="server" Text="设定餐类"
                    OnClick="btSetType_Click" Visible="False" />
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="返回选择页面" />
                <br />
                <asp:SqlDataSource ID="FoodType" runat="server"
                    ConnectionString="<%$ ConnectionStrings:orderfoodConnectionString %>"
                    SelectCommand="SELECT [OrderType], [OrderMan], [Date] FROM [OrderType]"></asp:SqlDataSource>

                <asp:Label ID="lbMenu" runat="server" Text="菜单"></asp:Label>
                <asp:Image ID="ImgMenu" runat="server" Style="width: 100%" /><br />
                <div style="float: left">
                    <asp:Label ID="lbLog" runat="server" Text="说明：1.首先我们需要一个人成为本日的负责人，否则无法点餐，请点击上方的“成为今天的点餐负责人”！
                                                          2.点了餐的人到负责人处付钱，由负责人收钱、在11点时根据点餐信息打电话订餐并付款。（系统还在试运行，有什么BUG请联系小贝修改）"></asp:Label>
                </div>

            </div>
            <div style="float: left">
                <asp:Label ID="lbMoney" runat="server" Text="总钱数"></asp:Label>
                <asp:TextBox ID="tbMoney" runat="server" ReadOnly="True" Width="64px"></asp:TextBox>
                <asp:GridView ID="GridView2" runat="server">
                </asp:GridView>
            </div>


        </div>

    </form>



</body>
</html>

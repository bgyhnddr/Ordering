using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Web.Security;

namespace OrderFood
{
    public partial class Main : System.Web.UI.Page
    {
        enum au
        {
            admin,
            orderman
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Type"] == null || GetFoodType().Select("OrderType='" + Request.QueryString["Type"] + "'").Length == 0)
            {
                Response.Redirect("Error.aspx?Type=" + Request.QueryString["Type"]);
            }

            if (CheckOverTime() && !HasAuth(au.orderman, Request.QueryString["Type"]))
            {
                Response.Redirect("Error.aspx?Type=" + Request.QueryString["Type"]);
            }

            var begintime = DateTime.Now.ToString("yyyy-MM-dd");
            var endtime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            orderfoodlist.FilterExpression = string.Format("Date > '{0}' AND Date < '{1}' AND Type = '{2}'", begintime, endtime, Request.QueryString["Type"]);


            GridView2.DataSource = GetTodayInfo(Request.QueryString["Type"]);
            GridView2.DataBind();

            if (!IsPostBack)
            {
                init();
                SetAuth();
            }
        }

        private void init()
        {
            this.tbUserName.Text = GetName();
            lbType.Text = Request.QueryString["Type"];
            ImgMenu.ImageUrl = lbType.Text + ".jpg";
            tbFoodType.Text = lbType.Text;
            tbOrderMan.Text = GetOrderMan(Request.QueryString["Type"]);


        }

        private void SetAuth()
        {
            GridView1.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            lbType.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            lbOrderFood.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            tbOrderFood.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            lbNumber.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            tbNumber.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            btOrder.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            lbMenu.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            lbPay.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;
            tbPay.Visible = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? false : true;



            btSetType.Visible = tbFoodType.Visible = HasAuth(au.orderman, Request.QueryString["Type"]);
            tbUserName.ReadOnly = !HasAuth(au.admin);
            tbOrderMan.ReadOnly = !HasAuth(au.admin);
            btChangeOrderMan.Visible = HasAuth(au.admin);
            btBeOrderMan.Enabled = GetOrderMan(Request.QueryString["Type"]) == string.Empty ? true : false;
        }

        private string GetName()
        {
            //var UserHostAddress = Request.UserHostAddress;
            //IPHostEntry myHost = new IPHostEntry();
            //myHost = Dns.GetHostEntry(UserHostAddress);
            if (Context.User.Identity.IsAuthenticated)
            {
                return Context.User.Identity.Name;
            }
            else
            {
                return string.Empty;
            }

        }

        private bool HasAuth(au type,string foodType = null)
        {
            switch (type)
            {
                case au.admin:

                    if (GetName().IndexOf(ConfigurationManager.AppSettings["Admin"]) >= 0)
                    {
                        return true;
                    }
                    break;
                case au.orderman:
                    if (GetName() == GetOrderMan(foodType) || HasAuth(au.admin))
                    {
                        return true;
                    }
                    break;
            }
            
            return false;
        }

        private string GetOrderMan(string foodType)
        {
            var dt = new DataTable();
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT OrderMan FROM OrderType WHERE CONVERT(VARCHAR(10),Date,120)=CONVERT(VARCHAR(10),GETDATE(),120) AND [OrderType] = '" + foodType + "'", conn);
            adapter.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                return string.Empty;
            }
            
            return dt.Rows[0][0].ToString();
        }

        private void SetOrderMan(string name,string foodType)
        {
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("UPDATE OrderType SET OrderMan = '" + name + "', Date = '" + DateTime.Now + "' WHERE [OrderType] = '" + foodType + "'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            tbOrderMan.Text = name;
        }

        private bool CheckOverTime()
        {
            //if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 11)
            //{
            //    return false;
            //}
            //return true;
            return false;
        }

        private void SetFoodType(string type)
        {
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("UPDATE TodayType SET OrderTypeToday = '" + type + "'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();


            lbType.Text = type;
            ImgMenu.ImageUrl = lbType.Text + ".jpg";
        }

        private DataTable GetFoodType()
        {
            var dt = new DataTable();
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM OrderType", conn);
            adapter.Fill(dt);
            return dt;
        }

        private void OrderFood()
        {
            var foodtype = Request.QueryString["Type"];
            var user = HasAuth(au.orderman, Request.QueryString["Type"]) ? tbUserName.Text : GetName();
            var orderfood = tbOrderFood.Text;
            var number = 1;
            try
            {
                number = int.Parse(tbNumber.Text);
            }
            catch
            {
            }
            var pay = 0;
            try
            {
                pay = int.Parse(tbPay.Text);
            }
            catch { }

            if (string.IsNullOrWhiteSpace(orderfood))
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('点餐内为空！');</script>");
                return;
            }
            else
            {
                var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(string.Format("INSERT INTO [orderfood].[dbo].[OrderList]([Name],[Type],[OrderFood],[Number],[Money])VALUES('{0}','{1}','{2}',{3},{4})", user, foodtype, orderfood, number, pay), conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                GridView1.DataBind();
            }
        }

        private decimal GetTodaySum(string foodType)
        {
            var dt = new DataTable();
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT SUM(Money) FROM OrderList WHERE CONVERT(VARCHAR(10),Date,120)=CONVERT(VARCHAR(10),GETDATE(),120) AND [Type] = '" + foodType + "'", conn);
            adapter.Fill(dt);
            if (string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString()))
            {
                return 0;
            }
            return decimal.Parse(dt.Rows[0][0].ToString());
        }

        private DataTable GetTodayInfo(string foodType)
        {
            var dt = new DataTable();
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT OrderFood AS 餐名,SUM(Number) AS 数量 FROM dbo.OrderList WHERE CONVERT(VARCHAR(10),Date,120)=CONVERT(VARCHAR(10),GETDATE(),120) AND [Type] = '" + foodType + "' GROUP BY OrderFood", conn);
            adapter.Fill(dt);

            return dt;
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //((BoundField)GridView1.Columns[5]).ReadOnly = !HasAuth(au.orderman, Request.QueryString["Type"]);
        }

        protected void btSetType_Click(object sender, EventArgs e)
        {
            SetFoodType(tbFoodType.Text);
        }

        protected void btOrder_Click(object sender, EventArgs e)
        {
            OrderFood();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((Button)e.Row.FindControl("btEdit")) != null)
                {
                    if ((e.Row.Cells[1].Text == GetName()) || HasAuth(au.orderman, Request.QueryString["Type"]))
                    {
                        ((Button)e.Row.FindControl("btEdit")).Enabled = ((Button)e.Row.FindControl("btDelete")).Enabled = true;
                        ((Button)e.Row.FindControl("btEdit")).Visible = ((Button)e.Row.FindControl("btDelete")).Visible = true;
                    }
                    else
                    {
                        ((Button)e.Row.FindControl("btEdit")).Enabled = ((Button)e.Row.FindControl("btDelete")).Enabled = false;
                        ((Button)e.Row.FindControl("btEdit")).Visible = ((Button)e.Row.FindControl("btDelete")).Visible = false;
                    }
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void btBeOrderMan_Click(object sender, EventArgs e)
        {
            if (GetOrderMan(Request.QueryString["Type"]) == string.Empty)
            {
                SetOrderMan(GetName(), Request.QueryString["Type"]);
                SetAuth();
            }
            else
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('点餐人已存在！');</script>");
            }
        }

        protected void btChangeOrderMan_Click(object sender, EventArgs e)
        {
            SetOrderMan(tbOrderMan.Text, Request.QueryString["Type"]);
            SetAuth();
        }

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView2.DataSource = GetTodayInfo(Request.QueryString["Type"]);
            GridView2.DataBind();
            tbMoney.Text = GetTodaySum(Request.QueryString["Type"]).ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Selector.aspx");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
    }
}
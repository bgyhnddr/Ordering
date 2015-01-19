using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;

namespace OrderFood
{
    public partial class Selector : System.Web.UI.Page
    {
        enum au
        {
            admin,
            orderman
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateBegin.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                DateEnd.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");
                OrderList.SelectCommand = "SELECT [Name], SUM([Money]) AS Money FROM [OrderList] WHERE 1=2 GROUP BY [Name]";
                castList.SelectCommand = "SELECT [Name], [OrderFood], [Number], [Money], [Date] FROM [OrderList] WHERE 1=2";
                lbHello.Text = "Hello!" + GetName();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ListBox1.SelectedValue))
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('请先选择餐类！');</script>");
            }
            else
            {
                Response.Redirect("Main.aspx?Type=" + ListBox1.SelectedValue);
            }
        }

        private string GetName()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                return Context.User.Identity.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        private bool HasAuth(au type, string foodType = null)
        {
            switch (type)
            {
                case au.admin:

                    if (GetName().IndexOf(ConfigurationManager.AppSettings["Admin"]) >= 0)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void search_Click(object sender, EventArgs e)
        {
            string begin, end;
            try
            {
                begin = DateTime.Parse(DateBegin.Text).ToString("yyyy-MM-dd");
            }
            catch
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('开始日期格式错误，应该为(yyyy-MM-dd)！');</script>");
                return;
            }

            try
            {
                end = DateTime.Parse(DateEnd.Text).AddDays(1).ToString("yyyy-MM-dd");
            }
            catch
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('结束日期格式错误，应该为(yyyy-MM-dd)！');</script>");
                return;
            }

            OrderList.SelectCommand = "SELECT [Name], SUM([Money]) AS Money FROM [OrderList] WHERE DATE>'" + begin + "' AND DATE<'" + end + "' GROUP BY [Name]";
            GridView1.DataBind();

            castList.SelectCommand = "SELECT [Name], [OrderFood], [Number], [Money], [Date] FROM [OrderList] WHERE DATE>'" + begin + "' AND DATE<'" + end + "' AND Name ='" + GetName() + "'";
        }
    }
}
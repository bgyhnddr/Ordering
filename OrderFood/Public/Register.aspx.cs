using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace OrderFood.Public
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int result = 0;
            var dt = new DataTable();
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);

            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [User] Where Account = '" + tbAccount.Text + "'", conn);
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('帐号已存在');</script>");
            }
            else
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [User]([Account],[Password]) VALUES ('" + tbAccount.Text + "','" + FormsAuthentication.HashPasswordForStoringInConfigFile(tbPassword.Text, "MD5") + "')", conn);

                result = cmd.ExecuteNonQuery();
            }
            conn.Close();
            if (result > 0)
            {
                FormsAuthentication.SetAuthCookie(tbAccount.Text, true);
                Response.Redirect("Main.html");
            }
        }

        protected void btnCencel_Click(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}
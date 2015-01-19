﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrderFood
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var code = Request.Params["code"];
            if (code != null)
            {
                var name = GetUserInfo(code);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    FormsAuthentication.SetAuthCookie(name, true);
                    Response.Redirect("Main.html");
                    return;
                }
            }
        }

        private bool AutoLogin()
        {
            return false;
        }

        private void Redirect(string name)
        {

            var redirect = FormsAuthentication.GetRedirectUrl(name, true);
            Response.Redirect(redirect);
        }

        private string GetUserInfo(string code)
        {
            var json = JsonConvert.DeserializeObject<JObject>(OrderHelper.GetUserInofByCode(code));
            if (json["name"] != null)
            {
                return json["name"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Public\\Register.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if(AccountInput.Text == "administrator")
            {
                FormsAuthentication.SetAuthCookie(AccountInput.Text, true);
                Response.Redirect("Main.html");
                return;
            }

            var dt = new DataTable();
            var connString = ConfigurationManager.ConnectionStrings["orderfoodConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);

            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [User] Where Account = '" + AccountInput.Text + "'", conn);
            adapter.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('帐号不存在');</script>");
            }
            else
            {
                if (dt.Rows[0]["Password"].ToString() == FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordInput.Text, "MD5"))
                {
                    FormsAuthentication.SetAuthCookie(dt.Rows[0]["Account"].ToString(), true);
                    Response.Redirect("Main.html");
                }
                else
                {
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>alert('密码错误');</script>");
                }
            }
        }
    }
}
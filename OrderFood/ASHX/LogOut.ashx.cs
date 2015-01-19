using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace OrderFood.ASHX
{
    /// <summary>
    /// Logout 的摘要说明
    /// </summary>
    public class LogOut : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            FormsAuthentication.SignOut();
            context.Response.Redirect("~/Login.aspx");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// UploadMenu 的摘要说明
    /// </summary>
    public class UploadMenu : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.UploadMenu(context));
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// UpdateComment 的摘要说明
    /// </summary>
    public class UpdateComment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.UpdateComment(context));
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
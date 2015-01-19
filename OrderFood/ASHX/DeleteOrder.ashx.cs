using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// DeleteOrder 的摘要说明
    /// </summary>
    public class DeleteOrder : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.DeleteOrder(context));
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
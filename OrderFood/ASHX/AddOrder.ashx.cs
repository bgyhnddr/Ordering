using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// AddOrder 的摘要说明
    /// </summary>
    public class AddOrder : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.AddOrder(context));
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
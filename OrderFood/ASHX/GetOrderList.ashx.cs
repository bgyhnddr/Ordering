using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// GetOrderList 的摘要说明
    /// </summary>
    public class GetOrderList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.GetOrderList("午餐"));
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
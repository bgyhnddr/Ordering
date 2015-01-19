using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// UpdatePrice 的摘要说明
    /// </summary>
    public class UpdatePrice : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.UpdatePrice(context));
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
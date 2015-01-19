using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// UpdatePay 的摘要说明
    /// </summary>
    public class UpdatePay : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.UpdatePay(context));
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
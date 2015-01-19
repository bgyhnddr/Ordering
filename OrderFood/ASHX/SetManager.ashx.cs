using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// SetManager 的摘要说明
    /// </summary>
    public class SetManager : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.SetManager(context));
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
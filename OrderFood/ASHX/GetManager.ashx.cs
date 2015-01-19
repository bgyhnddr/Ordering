using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// GetManager 的摘要说明
    /// </summary>
    public class GetManager : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(OrderAshxHelper.GetManager());
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
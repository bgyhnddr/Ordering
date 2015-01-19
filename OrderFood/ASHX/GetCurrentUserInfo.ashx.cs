using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OrderFood.ASHX
{
    /// <summary>
    /// GetCurrentUserInfo 的摘要说明
    /// </summary>
    public class GetCurrentUserInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.Write(OrderAshxHelper.GetUserInfo(context.User.Identity.Name));
            }
            catch (Exception ex)
            {
                context.Response.Write(ResponseJsonHelper.GetResponseString(false, "发生错误:" + Regex.Replace(ex.Message, @"\r\n", " ")));
            }
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
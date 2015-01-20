using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OrderFood
{
    public class OrderAshxHelper
    {
        public static string GetUserInfo(string name, string url = "")
        {
            var table = new DataTable();
            table.Columns.Add("Account", typeof(string));
            table.Columns.Add("url", typeof(string));
            table.Columns.Add("auth", typeof(int));
            var row = table.NewRow();
            row["Account"] = name;
            row["auth"] = OrderGlobalHelper.GetAuth(name);
            row["url"] = url;
            table.Rows.Add(row);
            return ResponseJsonHelper.GetResponseString(true, string.Empty, ResponseJsonHelper.GetJSONString(table));
        }

        public static string GetOrderList(string type)
        {
            var table = OrderDataBaseHelper.GetOrderList(type);
            return ResponseJsonHelper.GetResponseString(true, string.Empty, ResponseJsonHelper.GetJSONString(table));
        }

        public static string GetManager()
        {
            var manager = OrderDataBaseHelper.GetOrderManager("午餐");
            return ResponseJsonHelper.GetResponseString(true, string.Empty, ResponseJsonHelper.GetJSONString(manager));
        }

        public static string UploadMenu(HttpContext context)
        {
            var user = context.User.Identity.Name;
            if (OrderGlobalHelper.GetAuth(user) < 2)
            {
                return "没有权限";
            }

            if (OrderGlobalHelper.UploadMenu(context))
            {
                return "上传成功";
            }
            else
            {
                return "上传失败";
            }
        }

        public static string GetMenu(HttpContext context)
        {
            var dataSet = OrderGlobalHelper.GetMenu(context);
            return ResponseJsonHelper.GetResponseString(true, string.Empty, ResponseJsonHelper.GetJSONString(dataSet));
        }

        public static string UpdatePay(HttpContext context)
        {
            try
            {
                var user = context.User.Identity.Name;
                var id = context.Request.Params["id"];
                var pay = context.Request.Params["pay"];
                if (OrderGlobalHelper.GetAuth(user) < 2)
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有权限");
                }

                if (OrderDataBaseHelper.UpdatePay(int.Parse(id), float.Parse(pay)))
                {
                    return ResponseJsonHelper.GetResponseString(true);
                }
                else
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有更新");
                }
            }
            catch(Exception ex)
            {
                return ResponseJsonHelper.GetResponseString(false, "更新失败:" + Regex.Replace(ex.Message, @"\r\n"," "));
            }
        }

        public static string UpdateComment(HttpContext context)
        {
            try
            {
                var user = context.User.Identity.Name;
                var id = context.Request.Params["id"];
                var comment = context.Request.Params["comment"];
                if (OrderGlobalHelper.GetAuth(user, int.Parse(id)) < 1)
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有权限");
                }

                if (OrderDataBaseHelper.UpdateComment(int.Parse(id), comment))
                {
                    return ResponseJsonHelper.GetResponseString(true);
                }
                else
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有更新");
                }
            }
            catch (Exception ex)
            {
                return ResponseJsonHelper.GetResponseString(false, "更新失败:" + Regex.Replace(ex.Message, @"\r\n", " "));
            }
        }

        public static string DeleteOrder(HttpContext context)
        {
            try
            {
                var user = context.User.Identity.Name;
                var id = context.Request.Params["id"];
                if (OrderGlobalHelper.GetAuth(user, int.Parse(id)) < 1)
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有权限");
                }

                if (OrderDataBaseHelper.DeleteOrder(int.Parse(id)))
                {
                    return ResponseJsonHelper.GetResponseString(true);
                }
                else
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有更新");
                }
            }
            catch (Exception ex)
            {
                return ResponseJsonHelper.GetResponseString(false, "更新失败:" + Regex.Replace(ex.Message, @"\r\n", " "));
            }
        }

        public static string SetManager(HttpContext context)
        {
            try
            {
                var user = context.User.Identity.Name;
                var manager = context.Request.Params["manager"];
                var type = "午餐";
                var hasManager = !string.IsNullOrEmpty(OrderDataBaseHelper.GetOrderManager(type));
                if (hasManager)
                {
                    if (OrderGlobalHelper.GetAuth(user) < 3)
                    {
                        return ResponseJsonHelper.GetResponseString(false, "没有权限");
                    }
                    else
                    {
                        if (OrderDataBaseHelper.UpdateManager(manager, type))
                        {
                            return ResponseJsonHelper.GetResponseString(true);
                        }
                    }
                }
                else
                {
                    if (OrderDataBaseHelper.InsertManager(user, type))
                    {
                        return ResponseJsonHelper.GetResponseString(true);
                    }
                }

                return ResponseJsonHelper.GetResponseString(false, "设定失败");
            }
            catch (Exception ex)
            {
                return ResponseJsonHelper.GetResponseString(false, "更新负责人失败:" + Regex.Replace(ex.Message, @"\r\n", " "));
            }
        }

        public static string AddOrder(HttpContext context)
        {
            try
            {
                var user = context.User.Identity.Name;
                var store = context.Request.Params["store"];
                var food = context.Request.Params["food"];
                var price = context.Request.Params["price"];

                var auth = OrderGlobalHelper.GetAuth(user);

                var time = DateTime.Parse(ConfigurationManager.AppSettings["LimitTime"]);

                if (auth > 1)
                {
                    if (OrderDataBaseHelper.AddOrder("午餐", user, store, food, price))
                    {
                        return ResponseJsonHelper.GetResponseString(true);
                    }
                }
                else if (DateTime.Now > time)
                {
                    return ResponseJsonHelper.GetResponseString(false, "超过了可点餐时间");
                }
                else if (string.IsNullOrWhiteSpace(OrderDataBaseHelper.GetOrderManager("午餐")))
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有午餐负责人无法点餐");
                }
                else
                {
                    if (OrderDataBaseHelper.AddOrder("午餐", user, store, food, price))
                    {
                        return ResponseJsonHelper.GetResponseString(true);
                    }
                }

                return ResponseJsonHelper.GetResponseString(false, "点餐失败");

            }
            catch (Exception ex)
            {
                return ResponseJsonHelper.GetResponseString(false, "点餐失败:" + Regex.Replace(ex.Message, @"\r\n", " "));
            }
        }

        internal static string UpdatePrice(HttpContext context)
        {
            try
            {
                var user = context.User.Identity.Name;
                var id = context.Request.Params["id"];
                var price = context.Request.Params["price"];
                if (OrderGlobalHelper.GetAuth(user) < 2)
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有权限");
                }

                if (OrderDataBaseHelper.UpdatePrice(int.Parse(id), float.Parse(price)))
                {
                    return ResponseJsonHelper.GetResponseString(true);
                }
                else
                {
                    return ResponseJsonHelper.GetResponseString(false, "没有更新");
                }
            }
            catch (Exception ex)
            {
                return ResponseJsonHelper.GetResponseString(false, "更新失败:" + Regex.Replace(ex.Message, @"\r\n", " "));
            }
        }
    }
}
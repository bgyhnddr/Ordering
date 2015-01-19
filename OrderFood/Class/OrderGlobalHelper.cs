using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace OrderFood

{
    public class OrderGlobalHelper
    {
        public static bool IsAdmin(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            string Admin = ConfigurationManager.AppSettings["Admin"];


            return Admin.Split(',').Where(user => user == name).Count() > 0;
        }

        public static bool UploadMenu(HttpContext context)
        {
            try
            {
                if (context.Request.Files["xls"] != null)
                {
                    context.Request.Files["xls"].SaveAs(HttpContext.Current.Server.MapPath("~") + "\\PUBLIC\\" + "menu.xls");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataSet GetMenu(HttpContext context)
        {
            var dataSet = new DataSet();
            var path = context.Request.PhysicalApplicationPath + "\\PUBLIC\\menu.xls";

            string excelPath = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
            var conn = new OleDbConnection(excelPath);
            try
            {
                if (File.Exists(path))
                {

                    var menuAdapter = new OleDbDataAdapter("SELECT * FROM [menu$]", conn);
                    var typeAdapter = new OleDbDataAdapter("SELECT * FROM [type$]", conn);
                    conn.Open();
                    menuAdapter.Fill(dataSet, "menu");
                    typeAdapter.Fill(dataSet, "type");
                }
            }
            catch { }
            finally { conn.Close(); }

            return dataSet;
        }

        public static int GetAuth(string name, int id = -1)
        {
            if (IsAdmin(name))
            {
                return 3;
            }
            else if (OrderDataBaseHelper.GetOrderManager("午餐") == name)
            {
                return 2;
            }
            else if (OrderDataBaseHelper.IsUserOrder(name, id))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
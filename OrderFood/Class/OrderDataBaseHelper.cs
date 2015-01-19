using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace OrderFood
{
    public class OrderDataBaseHelper
    {
        public static string appConn = "orderfoodConnectionStringOle";

        public static DataTable ExecSqlDataTable(OleDbConnection connection, OleDbTransaction transaction, string sql, params CParam[] pars)
        {
            DataTable table = new DataTable();
            var conn = connection;
            if (conn == null)
            {
                conn = GetConn();
            }

            var tran = transaction;

            try
            {
                var comm = new OleDbCommand(sql, conn);
                foreach (var par in pars)
                {
                    comm.Parameters.AddWithValue(par.name, par.value);
                }
                if (tran != null)
                {
                    comm.Transaction = tran;
                }
                var adapter = new OleDbDataAdapter(comm);
                adapter.Fill(table);

                if (tran != null)
                {
                    tran.Commit();
                }
            }
            catch
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
            }
            finally
            {
                conn.Close();
            }

            return table;
        }

        public static DataTable ExecSqlDataTable(string sql, params CParam[] pars)
        {
            return ExecSqlDataTable(null, null, sql, pars);
        }

        public static OleDbConnection GetConn()
        {
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings[appConn].ConnectionString);
            return con;
        }

        public static DataTable GetUserInfo(string name)
        {
            return ExecSqlDataTable(
                "SELECT [Account] FROM [orderfood].[dbo].[User] WHERE [Account] = ?",
                new CParam() { name = "name", value = name }
                );
        }

        public static string GetOrderManager(string type)
        {
            var table = ExecSqlDataTable("SELECT * FROM [orderfood].[dbo].[SumReport] WHERE [Type]=?",
                        new CParam() { name = "Type", value = type }
                        );
            if (table.Rows.Count > 0)
            {
                return table.Rows[0]["OrderMan"].ToString();
            }
            return string.Empty;
        }

        public static DataSet GetOrderList(string type)
        {
            var dataSet = new DataSet();
            if (type != null)
            {
                dataSet.Tables.Add(ExecSqlDataTable("SELECT * FROM [orderfood].[dbo].[OrderListView] WHERE [Type]=? AND CONVERT(CHAR(10),[Date],120) = CONVERT(CHAR(10),GETDATE(),120)",
                        new CParam() { name = "Type", value = type }
                        ));
                dataSet.Tables.Add(ExecSqlDataTable("SELECT * FROM [orderfood].[dbo].[SumReport] WHERE [Type]=?",
                        new CParam() { name = "Type", value = type }
                        ));
            }
            else
            {
                dataSet.Tables.Add(ExecSqlDataTable("SELECT * FROM [orderfood].[dbo].[OrderListView] WHERE CONVERT(CHAR(10),[Date],120) = CONVERT(CHAR(10),GETDATE(),120)"));
                dataSet.Tables.Add(ExecSqlDataTable("SELECT * FROM [orderfood].[dbo].[SumReport]"));
            }
            return dataSet;
        }

        public static bool UpdatePay(int id, float pay)
        {
            var table = ExecSqlDataTable("UPDATE [orderfood].[dbo].[OrderList] SET [Pay] = ? WHERE id=?",
                         new CParam() { name = "Pay", value = pay },
                         new CParam() { name = "id", value = id }
                         );
            return true;
        }

        public static bool DeleteOrder(int id)
        {
            var table = ExecSqlDataTable("DELETE FROM [orderfood].[dbo].[OrderList] WHERE id=?",
                         new CParam() { name = "id", value = id }
                         );
            return true;
        }

        public static bool UpdateComment(int id, string comment)
        {
            var table = ExecSqlDataTable("UPDATE [orderfood].[dbo].[OrderList] SET [Comment] = ? WHERE id=?",
                         new CParam() { name = "Comment", value = comment },
                         new CParam() { name = "id", value = id }
                         );
            return true;
        }

        public static bool IsUserOrder(string name, int id)
        {
            ExecSqlDataTable("SELECT * FROM [orderfood].[dbo].[OrderList] WHERE [id]=? AND [Name]=? ",
                         new CParam() { name = "id", value = id },
                         new CParam() { name = "name", value = name }
                         );
            return true;
        }


        public static bool UpdateManager(string manager, string type)
        {
            ExecSqlDataTable("UPDATE [orderfood].[dbo].[OrderType] SET [OrderMan] = ? WHERE [OrderType]=? AND CONVERT(CHAR(10),[Date],120) = CONVERT(CHAR(10),GETDATE(),120)",
                         new CParam() { name = "manager", value = manager },
                         new CParam() { name = "type", value = type }
                         );
            return true;
        }
        public static bool InsertManager(string manager,string type)
        {
            ExecSqlDataTable("INSERT INTO [orderfood].[dbo].[OrderType]([OrderType],[OrderMan],[Date]) VALUES (?,?,GETDATE())",
                         new CParam() { name = "OrderType", value = type },
                         new CParam() { name = "OrderMan", value = manager }
                         );
            return true;
        }

        internal static bool AddOrder(string type,string user, string store, string food, string price)
        {
            ExecSqlDataTable("INSERT INTO [orderfood].[dbo].[OrderList]([Name],[Type],[Store],[OrderFood],[Money],[Date])VALUES(?,?,?,?,?,GETDATE())",
                         new CParam() { name = "name", value = user },
                         new CParam() { name = "type", value = type },
                         new CParam() { name = "store", value = store },
                         new CParam() { name = "food", value = food },
                         new CParam() { name = "price", value = price }
                         );
            return true;
        }

        internal static bool UpdatePrice(int id, float price)
        {
            var table = ExecSqlDataTable("UPDATE [orderfood].[dbo].[OrderList] SET [Money] = ? WHERE id=?",
                         new CParam() { name = "Money", value = price },
                         new CParam() { name = "id", value = id }
                         );
            return true;
        }
    }

    public class CParam
    {
        public string name;
        public object value;
    }
}
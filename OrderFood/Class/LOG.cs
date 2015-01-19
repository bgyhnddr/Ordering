﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// LOG 的摘要说明
/// </summary>
public class LOG
{
    public static void Log(string log)
    {
        File.AppendAllText(HttpContext.Current.Server.MapPath("~") + "\\log.txt", "\r\n" + log + "\r\n", Encoding.UTF8);
    }

    public static void SaveAccessToken(string obj, string url = "")
    {
        var o = JsonConvert.DeserializeObject<JObject>(obj);
        o["getTime"] = DateTime.Now.ToString();
        obj = JsonConvert.SerializeObject(o);
        File.WriteAllText(HttpContext.Current.Server.MapPath("~") + url + "\\access_token", obj, Encoding.UTF8);
    }

    public static string GetSavedAccessToken(string url = "")
    {
        try
        {
            var obj = File.ReadAllText(HttpContext.Current.Server.MapPath("~") + url + "\\access_token", Encoding.UTF8);
            var o = JsonConvert.DeserializeObject<JObject>(obj);
            var date = DateTime.Parse(o["getTime"].ToString());
            var expires = int.Parse(o["expires_in"].ToString()) * 3 / 4;

            var span = new TimeSpan(0, 0, expires);
            if (DateTime.Now - date > span)
            {
                return string.Empty;
            }
            else
            {
                return o["access_token"].ToString();
            }
        }
        catch
        {
            return string.Empty;
        }
    }

    public static APPIS GetAppIS()
    {
        var obj = File.ReadAllText(HttpContext.Current.Server.MapPath("~") + "\\APPIS", Encoding.UTF8);
        var appIS = JsonConvert.DeserializeObject<APPIS>(obj);
        return appIS;
    }
    public static void SaveSuiteTicket(string obj, string url = "")
    {
        File.WriteAllText(HttpContext.Current.Server.MapPath("~") + url + "\\suite_ticket", obj, Encoding.UTF8);
    }
}

public class APPIS
{
    public string appid;
    public string secret;
}
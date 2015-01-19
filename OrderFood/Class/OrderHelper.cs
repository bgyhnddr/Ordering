using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

/// <summary>
/// OrderHelper 的摘要说明
/// </summary>
public class OrderHelper
{
    public OrderHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public static string GetUserInofByCode(string code)
    {
        return GetUserInfo(GetUserId(code));
    }

    public static string GetAccessToken()
    {
        var url = "";
        var token = LOG.GetSavedAccessToken(url);
        if (string.IsNullOrWhiteSpace(token))
        {
            var appIS = LOG.GetAppIS();

            var resObj = JsonConvert.DeserializeObject<JObject>(WXRequest.SendRequest(
                string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", appIS.appid, appIS.secret)));

            LOG.SaveAccessToken(JsonConvert.SerializeObject(resObj), url);

            return resObj["access_token"].ToString();

        }
        return token;
    }

    public static string GetUserId(string code)
    {
        var resObj = JsonConvert.DeserializeObject<JObject>(WXRequest.SendRequest(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}&agentid=2",
            GetAccessToken(), code)));

        return resObj["UserId"] != null ? resObj["UserId"].ToString() : string.Empty;
    }

    public static string GetUserInfo(string userId)
    {
        var returnStr = WXRequest.SendRequest(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}",
            GetAccessToken(), userId));
        return returnStr;
    }

}
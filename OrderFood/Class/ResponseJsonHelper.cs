using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrderFood
{
    public class ResponseJsonHelper
    {
        public static JObject GetJObject(string obj)
        {
            return JsonConvert.DeserializeObject<JObject>(obj);
        }

        public static string GetJSONString(object obj)
        {
            if (obj is string)
            {
                return "\"" + obj + "\"";
            }

            return JsonConvert.SerializeObject(obj);
        }

        public static string GetResponseString(bool success, string message = "", string data = "{}")
        {
            return "{\"success\":" + (success ? "true" : "false") + ",\"data\":" + data + ",\"message\":\"" + message + "\"}";
        }
    }

}
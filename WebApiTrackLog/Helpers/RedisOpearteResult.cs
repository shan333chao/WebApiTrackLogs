using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTrackLog.Helpers
{
    public class RedisOpearteResult
    {
        public string token { get; set; }
        public bool isok { get; set; }
        public int code { get; set; }
        public object data { get; set; } 
        public string result { get; set; }

    }

    public static class RedisHashEnum
    {

        public static string userinfo = "userinfo";

    }
}

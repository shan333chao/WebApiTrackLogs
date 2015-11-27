using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTrackLog.WebApiAttributes;

namespace WebApiTrackLog.Areas.ShowDemo.Controllers
{
    /// <summary>
    /// 该Controller 下的所有action 都不会被全局的OperateTrack Filter 拦截
    /// </summary>
    [NoLog]
    public class UserManagerController : ApiController
    {
        public List<string> GetUsers() {
            return new List<string>() { "tomers","jack"};
        }
        public  string GiveUserSomeMoney(int money)
        {
            return money+"";
        }
    }
}

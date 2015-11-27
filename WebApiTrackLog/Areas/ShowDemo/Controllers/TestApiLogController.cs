using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiTrackLog.Areas.ShowDemo.Models;
using WebApiTrackLog.WebApiAttributes;

namespace WebApiTrackLog.Areas.ShowDemo.Controllers
{
    /// <summary>
    /// 记录该类中的Action内容
    /// </summary>
    [OperateTrack]
    public class TestApiLogController : ApiController
    {
        [HttpPost]
        public object Login(UserInfo user)
        {
            var result = new { data = user, status = true };
            return result;
        }
        /// <summary>
        /// 该类不参与记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [NoLog]
        public string DontLogMe(string name)
        {

            return name;
        }
    }
}

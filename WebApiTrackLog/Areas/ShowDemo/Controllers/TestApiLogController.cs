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

    public class TestApiLogController : ApiController
    {        [OperateTrack]
        [HttpPost]

        public object Login(UserInfo user)
        {

            var result = new { data = user, status = true };


            return  result;

        }

 


    }
}

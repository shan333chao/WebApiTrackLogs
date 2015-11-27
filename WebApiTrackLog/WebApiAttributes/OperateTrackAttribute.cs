using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApiTrackLog.Models;

namespace WebApiTrackLog.WebApiAttributes
{
    public class OperateTrackAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 自定义参数
        /// </summary>
        public string msg { get; set; }
        public OperateTrackAttribute()
        {

        }


        /// <summary>
        /// 初始化时填入类的说明
        /// </summary>
        /// <param name="message"></param>
        public OperateTrackAttribute(string message)
        {
            msg = message;
        }


        private static readonly string key = "enterTime";
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (SkipLogging(actionContext))
            {
                return base.OnActionExecutingAsync(actionContext, cancellationToken);

            }
            //记录进入请求的时间
            actionContext.Request.Properties[key] = DateTime.Now.ToBinary();

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
        /// <summary>
        /// 在请求执行完后 记录请求的数据以及返回数据
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            object beginTime = null;
            if (actionExecutedContext.Request.Properties.TryGetValue(key, out beginTime))
            {
                DateTime time = DateTime.FromBinary(Convert.ToInt64(beginTime));
                HttpRequest request = HttpContext.Current.Request;
                string token = request.Headers["token"];

                WepApiActionLog apiActionLog = new WepApiActionLog
                {
                    Id = Guid.NewGuid(),
                    ///获取action名称
                    actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                    ///获取Controller 名称
                    controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    ///获取action开始执行的时间
                    enterTime = time,
                    ///获取执行action的耗时
                    costTime = (DateTime.Now - time).TotalMilliseconds,
                    navigator = request.UserAgent,
                    token = token,
                    ///获取用户token
                    userId = getUserByToken(token),
                    ///获取访问的ip
                    ip = request.UserHostAddress,
                    userHostName = request.UserHostName,
                    urlReferrer = request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "",
                    browser = request.Browser.Browser + " - " + request.Browser.Version + " - " + request.Browser.Type,
                    ///获取request提交的参数
                    paramaters = GetRequestValues(actionExecutedContext),//JsonConvert.SerializeObject(request.QueryString),//JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionDescriptor.GetParameters()),
                    //获取response响应的结果
                    executeResult = GetResponseValues(actionExecutedContext),// "",//JsonConvert.SerializeObject(actionExecutedContext.Response.RequestMessage),
                    comments = msg,
                    RequestUri = request.Url.AbsoluteUri
                };
                using (TrackLogEntities context = new TrackLogEntities())
                {
                    context.WepApiActionLogs.Add(apiActionLog);
                    context.SaveChanges();
                }
            }
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

        }
        /// <summary>
        /// 获取当前登录用户的id
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int getUserByToken(string token)
        {
            UserInfo user = null;
            // TokenManager.getUserByToken(token, out user);
            return user == null ? 0 : user.user_id;
        }
        /// <summary>
        /// 读取request 的提交内容
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetRequestValues(HttpActionExecutedContext actionExecutedContext)
        {

            Stream stream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            /*
                这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
                因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0;
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }

        /// <summary>
        /// 读取action返回的result
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetResponseValues(HttpActionExecutedContext actionExecutedContext)
        {
            Stream stream = actionExecutedContext.Response.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            /*
            这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
            因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0; 
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }
        /// <summary>
        /// 判断类和方法头上的特性是否要进行Action拦截
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool SkipLogging(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<NoLogAttribute>().Any() || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<NoLogAttribute>().Any();
        }

    }

}
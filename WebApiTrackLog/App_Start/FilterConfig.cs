using System.Web;
using System.Web.Mvc;
using WebApiTrackLog.MvcAttributes;

namespace WebApiTrackLog
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //注册mvc的ActionFilterAttribute 
            filters.Add(new MvcCrosAttribute());
        }
    }
}

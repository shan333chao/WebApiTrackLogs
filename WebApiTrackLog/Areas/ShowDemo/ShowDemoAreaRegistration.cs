using System.Web.Mvc;

namespace WebApiTrackLog.Areas.ShowDemo
{
    public class ShowDemoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ShowDemo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ShowDemo_default",
                "ShowDemo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
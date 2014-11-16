namespace CallCenterCrm.Web.Areas.Manage
{
    using System.Web.Mvc;

    public class ManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Statistics",
                "Manager/Statistics/{action}/{id}",
                new { controller = "Statistics", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CallCenterCrm.Web.Areas.Manager.Controllers" });
            
            context.MapRoute(
                "Manager_default",
                "Manager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}

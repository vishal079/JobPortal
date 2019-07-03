using System.Web.Mvc;

namespace JobPortal.Areas.Job
{
    public class JobAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Job";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Job_default",
                "Job/{controller}/{action}/{id}",
                new { Controller = "JobSearch", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

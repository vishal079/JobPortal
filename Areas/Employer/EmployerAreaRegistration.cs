using System.Web.Mvc;

namespace JobPortal.Areas.Employer
{
    public class EmployerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Employer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Employer_default",
                "Employer/{controller}/{action}/{id}",
                new { Controller="EmployerAccount",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

using System.Web.Mvc;

namespace JobPortal.Areas.Candidate
{
    public class CandidateAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Candidate";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Candidate_default",
                "Candidate/{controller}/{action}/{id}",
                new { Controller="CandidateSearch", action = "CandidateList", id = UrlParameter.Optional }
            );
        }
    }
}

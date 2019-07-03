using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Core;
using JobPortal.Core.Domain;
using JobPortal.Core.Repository;
using JobPortal.Web;

namespace JobPortal.Areas.Candidate.Controllers
{
    public class CandidateSearchController : Controller
    {
        
       private CandidateSearchModel _candidateSearchModel;
        
       private readonly  CandidateRepository _repository =new CandidateRepository();
       
       
        [Authorize]
        [HttpGet]
        public  ActionResult CandidateList()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(Ticket.Name);

                _candidateSearchModel = new CandidateSearchModel();
                _candidateSearchModel.AddPersonal = new CandidatePersonal();
                _candidateSearchModel.AddPersonal.Experience = new Experience();
                _candidateSearchModel.AddPersonal.Education = new Education();
                _candidateSearchModel.AddPersonal.Skill = new Skill();
                _candidateSearchModel.AddPersonal.PageIndex = 0;

                _candidateSearchModel.PersonalList = _repository.CandidateSearch(_candidateSearchModel.AddPersonal);
                ViewBag.count = _candidateSearchModel.PersonalList.Count();

                return View(_candidateSearchModel);
            }
            return RedirectToAction("login", "Home");


        }
       [HttpPost]
        public ActionResult CandidateList(CandidateSearchModel candidate)
        {
           
           // candidate.AddPersonal = new CandidatePersonal();
            //candidate.AddPersonal.Experience = new Experience();
            //candidate.AddPersonal.Skill = new Skill();
            //candidate.AddPersonal.Education = new Education();
           // _candidateSearchModel=new CandidateSearchModel();
             //candidate.AddPersonal.PageIndex = 0;
            candidate.PersonalList = _repository.CandidateSearch(candidate.AddPersonal);
           // _candidateSearchModel.AddPersonal= candidate.AddPersonal;
           // _candidateSearchModel.AddPersonal.Skill = candidate.AddPersonal.Skill;
            //_candidateSearchModel.AddPersonal.Experience = candidate.AddPersonal.Experience;
           
            return View(candidate);

        }
       
      
        [HttpGet]
        public ActionResult CandidateDetail(int id)
        {
             CandidatePersonal cp = new CandidatePersonal();
               
             cp.CandidateList =_repository.GetCandidateRecord(id).ToList();
             cp.ExperienceList = _repository.GetExperienceRecord(id).ToList();
             cp.EducationList  = _repository.GetEducationRecord(id).ToList();
             cp.SkillList = _repository.GetSkillRecord(id).ToList();

            return View(cp) ;
        }

        public ActionResult Paging(int id, string what, string where, string funArea, string minimumExp)
        {
            CandidateSearchModel model = new CandidateSearchModel();
            model.AddPersonal = new CandidatePersonal();
            model.AddPersonal.Experience = new Experience();
            model.AddPersonal.Skill = new Skill();
            model.AddPersonal.Education = new Education();
            
            model.AddPersonal.PageIndex = id;
            model.AddPersonal.Skill.SkillTerm = what;
            model.AddPersonal.City = where;

            model.AddPersonal.Experience.FunctionalArea = funArea;
            model.AddPersonal.Experience.ExpYear = minimumExp;

            
            CandidateRepository _repository=new CandidateRepository();
          
            model.PersonalList = _repository.CandidateSearch(model.AddPersonal);
            
            
            return PartialView("ShortDetail", model);
        }

        [HttpGet]
        public ActionResult NewProfile()
        {
            return  View();
        }      
    }
}

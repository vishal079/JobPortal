using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Areas.Employer.Models;
using JobPortal.Core;
using JobPortal.Web.Areas.Profile.Models;

namespace JobPortal.Web.Areas.Profile.Controllers
{
    public class MyMessageController : Controller
    {
        //
        // GET: /Profile/MyMessage/
        
        private EmployerRepository _employerRepository;
        private ProfileResume _profileResume; 
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie != null)
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(Ticket.Name);
                
                _employerRepository=new EmployerRepository();
                _profileResume=new ProfileResume();
                _profileResume.JobTitle=_employerRepository.GetJobTitle(candidateId);
                return PartialView("Index",_profileResume);
            }
            return View();
        }
        [Authorize]
        public ActionResult ShowMsg(string title)
        {

            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);

                _employerRepository = new EmployerRepository();
                _profileResume = new ProfileResume();
                _profileResume.PersonalMessages = _employerRepository.GetUnreadCandidateMessage(candidateId, title);
                //_profileResume.JobTitle = _employerRepository.GetJobTitle(candidateId);
                return PartialView("ShowMsg",_profileResume);
            }
            return PartialView();
        }
        public ActionResult DetailShowMsg(int id)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);
                _profileResume=new ProfileResume();
                _employerRepository = new EmployerRepository();
                _profileResume.PersonalMessages = _employerRepository.AllShowMsg(id);
                return PartialView("DetailShowMsg", _profileResume);
            }
            return PartialView();
        }

        public ActionResult SentNewMessage(ProfileResume model)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);
                _profileResume = new ProfileResume();
                _employerRepository = new EmployerRepository();
               // _profileResume.PersonalMessages = _employerRepository.AllShowMsg();
                return PartialView("DetailShowMsg", _profileResume);
            }
            return null;
        }

    }
}

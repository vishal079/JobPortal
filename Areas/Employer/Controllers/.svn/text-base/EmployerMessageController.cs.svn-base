using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Areas.Employer.Models;
using JobPortal.Core;
using JobPortal.Core.Domain.User;

namespace JobPortal.Areas.Employer.Controllers
{
    public class EmployerMessageController : Controller
    {
        //
        // GET: /Employer/EmployerMessage/

        private EmployerVM _employerVm;
        private EmployerRepository _employerRepository;
        [Authorize]
        public ActionResult Index()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);
                _employerVm = new EmployerVM();
                _employerRepository = new EmployerRepository();
                //_employerVm.PMs = _employerRepository.GetUnReadMessage(candidateId);
                _employerVm.MsgTitle = _employerRepository.GetMsgTitle(candidateId);
                return View("Index", _employerVm);
            }
            return View();
        }

        public ActionResult DisplayMsg(string title)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);
                _employerVm = new EmployerVM();
                _employerRepository = new EmployerRepository();
                _employerVm.PMs = _employerRepository.GetUnReadMessage(candidateId,title);
                return PartialView("DisplayMsg",_employerVm);
            }
            Index();
            return View("Index");
        }

        public ActionResult DetailDiscussion(long id)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);
                _employerVm = new EmployerVM();
                _employerRepository = new EmployerRepository();
                _employerVm.PMs = _employerRepository.AllDiscussionMsg(id);
                return PartialView("DisplayDisscuss",_employerVm);
            }
            Index();
            return View("Index");
        }

    }
}

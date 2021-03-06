﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using JobPortal.Areas.Admin.Models;
using JobPortal.Core;
using JobPortal.Core.Domain;
using JobPortal.Core.Domain.User;
using JobPortal.Web;

namespace JobPortal.Areas.Admin.Controllers
{
    
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/
        private AdminVM _adminvm;
        private SearchRepository _searchRepository;
        private EmployerRepository _employerRepository;
        private CandidateSearchModel _candidateSearchModel = new CandidateSearchModel();
        private AdminRepository _adminRepository;
        private readonly CandidateRepository _repository = new CandidateRepository();
        
        
        public ActionResult CandidateProfileList()
        {
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
        
        
       [HttpGet]
       public ActionResult CandidateProfileDetail(int id)
       {
           CandidatePersonal cp = new CandidatePersonal();

           cp.CandidateList = _repository.GetCandidateRecord(id).ToList();
           cp.ExperienceList = _repository.GetExperienceRecord(id).ToList();
           cp.EducationList = _repository.GetEducationRecord(id).ToList();
           cp.SkillList = _repository.GetSkillRecord(id).ToList();

           return View(cp);
       }
        
        
       public ActionResult DeleteProfile(long id )
       {
          _adminRepository=new AdminRepository();
 
           Core.CandidatePersonal candidate = _adminRepository.GetProfileId(id);
           _adminRepository.DeleteProfile(candidate);
           return RedirectToAction("CandidateProfileList","Admin");
       }


       
        public ActionResult GetJob()
        {
            
            Core.Job job = new Core.Job();
            _adminvm = new AdminVM();
            _searchRepository = new SearchRepository();
            _employerRepository = new EmployerRepository();
            _adminvm.Jobs = _searchRepository.Search(job,1);
            _adminvm.Job = job;
            Session["ListJob"] = _adminvm.Jobs;
            ViewBag.Count = _adminvm.Jobs.Count();
            return View(_adminvm);
        }

        
        [HttpGet]
        public ActionResult AdminPageIndex(int id)
        {
            Core.Job j = new Core.Job();
            j.PageIndex = id;
            _searchRepository = new SearchRepository();
            _adminvm = new AdminVM();
            _adminvm.Jobs = _searchRepository.Search(j, 1);
            _adminvm.Job = j;
            Session["ListJob"] = _adminvm.Jobs;
            //return PartialView("GetJob", _adminvm);
            return View("GetJob", _adminvm);
        }
        
        [HttpGet]
        public ActionResult ShowDetailJob(int id)
        {
            _adminvm = new AdminVM();
            Core.Job Detaildiscription = new Core.Job();
            List<Core.Job> Joblists = new List<Core.Job>();
            Joblists = (List<Core.Job>) Session["ListJob"];
            foreach (var job in Joblists)
            {
                if (job.Id == id)
                {
                    _adminvm.Job = job;
                }
            }
            return View(_adminvm);
        }

        public ActionResult DeleteJob(int id)
        {
            _adminRepository = new AdminRepository();
             Core.Job job = _adminRepository.GetJobId(id);
            _adminRepository.DeleteJob(job);
            GetJob();
            return View("GetJob");
            //return RedirectToAction("GetJob","Admin");
        }

        public ActionResult UpdateRecord(int id,Boolean status)//, Boolean status
        {
            _adminRepository= new AdminRepository();
            Core.Job job = _adminRepository.Jobstatus(id,status);
            if (job.Status == true)
            {
                job.Status = false;
            }
            else
            {
                job.Status = true;
            }
           
            _adminRepository.UpdateJob(job);
            
            // _adminRepository =new AdminRepository();
            // _adminRepository.UpdateJob(id, status);
            //  GetJob();
            return RedirectToAction("GetJob","Admin");

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public  ActionResult Login(Login model,string returnUrl)
        {
            _adminRepository = new AdminRepository();
            Login login = new Login();
            login = _adminRepository.Login(model, 2);
            if (login.UserName == model.UserName)
            {

             
                return  RedirectToAction("GetJob","Admin");
             
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View("login");
            }
         }

        
      

        public ActionResult Information( )
        {   
            return View();
        }

        public ActionResult ForgotPassword(Login model)
        {
            _adminRepository = new AdminRepository();
            Login login=new Login();
            model.Password = "p";
            login = _adminRepository.Login(model,1);
            
            //bool Isavaible = logins.Any(a => a.UserName==model.UserName);
            
            if (login.UserName==model.UserName)
            {
            var fromAddress = new MailAddress("bhumi.parmar1@gmail.com", "Job Portal");
            var toAddress = new MailAddress(model.UserName, "To Name");
            const string fromPassword = "bhumi_143";
            const string subject = "forgot password";
             string body = login.Password;
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try{
                smtp.Send(message);
                ModelState.AddModelError("", "Password send Your MAil Address");
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("","Sorry E-Mail Not Send");
                }
            }

            }
            else
            {
                ModelState.AddModelError("", "Plz Enter Your Correct Email-Address");    
            }
           
            
             //return Content("Email Sent!");
            
            return View("Login");
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Areas.Job.Controllers
{
    using JobPortal.Core;

    public class JobPostController : Controller
    {
        //
        // GET: /Job/JobPost/

        private SearchRepository _searchRepository;
        private EmployerRepository _employerRepository;


        [HttpGet]
        public ActionResult JobPost()
        {
            Job job = new Job();
            job.Emplyer = new Core.Domain.User.Employer();
            _employerRepository = new EmployerRepository();
            job.Emplyer.EmployerId = 16;
            job.Emplyer = _employerRepository.IsEmployerAvailable(job.Emplyer.EmployerId);
            if (job.Emplyer != null)
            {
                return View(job);
            }
            return View();
        }
        public ActionResult GetCompanyDetail()
        {
            return View("JobPost");
        }

        /*[HttpPost]
        [ValidateInput(false)]
        public ActionResult JobPost(Job job)
        {
            _searchRepository = new SearchRepository();
            job.PostedTime = DateTime.Now;
            job.KeyTerm = "c java php .net";
            _searchRepository.JobPost(job);
            return View();
        }*/

        [ValidateInput(false)]
        public ActionResult JobPost(Job job)
        {
            job.Emplyer = new Core.Domain.User.Employer();
            _searchRepository = new SearchRepository();
            _employerRepository = new EmployerRepository();

            job.PostedTime = DateTime.Now;
            //job.EmpId = 16;
            job.Emplyer.EmployerId = 16;
            //job.Emplyer = _employerRepository.IsEmployerAvailable(job.EmpId);
            //if(emp != null)
            //{
                _searchRepository.JobPostByCastle(job);
                return View();
            /*}
            else
            {
                _employerRepository.EmployerRegistration(job);
                _searchRepository.JobPostByCastle(job);
            }
            return View();*/
        }
    }
}

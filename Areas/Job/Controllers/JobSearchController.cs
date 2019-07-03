using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Core;
using JobPortal.Areas.Job.Models;
using JobPortal.Core.Repository;

namespace JobPortal.Areas.Job.Controllers
{
    using Core.Domain.User;

    public class JobSearchController : Controller
    {
        private SearchRepository _searchRepository;
        private FavRepository _favRepository;
        private EmployerRepository _employerRepository;
        private JobSearchVM _jobSearchVm;

        public ActionResult Index()
        {
            return View();
        }

        /*====== This method fetch Jobs As per Keyword Entered If Keywords are null then it will fetch all records */
        [HttpPost]
        public ActionResult Index(Core.Job job)
        {
            _searchRepository = new SearchRepository();
            _favRepository = new FavRepository();
            _jobSearchVm = new JobSearchVM();

            if(job.KeyTerm != null || job.Country!= null)
            {
                var recentSearch = new RecentSearch(job.KeyTerm,job.Country);
                recentSearch.RecentSearches = new List<RecentSearch>();
                recentSearch.RecentSearches.Add(new RecentSearch(job.KeyTerm,job.Country));

                if (Session["RecentSearch"] != null)
                {
                    var sessionItems = (List<RecentSearch>)Session["RecentSearch"];
                    
                    recentSearch.RecentSearches.AddRange((List<RecentSearch>)Session["RecentSearch"]);
                    bool hasKeyword = false; bool hasLocation=false;
                    hasKeyword = sessionItems.Any(has => has.KeyTerms == recentSearch.KeyTerms);
                    hasLocation = sessionItems.Any(has => has.Location == recentSearch.Location);
                    if (hasKeyword == false || hasLocation == false)
                    {
                        Session["RecentSearch"] = (List<RecentSearch>)recentSearch.RecentSearches;
                    }
                }
                else
                {
                    //recentSearch.RecentSearches.Add(new RecentSearch(job.KeyTerm,job.KeyTerm));
                    Session["RecentSearch"] = (List<RecentSearch>) recentSearch.RecentSearches;
                }
            }
            _jobSearchVm.Jobs = _searchRepository.Search(job,2);
            _jobSearchVm.Job = job;
            
            /*============ Data To Use In Other Method ==============*/
            TempData.Clear();
            TempData["ListofJobs"] = _jobSearchVm.Jobs;
            //Session["ListofJobs"] = _jobSearchVm.Jobs;
            /*=======================================================*/

            if (HttpContext != null)
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    int candidateId = int.Parse(ticket.Name);
                    if(candidateId !=0)
                    {
                        _jobSearchVm.FavJobs = _favRepository.GetAll(candidateId);
                    }
                }
            }
            return View(_jobSearchVm);
        }
        
        /*====== This method fetch Jobs As per Keyword Entered Using Ajax Request If Keywords are null then it will fetch all records Ajax Request */
        [HttpPost]
        public ActionResult AjaxSearch(Core.Job job)
        {        
            _searchRepository = new SearchRepository();        
            _jobSearchVm = new JobSearchVM();        
            _favRepository = new FavRepository();        
            if (job.KeyTerm != null || job.Country != null)        
            {        
                var recentSearch = new RecentSearch                               
                {                               
                    RecentSearches = new List<RecentSearch>{new RecentSearch(job.KeyTerm, job.Country)}                               
                };        
                if (Session["RecentSearch"] != null)        
                {        
                    Session.Add("RecentSearch", recentSearch.RecentSearches);        
                }        
                else        
                {        
                    Session["RecentSearch"] = (List<RecentSearch>) recentSearch.RecentSearches;        
                }        
            }        
            _jobSearchVm.Jobs = _searchRepository.Search(job, 2);        
            _jobSearchVm.Job = job;
            TempData.Clear();
            TempData["ListofJobs"] = _jobSearchVm.Jobs;
            return PartialView("Detail",_jobSearchVm);
        }


        /*====== This Action is used for Water Text ======*/
        /*public ActionResult Lookup(string id)
        {
            _searchRepository = new SearchRepository();
            _jobSearchVm = new JobSearchVM();
            _jobSearchVm.BasicSearches = _searchRepository.Keyterms();
            var retval = _jobSearchVm.BasicSearches
                .Where(x => x.StartsWith(id))
                .OrderBy(x => x)
                .Take(3)
                .Select(r => new { Tag = r });

            var tagNames = (from p in _jobSearchVm.BasicSearches where p.Contains(id) select p).Distinct().Take(3);

            string content = string.Join<string>("\n", tagNames);
            //return Content(content);
            return Json(content);
        }*/


        /*====== Each Page Shows five Jobs Per Page By Using this action candidate can see other jobs ======*/
        public ActionResult PageIndex(int id, string what, string where, string sortBy)
        {
            _favRepository = new FavRepository();
            _searchRepository = new SearchRepository();
            var j = new Core.Job {KeyTerm = what, Country = @where, OrderBy = sortBy, PageIndex = id};
            _jobSearchVm = new JobSearchVM {Jobs = _searchRepository.Search(j, 2), Job = j};
            TempData.Clear();
            TempData["ListofJobs"] = _jobSearchVm.Jobs;
            return PartialView("Detail",_jobSearchVm);
        }


        /*====== This Action shows a Detail Description about a particular job ======*/
        [HttpGet]
        public ActionResult JobDetail(int id)
        {
            _jobSearchVm = new JobSearchVM();
            _employerRepository = new EmployerRepository();
            _jobSearchVm.Job = new Core.Job {Emplyer = new Employer()};
            _searchRepository = new SearchRepository();
            _jobSearchVm.Job = _searchRepository.SearchJobbyId(id);
            _jobSearchVm.Job.Emplyer = _employerRepository.GetEmployerById(_jobSearchVm.Job.Emplyer.EmployerId);
            return View(_jobSearchVm);
        }


        /*====== User click on keyterm as per that key term search from database ======*/
        public virtual ActionResult SearchOnKeyterm(string id)
        {
            var job = new Core.Job {Emplyer = new Employer(), KeyTerm = id};
            Index(job);
            return View("Index");
        }


        /*====== User click on ViewAll Job this action will perform ======*/
        public ActionResult GetAllJobs()
        {
            var job = new Core.Job {Emplyer = new Employer()};
            Index(job);
            return View("Index");
        }


        /*====== User click on recent search jobs then this job will display previous search ======*/
        public ActionResult RecentSearch(string pKeyTerm,string pLocation)
        {
            var job = new Core.Job {KeyTerm = pKeyTerm, Country = pLocation};
            Index(job);
            return View("Index");
        }


        /*====== This action used to add jobs to favorite job list It is Only For Autorised user ======*/
        [Authorize]
        public ActionResult FavJob(int jid,string jobTitle,string company)
        {
            if (HttpContext != null)
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    int candidateId = int.Parse(ticket.Name);

                    var favJob = new FavJob {Job = new Core.Job(), MainUserId = new MainUser()};
                    _jobSearchVm = new JobSearchVM();
                    _favRepository = new FavRepository();
                    var uow = new UnitOfWork();

                    favJob.Job.Id = jid;
                    favJob.MainUserId.Mid = candidateId;
                    favJob.JobTitle = jobTitle;
                    favJob.CompanyName = company;

                    _favRepository.AddFavoritejob(favJob,uow);
                    uow.Commit();
                
                    _jobSearchVm.FavJobs = _favRepository.GetAll(candidateId);
                }
            }
            //_jobSearchVm = new JobSearchVM();
            //_jobSearchVm.Jobs = _searchRepository.Search(j, 2);
            //_jobSearchVm.Job = j;
            //TempData.Clear();
            //TempData["ListofJobs"] = _jobSearchVm.Jobs;
            return PartialView("FavoriteJobs", _jobSearchVm);
        }


        /*====== This action used to Remove jobs from favorite job list It is Only For Autorised user ======*/
        [Authorize]
        public ActionResult RemoveFavoriteJob(int jobId,Boolean removeType)
        {
            if (HttpContext != null)
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    int candidateId = int.Parse(ticket.Name);

                    var favJob = new FavJob {Job = new Core.Job(), MainUserId = new MainUser()};
                    _jobSearchVm = new JobSearchVM();
                    _favRepository = new FavRepository();
                    var uow = new UnitOfWork();

                    favJob.FavoriteJobId = jobId;
                    favJob.Job.Id = jobId;
                    favJob.MainUserId.Mid = candidateId;
                    if (removeType == true)
                    {
                        _favRepository.RemovoFavoriteJobByCastleActive(favJob,uow);
                        uow.Commit();
                    }
                    else
                    {
                        _favRepository.RemoveFavoriteJobByProcedure(favJob);    
                    }
                    _jobSearchVm.FavJobs = _favRepository.GetAll(candidateId);
                }
            }

            //_jobSearchVm = new JobSearchVM();
            //_jobSearchVm.Jobs = _searchRepository.Search(j, 2);
            //_jobSearchVm.Job = j;
            //TempData.Clear();
            //TempData["ListofJobs"] = _jobSearchVm.Jobs;
            return PartialView("FavoriteJobs", _jobSearchVm);
        }
    }
}

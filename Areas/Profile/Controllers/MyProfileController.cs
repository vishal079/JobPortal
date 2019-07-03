using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web;
using System.Web.Security;
using JobPortal.Core.Domain;
using JobPortal.Core.Domain.User;
using JobPortal.Web;
using JobPortal.Core;
using JobPortal.Web.Areas.Profile.Models;


namespace JobPortal.Web.Areas.Profile.Controllers
{
    public class MyProfileController : Controller
    {
        
     
        private CandidateRepository _candidateRepo;
            
        [Authorize]
        [HttpGet]
        public ActionResult RegisterProfile()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(Ticket.Name);
            
        
            //first check if user is login or not ,if user is login then check that user is registration completed or not
           _candidateRepo=new CandidateRepository();
           
            long key= _candidateRepo.CheckRegistration(candidateId);
            
                
           
            ProfileResume mResume = new ProfileResume(); 

            if (key != 0 )
            {
                mResume.Candidate = new CandidatePersonal();
                mResume.Candidate.Experience =new Experience();
                mResume.Candidate.Education=new Education();
                mResume.Candidate.Skill=new Skill();
                mResume.Candidate.Resume=new Resume();

                mResume.Candidate.Experience.MainUser = new MainUser();
                mResume.Candidate.Experience.MainUser.Mid = candidateId; 

                mResume.Candidate.Education.MainUser=new MainUser();
                mResume.Candidate.Education.MainUser.Mid = candidateId;

                mResume.Candidate.Skill.MainUser=new MainUser();
                mResume.Candidate.Skill.MainUser.Mid = candidateId;
 
                mResume.Candidate.Resume.MainUser=new MainUser();
                mResume.Candidate.Resume.MainUser.Mid = candidateId;

                mResume.CandidatePersonals = _candidateRepo.GetCandidateRecord(candidateId).ToList();
                mResume.Experiences = _candidateRepo.GetExperienceRecord(candidateId).ToList();
                mResume.Educations = _candidateRepo.GetEducationRecord(candidateId).ToList();
                mResume.Skills = _candidateRepo.GetSkillRecord(candidateId).ToList();
                mResume.Resumes = _candidateRepo.GetResumeRecord(candidateId).ToList();

                return View("Profile", mResume);
            }
            else
            {
               
                mResume.Candidate =new CandidatePersonal();
                mResume.Candidate.MainUser =new MainUser();
                mResume.Candidate.MainUser.Mid = candidateId;
                mResume.Candidate.CreatedOn = DateTime.Now;
                mResume.Candidate.UpdatedOn = DateTime.Now;

                return View(mResume);    
            }

            }
            return null;
        }
        public void UpdateDate(long id)
        {
            Core.CandidatePersonal candidate=_candidateRepo.FindDateId(id);
            candidate.SetUpdateOn();
            _candidateRepo.ChangeDate(candidate);

        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            _candidateRepo=new CandidateRepository();
            var ProfileVM = new ProfileResume {EditCandidate = _candidateRepo.UpdateCandidateId(id)};
            return PartialView("Edit",ProfileVM);
        }
        [HttpPost]
        public  ActionResult Edit(ProfileResume model)
        {
            _candidateRepo=new CandidateRepository();
            var uow = new UnitOfWork();
           
            if (model.EditCandidate != null)
            {
             
                _candidateRepo.UpdateCandidate(model.EditCandidate,uow);
                if (uow.Commit())
                {
                    UpdateDate(model.EditCandidate.MainUser.Mid);
                    this.ShowMessage(MessageType.Success, "Candidate Personal detail has been changed.");
                }
                else
                {
                    this.ShowMessage(MessageType.Error, uow.ErrorMessage);
                }
            }
            else if(model.EditExperience !=null)
            {
                
                _candidateRepo.UpdateExperience(model.EditExperience,uow);
          
                if (uow.Commit())
                {
                    UpdateDate(model.EditExperience.MainUser.Mid);

                    this.ShowMessage(MessageType.Success, "Candidate Experience detail has been changed.");
                }
                else
                {
                    this.ShowMessage(MessageType.Error, uow.ErrorMessage);
                }
            }
            else if(model.EditEducation !=null)
            {
                _candidateRepo.UpdateEducation(model.EditEducation, uow);
                if(uow.Commit())
                {
                    UpdateDate(model.EditEducation.MainUser.Mid);
                    this.ShowMessage(MessageType.Success, "Candidate Education detail has been changed.");
                    
                }
                else
                {
                    this.ShowMessage(MessageType.Error, uow.ErrorMessage);
                }
            }
            else if(model.EditSkill !=null)
            {
                _candidateRepo.UpdateSkill(model.EditSkill, uow);

                if (uow.Commit())
                {
                    UpdateDate(model.EditSkill.MainUser.Mid);
                    this.ShowMessage(MessageType.Success, "Candidate Skill detail has been changed.");
                }
                else
                {
                    this.ShowMessage(MessageType.Error, uow.ErrorMessage);
                }
               
            }
           else if(model.EditResume !=null)
            {
                if (model.EditResume.Photo != null)
                {
                        
                    string path = @"D:\Temp\";

                   /* if (model.EditResume.Photo.ContentLength > 190240)
                    {
                        ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
                        return View();
                    }*/
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "txt", "doc","docx", "pdf" };
                    model.EditResume.FileExtension = System.IO.Path.GetExtension(model.EditResume.Photo.FileName).Substring(1);
                    model.EditResume.FileName = model.EditResume.Photo.FileName;

                    if (!supportedTypes.Contains(model.EditResume.FileExtension))
                    {
                        ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        //_jobSearch.JobDetail(j.Id);
                        //        //return View();
                    }
                    //   csVM.AddResume.Photo.SaveAs(path + csVM.AddResume.Photo.FileName);
                    //  path = csVM.AddResume.Photo.InputStream.ToString();
                    MemoryStream target = new MemoryStream();
                    model.EditResume.Photo.InputStream.CopyTo(target);
                    model.EditResume.FileData = target.ToArray();

                   // Core.Resume resume = _candidateRepo.ChangeResume(model.EditResume.Rid);
                    //_candidateRepo.DeleteOldResume(resume);

                    _candidateRepo.ChangeResume(model.EditResume,uow);
                    if (uow.Commit())
                    {
                        UpdateDate(model.EditResume.MainUser.Mid);
                        this.ShowMessage(MessageType.Success, "Candidate resume has been changed.");

                    }
                    else
                    {
                        this.ShowMessage(MessageType.Error, uow.ErrorMessage);
                    }

                }

            } 
            
            
            return RedirectToAction("RegisterProfile");

        }
        [HttpGet]
        public ActionResult EditExperience(long id)
        {
            _candidateRepo=new CandidateRepository();
            var profileVM = new ProfileResume { EditExperience = _candidateRepo.UpdateExperienceId(id) };
            
            return PartialView("Edit",profileVM);
        }
      
        [HttpGet]
        public ActionResult EditEducation(long id)
        {
            _candidateRepo = new CandidateRepository();
            var profileVM = new ProfileResume {EditEducation = _candidateRepo.UpdateEducationId(id)};
            return PartialView("Edit",profileVM);
        }
     
        
        public ActionResult EditSkill(long id)
        {
            _candidateRepo = new CandidateRepository();
            var profileVM = new ProfileResume {EditSkill  = _candidateRepo.UpdateSkillId(id)};
            return PartialView("Edit",profileVM);
        }
      
       
       public  ActionResult ChangeResume(long id)
       {
           _candidateRepo = new CandidateRepository();
           var profileVM = new ProfileResume {EditResume = _candidateRepo.ChangeResume(id)};
           return PartialView("Edit", profileVM);
       }
     
        public ActionResult DeleteExperience(long id)
        {
            _candidateRepo = new CandidateRepository();
            var uow = new UnitOfWork();
            Core.Experience experience = _candidateRepo.UpdateExperienceId(id);
            _candidateRepo.RemoveExperience(experience,uow);
            if (uow.Commit())
            {
                this.ShowMessage(MessageType.Success, "Candidate  Experience has been deleted.");
            }
            else
            {
                this.ShowMessage(MessageType.Error, uow.ErrorMessage);
            }

            return RedirectToAction("RegisterProfile");
        }
        public ActionResult DeleteEducation(long id)
        {
            _candidateRepo = new CandidateRepository();
            var uow = new UnitOfWork();
            Core.Education education = _candidateRepo.UpdateEducationId(id);
            _candidateRepo.RemoveEducation(education,uow);
            if (uow.Commit())
            {
                this.ShowMessage(MessageType.Success, "Candidate Education  has been Deleted.");
            }
            else
            {
                this.ShowMessage(MessageType.Error, uow.ErrorMessage);
            }
            return RedirectToAction("RegisterProfile");
            
        }
        public ActionResult DeleteSkill(long id)
        {
            _candidateRepo = new CandidateRepository();
            var uow = new UnitOfWork();
            Core.Skill skill = _candidateRepo.UpdateSkillId(id);
            _candidateRepo.RemoveSkill(skill,uow);
            if (uow.Commit())
            {
                this.ShowMessage(MessageType.Success, "Candidate Skill  has been deleted.");
            }
            else
            {
                this.ShowMessage(MessageType.Error, uow.ErrorMessage);
            }
            return RedirectToAction("RegisterProfile");

        }
    
        public ActionResult AddExperience(ProfileResume model)
        {
            _candidateRepo=new CandidateRepository();
            var uow = new UnitOfWork();
          
                _candidateRepo.AddExpe(model.Candidate.Experience, uow);
                if (uow.Commit())
                {
                    this.ShowMessage(MessageType.Success, "Candidate new Experience  has been added.");
                }
                else
                {
                    this.ShowMessage(MessageType.Error, uow.ErrorMessage);
                }
            return RedirectToAction("RegisterProfile");

        }
        public ActionResult AddEducation(ProfileResume model)
        {
            _candidateRepo = new CandidateRepository();
            var uow = new UnitOfWork();

            _candidateRepo.AddEdu(model.Candidate.Education, uow);
            if (uow.Commit())
            {
                this.ShowMessage(MessageType.Success, "Candidate new Education  has been added.");
            }
            else
            {
                this.ShowMessage(MessageType.Error, uow.ErrorMessage);
            }
            return RedirectToAction("RegisterProfile");

        }
        public ActionResult AddSkill(ProfileResume model)
        {
            _candidateRepo = new CandidateRepository();
            var uow=new UnitOfWork();
            _candidateRepo.Addski(model.Candidate.Skill,uow);
            if (uow.Commit())
            {
                    this.ShowMessage(MessageType.Success, "Candidate new Skill has been added.");
            }
            else
            {
                    this.ShowMessage(MessageType.Error, uow.ErrorMessage);
            }
            return RedirectToAction("RegisterProfile");
     
        }

    
        [HttpPost]
        public  ActionResult RegisterProfile(ProfileResume csVm)
        {
             _candidateRepo = new CandidateRepository();
                _candidateRepo.InsertPersonalRecord(csVm.Candidate);
                _candidateRepo.InsertExperienceRecord(csVm.Candidate);
                _candidateRepo.InsertEducationRecord(csVm.Candidate);
                _candidateRepo.InsertSkillRecord(csVm.Candidate);
           
            csVm.AddResume.MainUser =new MainUser();
            csVm.AddResume.MainUser.Mid = csVm.Candidate.MainUser.Mid;
            
            if (csVm.AddResume.Photo != null)
            {
                string path = @"D:\Temp\";

                if (csVm.AddResume.Photo.ContentLength > 10240)
                {   
                    ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
                    return View();
                }
                var supportedTypes = new[] {"jpg", "jpeg", "png", "txt", "doc", "pdf"};
                csVm.AddResume.FileExtension = System.IO.Path.GetExtension(csVm.AddResume.Photo.FileName).Substring(1);
                csVm.AddResume.FileName = csVm.AddResume.Photo.FileName;

                if (!supportedTypes.Contains(csVm.AddResume.FileExtension))
                {
                    ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                    
                   return View();
                }
                //csVM.AddResume.Photo.SaveAs(path + csVM.AddResume.Photo.FileName);
                // path = csVM.AddResume.Photo.InputStream.ToString();
                 
                MemoryStream target = new MemoryStream();
                 csVm.AddResume.Photo.InputStream.CopyTo(target);
                 csVm.AddResume.FileData = target.ToArray();

               }
            _candidateRepo.InsertResume(csVm.AddResume);
            
            return View("RegisterCompelete");
        }
        [Authorize]
        [HttpGet]
        public ActionResult ApplyEdit(int id)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(Ticket.Name);

                ProfileResume model = new ProfileResume { EditResume = new Resume { Apply = new ApplyJob { JobId = new Job { Id = id } , CandidateId= new MainUser { Mid = candidateId } }} };
                
                return PartialView(model);
            }
            return RedirectToAction("login","Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult ApplyEdit(ProfileResume model)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(Ticket.Name);

                UnitOfWork uow = new UnitOfWork();
                _candidateRepo = new CandidateRepository();

                if (model.EditResume.Photo != null)
                {

                    string path = @"D:\Temp\";

                    /* if (model.EditResume.Photo.ContentLength > 190240)
                    {
                        ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
                        return View();
                    }*/
                    var supportedTypes = new[] {"jpg", "jpeg", "png", "txt", "doc", "docx", "pdf"};
                    model.EditResume.FileExtension =
                        System.IO.Path.GetExtension(model.EditResume.Photo.FileName).Substring(1);
                    model.EditResume.FileName = model.EditResume.Photo.FileName;

                    if (!supportedTypes.Contains(model.EditResume.FileExtension))
                    {
                        ModelState.AddModelError("photo",
                                                 "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        //_jobSearch.JobDetail(j.Id);
                        //        //return View();
                    }
                    //   csVM.AddResume.Photo.SaveAs(path + csVM.AddResume.Photo.FileName);
                    //  path = csVM.AddResume.Photo.InputStream.ToString();
                    MemoryStream target = new MemoryStream();
                    model.EditResume.Photo.InputStream.CopyTo(target);
                    model.EditResume.FileData = target.ToArray();


                    Core.Resume resume = _candidateRepo.FindResume(candidateId);
                    _candidateRepo.DeleteOldResume(resume);
                    model = new ProfileResume { EditResume = new Resume {MainUser = new MainUser {Mid = candidateId} }};
                   
                    model.EditResume.MainUser =new MainUser();
                    model.EditResume.MainUser.Mid = candidateId;              
                    _candidateRepo.ChangeResume(model.EditResume, uow);
                    uow.Commit();
                    UpdateDate(candidateId);

                }
                _candidateRepo.MatchJobApply(model.EditResume.Apply);
                 return RedirectToAction("ProfileAccounts");
            }
            else
            {
                return RedirectToAction("login", "home");
            }
           
        }

        [Authorize]
        [HttpGet]
        public ActionResult ProfileAccounts()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(Ticket.Name);

                SearchRepository _searchRepository = new SearchRepository();
                ProfileResume model = new ProfileResume();
                model.MatchJobs = _searchRepository.FindMatchJob(candidateId);
                ViewBag.matchCount = model.MatchJobs.Count();
                return View(model);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }

        }
        [Authorize]
        public ActionResult MatchJob()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(Ticket.Name);

                SearchRepository _searchRepository = new SearchRepository();
                ProfileResume model = new ProfileResume();
                model.MatchJobs = _searchRepository.FindMatchJob(candidateId);
                ViewBag.matchCount = model.MatchJobs.Count();
                return PartialView("MatchJob",model);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }

            
        }
        [Authorize]
        [HttpGet]
        public ActionResult ApplyMatchJob()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if(cookie !=null)
            {

                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);

                SearchRepository searchRepository=new SearchRepository();
         
                ProfileResume model = new ProfileResume();
                model.MatchJobs = searchRepository.FindAppliedJob(candidateId);
                ViewBag.appliedCount = model.MatchJobs.Count();
               
                return PartialView("ApplyMatchJob",model);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
        [HttpGet]
        public ActionResult MatchJobDetail(int id)
        {
            SearchRepository search=new SearchRepository();
            EmployerRepository employer=new EmployerRepository();

            ProfileResume model=new ProfileResume();

            model.Job = search.SearchJobbyId(id);
            model.Job.Emplyer = employer.GetEmployerById(model.Job.Emplyer.EmployerId);

            return View("MatchJobDetail",model);
        }
    }
}

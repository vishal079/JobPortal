using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Core;
using System.Data;
using System.IO;
using JobPortal.Core.Domain.User;
using JobPortal.Core.Repository;
using JobPortal.Web;
using JobPortal.Areas.Job.Models;
using NHibernate.Mapping;

namespace JobPortal.Areas.Job.Controllers
{
    public class JobApplyController : Controller
    {
        //
        // GET: /Search/JobApply/

        private SearchRepository _searchRepository;
        private JobSearchController _jobSearch;
        private JobSearchVM _jobSearchVm;
        private UserRepository _userRepository;
        
        [Authorize]
        public ActionResult UploadResume(int jobId, string companyName)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);

                _searchRepository = new SearchRepository();
                _userRepository = new UserRepository();

                var resume = new ResumeUploadVM();
                resume.ResumeUpload = new ResumeUpload();
                var httpCookie = Request.Cookies.Get("jobid");
                if (httpCookie != null)
                    resume.ResumeUpload.JobId = int.Parse(httpCookie.Value.ToString());

                var dataTable = new DataTable();
                dataTable = _searchRepository.IsResumeAvailable(1, candidateId);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var resumeUpload = new ResumeUpload(dataRow);
                    resume.ResumeUpload = resumeUpload;
                    resume.ResumeUpload.JobId = jobId;
                    if (resume.ResumeUpload.FileName != "")
                    {
                        resume.IsResumeAvailable = true;
                    }
                    else
                    {
                        resume.IsResumeAvailable = false;
                    }
                    resume.ResumeUpload.IsUserAvailable = true;
                }
                return PartialView("Test", resume);
            }
            return RedirectToAction("JobDetail", "JobSearch");
        }
        
        [Authorize]
        public virtual PartialViewResult DeleteResume(int jobId, string companyName)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);
                _searchRepository = new SearchRepository();
                _searchRepository.DeleteResume(2, candidateId);
            }
            var resume = new ResumeUploadVM();
            resume.IsResumeAvailable = false;
            Test(jobId, companyName);
            return PartialView("Test");
        }

        /*public ActionResult RetriveFile(int id)
        {
            _searchRepository = new SearchRepository();

            DataTable dt = new DataTable();
            dt = _searchRepository.ResumeRetrive(id);
            MemoryStream ms = new MemoryStream();
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, dt);
            byte[] image = null;
            image = ms.ToArray();
            return File(ms,"jpeg");
        }*/

        /*public ActionResult RetriveFile(int id)
        {
            _searchRepository = new SearchRepository();

            DataTable dt = new DataTable();
            dt = _searchRepository.ResumeRetrive(id);
            MemoryStream ms = new MemoryStream();
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms,dt);
            byte[] image = null;
            image = ms.ToArray();
            
            /*Image _image = Image.FromStream(ms);
            byte[] bt;
            bt = ms.ToArray();
            using (BinaryReader br = new BinaryReader(ms))
            {
                bt = br.ReadBytes((int) ms.Length);
            }
            return new FileContentResult(ms.ToArray(),"Image/Jpge");
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
            Response.BufferOutput = true;
            byte[] pdf;
            _searchRepository = new SearchRepository();
            Response.AppendHeader("Content-Length");
            Response.BinaryWrite(pdf);
            Response.End();
            return View();
            //return _image;
            //ResumeUpload resumeUpload = new ResumeUpload();
            return File(image, "image/png");
        }
        public FileContentResult GetImage(int id)
        {
            _searchRepository = new SearchRepository();

            DataTable dt = new DataTable();
            dt = _searchRepository.ResumeRetrive(id);
            MemoryStream ms = new MemoryStream();
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, dt);
            byte[] imageArray = ms.ToArray();
            return new FileContentResult(imageArray, "image/jpeg");
            //Response.ContentType = "image/jpeg";
            //Response.Write(imageArray);
        }*/
        
        [Authorize]
        [ValidateInput(false)]
        public ActionResult ApplyJob(ResumeUpload resumeUpload)
        {
            //if (ModelState.IsValid)
            //{
                HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    int candidateId = int.Parse(ticket.Name);

                    _searchRepository = new SearchRepository();
                    _userRepository = new UserRepository();
                    _jobSearch = new JobSearchController();
                    Core.Job j = new Core.Job();

                    if (resumeUpload.Photo != null)
                    {
                        string path = @"D:\Temp\";

                        /*if (resumeUpload.Photo.ContentLength > 10240){   
                        ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
                        return View();}*/
                        var supportedTypes = new[] {"jpg", "jpeg", "png", "txt", "doc", "pdf"};
                        var extension = System.IO.Path.GetExtension(resumeUpload.Photo.FileName);
                        if (extension != null)
                            resumeUpload.FileExt = extension.Substring(1);
                        resumeUpload.FileName = resumeUpload.Photo.FileName;

                        if (!supportedTypes.Contains(resumeUpload.FileExt))
                        {
                            ModelState.AddModelError("photo","Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        }
                        resumeUpload.Photo.SaveAs(path + resumeUpload.Photo.FileName);
                        path = resumeUpload.Photo.InputStream.ToString();

                        var target = new MemoryStream();
                        resumeUpload.Photo.InputStream.CopyTo(target);
                        resumeUpload.image = target.ToArray();
                    }

                    if (resumeUpload.user.UserName == null && resumeUpload.Photo == null)
                    {
                        //***********Add only Applied Job*************//
                        resumeUpload.ApplayJob.CandidateId = new MainUser();

                        resumeUpload.ApplayJob.CandidateId.Mid = candidateId;
                        _userRepository.ApplayToJob(resumeUpload);
                    }

                    else if(resumeUpload.IsUserAvailable == true && resumeUpload.user !=null && resumeUpload.Photo == null)
                    {
                        //***********update Candidate Detail*************//
                        resumeUpload.user.CandidateId = new MainUser();
                        resumeUpload.ApplayJob.CandidateId = new MainUser();
                        
                        resumeUpload.ApplayJob.CandidateId.Mid = candidateId;
                        resumeUpload.user.CandidateId.Mid = candidateId;
                        _userRepository.UpdateCandidationInformation(resumeUpload);
                        _userRepository.ApplayToJob(resumeUpload);
                    }

                    else if (resumeUpload.user.UserName == null && resumeUpload.Photo != null)
                    {
                        //***********Update Candidat Resume Only*************//
                        resumeUpload.ApplayJob.CandidateId = new MainUser();
                        resumeUpload.CandidateId = new MainUser();

                        resumeUpload.ApplayJob.CandidateId.Mid = candidateId;
                        resumeUpload.CandidateId.Mid = candidateId;
                        _searchRepository.UploadResume(resumeUpload);
                        _userRepository.ApplayToJob(resumeUpload);
                    }
                    else if(resumeUpload.IsUserAvailable == true && resumeUpload.user.UserName == null && resumeUpload.Photo != null)
                    {
                        //***********Update Candidate Detail and Update Candidate Resume Detail*************//
                        resumeUpload.user.CandidateId = new MainUser();
                        resumeUpload.ApplayJob.CandidateId = new MainUser();

                        resumeUpload.ApplayJob.CandidateId.Mid = candidateId;
                        resumeUpload.user.CandidateId.Mid = candidateId;
                        _userRepository.UpdateCandidationInformation(resumeUpload);
                        _searchRepository.UploadResume(resumeUpload);
                        _userRepository.ApplayToJob(resumeUpload);
                    }
                    else if(resumeUpload.IsUserAvailable == false && resumeUpload.user != null && resumeUpload.Photo != null)
                    {
                        //***********Add All information This is new Candidate*************//
                        resumeUpload.user.CandidateId = new MainUser();
                        resumeUpload.ApplayJob.CandidateId = new MainUser();
                        resumeUpload.CandidateId = new MainUser();
                        
                        resumeUpload.ApplayJob.CandidateId.Mid = candidateId;
                        resumeUpload.user.CandidateId.Mid = candidateId;
                        resumeUpload.CandidateId.Mid = candidateId;
                        _userRepository.AddCandidateInformation(resumeUpload.user);
                        _searchRepository.UploadResume(resumeUpload);
                        _userRepository.ApplayToJob(resumeUpload);
                    }
                    return View("AppliedJob");
                }
            //}
            UploadResume(3,"COOL");
            return View("UploadResume");
        }
        [Authorize]
        public ActionResult Test(int jobId, string companyName)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                int candidateId = int.Parse(ticket.Name);

                _searchRepository = new SearchRepository();
                _userRepository = new UserRepository();

                var resume = new ResumeUploadVM();
                resume.ResumeUpload = new ResumeUpload();
                Response.Cookies.Add(new HttpCookie("jobid", jobId.ToString()));
                Response.Cookies.Add(new HttpCookie("companyname",companyName));
                var httpCookie = Request.Cookies.Get("jobid");
                if (httpCookie != null)
                    resume.ResumeUpload.JobId = int.Parse(httpCookie.Value.ToString());

                var dataTable = new DataTable();
                dataTable = _searchRepository.IsResumeAvailable(1, candidateId);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var resumeUpload = new ResumeUpload(dataRow);
                    resume.ResumeUpload = resumeUpload;
                    resume.ResumeUpload.JobId = jobId;
                    if (resume.ResumeUpload.FileName != "")
                    {
                        resume.IsResumeAvailable = true;
                    }
                    else
                    {
                        resume.IsResumeAvailable = false;
                    }
                    resume.ResumeUpload.IsUserAvailable = true;
                }
                return PartialView("Test",resume);
            }
            return RedirectToAction("JobDetail", "JobSearch");
        }
    }
}

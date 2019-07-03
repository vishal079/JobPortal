using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.IO;
using System.Web.Helpers;
using System.Web.Mvc;
using JobPortal.Core;
using JobPortal.Core.Domain.User;

namespace JobPortal.Web
{
    public class ResumeUploadVM
    {
        public ResumeUpload ResumeUpload { get; set; }

        public ApplyJob ApplayJob { get; set; }

        public User User { get; set; }

        public List<User> Users { get; set; }

        public Core.Job Job { get; set; }
        
        public Boolean IsResumeAvailable { get; set; }

        /** Method to Display File to User */
        public FileContentResult getimg()
        {
            SearchRepository _searchRepository = new SearchRepository();

            DataTable dt = new DataTable();

            dt = _searchRepository.ResumeRetrive(6);

            MemoryStream ms = new MemoryStream();

            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, dt);
            ResumeUpload.image = ms.ToArray();
            return new FileContentResult(ResumeUpload.image, "image/jpeg");
        }
    }
}
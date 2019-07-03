﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using JobPortal.Core;


namespace JobPortal.Areas.Employer.Models
{
    using Core.Domain.User;

    public class EmployerVM
    {
        public Employer ObjEmployer { get; set; }

        public Core.Job ObjJob { get; set; }

        public List<Core.Job> Jobs { get; set; }

        public ApplyJob OApplayJob { get; set; }

        public List<ApplyJob> OApplayJobs { get; set; }

        public ResumeUpload Resume { get; set; }

        public List<ResumeUpload> ResumeList { get; set; }

        public List<long> ProfileId { get; set; } 

        public PersonalMessage PM { get; set; }

        public List<PersonalMessage> PMs { get; set; }

        public List<PersonalMessage> ReadMessages { get; set; }

        public List<PersonalMessage> UnReadMessges { get; set; }

        public IList<CandidatePersonal> Candidate { get; set; }

        public IList<Experience> Experiences { get; set; }

        public IList<Education> Educations { get; set; }

        public IList<Skill> Skills { get; set; } 

        public DataTable Image { get; set; }

        public List<string> MsgTitle { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobPortal.Core.Domain.User;

namespace JobPortal.Web.Areas.Profile.Models
{
    public class ProfileResume
    {

        public Core.Resume AddResume { get; set; }
        public Core.CandidatePersonal Candidate { get; set; }

        public Core.CandidatePersonal EditCandidate { get; set; }
        public Core.Experience EditExperience { get; set; }
        public Core.Education EditEducation { get; set; }
        public Core.Skill EditSkill { get; set; }
        public Core.Resume EditResume { get; set; }

        public Core.Job Job { get; set; }
       

        public List<Core.CandidatePersonal> CandidatePersonals { get; set; }
        public List<Core.Experience> Experiences { get; set; }
        public List<Core.Education> Educations { get; set; }
        public List<Core.Skill> Skills { get; set; }
        public List<Core.Resume> Resumes { get; set; }

        public List<Core.Job> MatchJobs { get; set; }

        public List<PersonalMessage> PersonalMessages { get; set; }

        public PersonalMessage Personal { get; set; }

        public List<string> JobTitle { get; set; }
    }
}
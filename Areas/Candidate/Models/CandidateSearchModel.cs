using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobPortal.Core;
using JobPortal.Core.Domain;


namespace JobPortal.Web
{

    public class CandidateSearchModel
    {
 
        //public Core.Candidate AddCandidate { get; set; }
        private readonly SearchRepository _searchRepository=new SearchRepository();

        

        private  readonly CandidateRepository _candidateRepository=new CandidateRepository();
        public Core.MainUser AddUser { get; set; }

        public Core.Resume AddResume { get; set; }
        public Core.CandidatePersonal AddPersonal { get; set; }
        
        public List<CandidatePersonal> PersonalList { get; set; }

        

 
       /* public List<CandidatePersonal>  GetAllPersonal()
        {
            PersonalList=new List<CandidatePersonal>();
            DataTable dt=new DataTable();
            dt = _candidateRepository.CandidateSearch();
            foreach (DataRow dr in dt.Rows)
            {
                CandidatePersonal cp=new CandidatePersonal(dr);
                PersonalList.Add(cp);
            }
            return PersonalList;
        }*/

      /*  public List<CandidatePersonal> GetPersonal(CandidateSearchModel sModel)
        {
            //sModel.AddPersonal =new CandidatePersonal();
            //sModel.AddPersonal.Experience =new Experience();
            //sModel.AddPersonal.Skill=new Skill();
            //sModel.AddPersonal.Education=new Education();

            sModel.AddPersonal = sModel.AddPersonal;
            PersonalList = new List<CandidatePersonal>();
            DataTable dt = new DataTable();
            dt = _candidateRepository.CandidateSearch(sModel.AddPersonal);
            foreach (DataRow dr in dt.Rows)
            {
                CandidatePersonal cp = new CandidatePersonal(dr);
                PersonalList.Add(cp);
            }
            return PersonalList;
        }*/


       

    }
}
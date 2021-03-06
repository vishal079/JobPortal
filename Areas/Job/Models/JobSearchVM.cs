﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using JobPortal.Core;

namespace JobPortal.Areas.Job.Models
{
    public class JobSearchVM
    {
        public Core.Job Job { get; set; }

        public List<Core.Job> Jobs { get; set; }

        public BasicSearch Basic { get; set; }

        public List<BasicSearch> BasicSearches { get; set; }

        public FavJob Fav { get; set; }

        public List<FavJob> FavJobs { get; set; }


        public JobSearchVM()
        {
            Fav = new FavJob();
            FavJobs = new List<FavJob>();
        }
    }

    public class RecentSearch
    {
        public string KeyTerms { get; set; }

        public string Location { get; set; }

        public List<RecentSearch> RecentSearches;

        public RecentSearch()
        {
            KeyTerms = string.Empty;
            Location = string.Empty;
        }
        public RecentSearch(string pKeyTerms,string pLocation)
        {
            KeyTerms = pKeyTerms;
            Location = pLocation;
        }
    }
}
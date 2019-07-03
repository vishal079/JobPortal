using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobPortal.Core;
using System.ComponentModel.DataAnnotations;
using JobPortal.Core.Domain.User;

namespace JobPortal.Web.Models
{
    public class UserVM
    {
        public Login user { get; set; }

        public Boolean IsLogedin { get; set; }

        public BasicSearch Basic { get; set; }

        public List<BasicSearch> BasicSearches { get; set; }

    }
}

﻿using System.Collections.Generic;

﻿    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using JobPortal.Core;
    using JobPortal.Core.Domain.User;



namespace JobPortal.Areas.Admin.Models
{
    using Core.Domain.User;

    public class AdminVM 
    {
        public Core.Job Job { get; set; }
        public List<Core.Job> Jobs { get; set; }
        public Employer Emplyer { get; set; }
        public List<Employer> Emplyers { get; set;}
        public List<Login> Logins { get; set; } 
        public Login model { get; set; }
       
    }
      
  
   

}
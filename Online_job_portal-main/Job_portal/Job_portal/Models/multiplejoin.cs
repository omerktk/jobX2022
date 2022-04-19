using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Job_portal.Models
{
    public class multiplejoin
    {
        public jobseeker_tb jobseekerTable { get; set; }
        public Company_tb companytable { get; set; }
        public ApplyJob_tb applytable { get; set; }


    }
}
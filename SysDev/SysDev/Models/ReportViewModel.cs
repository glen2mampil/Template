using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysDev.Models
{
    public class ReportViewModel : ViewModelBase
    {
        public List<AuditTrail> AuditTrails { get; set; }
        public List<ApplicationUser> UserProfiles { get; set; }

        public List<MasterDetail> Modules { get; set; }
    }

    
}
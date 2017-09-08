using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SysDev.Models
{
    public class ReportModels
    {

    }

    public class AuditTrail
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int PageId { get; set; }
        public string Action { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class MasterData
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Status { get; set; }
        public int OrderNumber { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeUpdated { get; set; }
    }

    public class MasterDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int OrderNumber { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeUpdated { get; set; }

        public int MasterDataId { get; set; }
        public MasterData MasterData { get; set; }
    }
}
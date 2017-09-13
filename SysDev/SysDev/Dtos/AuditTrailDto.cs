using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SysDev.Models;

namespace SysDev.Dtos
{
    public class AuditTrailDto
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int PageId { get; set; }
        public string Action { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
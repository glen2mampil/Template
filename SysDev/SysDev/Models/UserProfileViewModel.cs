using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SysDev.Models
{
    [Authorize]
    public class UserProfileViewModel
    {
        public List<UserProfile> UserProfiles { get; set; }

        public List<ApplicationUser> Accounts { get; set; }
    }

    public class AddUserViewModel
    {
        public UserProfile Profile { get; set; }

        public ApplicationUser Account { get; set; }

        public string Password { get; set; }

        public string MaritalStatus { get; set; }
    }

    public class UserPermissionViewModel
    {
        public List<Permission> Permissions { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }

    public class DetailedPermissionViewModel
    {
        public IEnumerable<Permission> Permissions { get; set; }
        public IdentityRole Role { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SysDev.Models
{
    [Authorize]
    public class UserProfileViewModel : ViewModelBase
    {
        public List<UserProfile> UserProfiles { get; set; }

        public List<ApplicationUser> Accounts { get; set; }

        public UserProfile EditProfile { get; set; }

        public ApplicationUser EditAccount { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }

    //soon to delete
    public class AddUserViewModel : ViewModelBase
     { 
        public UserProfile Profile { get; set; }
        public string Password { get; set; }
    }


    public class UserPermissionViewModel : ViewModelBase
    {
        public List<Permission> Permissions { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }

    public class DetailedPermissionViewModel : ViewModelBase
    {
        public IEnumerable<Permission> Permissions { get; set; }
        public IdentityRole Role { get; set; }
        public List<MasterDetail> MasterDetails { get; set; }
    }
}
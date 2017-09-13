using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SysDev.Models;

namespace SysDev.Controllers.Api
{
    public class UsersController : ApiController
    {
        private ApplicationDbContext _context;

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/customers
        public UserProfileViewModel GetUsers(string query = null)
        {
            //return _context.Users.ToList();
            //return _context.UserProfiles.ToList();
            var users = _context.UserProfiles.ToList();
            //var accounts = _context.Users.ToList();
            var useraccount = new UserProfileViewModel
            {
                UserProfiles = users,
                //Accounts = accounts,
                AccountRole = ""
            };
            return useraccount;
        }

        // GET /api/customers/1
        //public AuditTrail CreateAu
    }
}

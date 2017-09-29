using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SysDev.Models;

namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SysDev.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SysDev.Models.ApplicationDbContext context)
        {
            Console.WriteLine("Initial seeding");
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            if (!roleManager.RoleExists("SuperAdmin"))
            {
                //Create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole {Name = "SuperAdmin"};
                roleManager.Create(role);

                //Create a Admin super user who will maintain the website                  
                var profile = new UserProfile
                {
                    FirstName = "Glenn",
                    LastName = "Tumampil",
                    MiddleName = "Mendez",
                    MaritalStatus = "Single",
                    Address = "Makati City",
                    Gender = "Male",
                    ContactNo = "09770975881",
                    CompanyName = "Bluebell corp",
                    DateCreated = DateTime.Now.ToString(),
                    CompanyId = "10000010"

                };
                context.UserProfiles.Add(profile);


                var user = new ApplicationUser
                {
                    UserName = "suadmin2017",
                    Email = "SuAdmin@Bluebell.com",
                    UserProfileId = profile.Id,
                    UserProfile = profile,
                    PhoneNumber = profile.ContactNo,
                    Status = "Active"
                };

                string userPWD = "admin123";

                var chkUser = userManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "SuperAdmin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole {Name = "Manager"};
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = "Employee" };
                roleManager.Create(role);

            }

            //context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

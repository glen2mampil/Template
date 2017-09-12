using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SysDev.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Status { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public override string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public override string UserName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Contact No.")]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Id")]
        public string CompanyId { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Date and Time Created")]
        public string DateCreated { get; set; }
    }

    public class Permission
    {
        public int Id { get; set; }
        public string IdentityRoleId { get; set; }
        public IdentityRole IdentityRole { get; set; }

        public int MasterDetailId { get; set; }
        public MasterDetail MasterDetail { get; set; }

        public int AllowView { get; set; }
        public int AllowCreate { get; set; }
        public int AllowEdit { get; set; }
        public int AllowDelete { get; set; }
        public int AllowGenerateReport { get; set; }

    }



    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }

        public DbSet<MasterData> MasterDatas { get; set; }
        public DbSet<MasterDetail> MasterDetails { get; set; }
        public ApplicationDbContext()
            : base("name=DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
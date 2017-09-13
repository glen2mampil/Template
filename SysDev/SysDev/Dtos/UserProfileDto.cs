using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SysDev.Dtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyId { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string MaritalStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string DateCreated { get; set; }
    }
}
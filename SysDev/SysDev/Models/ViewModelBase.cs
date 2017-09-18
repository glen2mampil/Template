using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysDev.Models
{
    public abstract class ViewModelBase
    {
        public ApplicationUser Account { get; set; }
        public string AccountRole { get; set; }

        public Permission Permission { get; set; }      
    }

}
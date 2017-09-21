using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysDev.App_Start
{
    public class ApplicationConfig
    {

    }

    public class RoleName
    {
        public const string SuperAdmin = "SuperAdmin";
    }

    public class Page
    {
        public const string Users = "Users";
        public const string UserPermission = "User Permission";
        public const string Settings = "Settings";
        public const string AuditTrail = "Audit Trail";
    }

    public class UserAction
    {
        public const string Save = "Save";
        public const string Edit = "Edit";
        public const string View = "View";
        public const string Create = "Create";
        public const string Print = "Print";
        public const string Update = "Update";
        public const string Delete = "Delete";
    }
}

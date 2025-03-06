using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using HRMS.Models;
using System.Web.Mvc;

namespace HRMS
{
    public class Constant
    {
        public const string BE = "BE";
        public const string BU = "BU";
        public const string Username = "Username";
        public const string Role = "Role";
        public const string RoleID = "RoleID";
        public const string EmpName = "EmpName";
        public const string EmpCode = "EmpCode";
        public static int SessionTime = 30; // Minutes
        public static string SessionUserEmpCode = "SessionUserEmpCode";

        public const string Insert = "Inserted Successfully";
        public const string Update = "Updated Successfully";
        public const string Delete = "Deleted Successfully";

        public static DateTime Date()
        {
            return DateTime.UtcNow.AddHours(5).AddMinutes(27);
        }
    }
}

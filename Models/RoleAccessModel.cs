using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models
{

    public class Role
    {
        public int ROLE_ID { get; set; }
        public string ROLE_DESC { get; set; }
    }

    public class RoleAccess
    {
        public int RA_FORM_ID { get; set; }
        public string RA_FORM_NAME { get; set; }
        public bool RA_FLAG { get; set; }
    }

    public class RoleAccessModel
    {
        public Role RoleData { get; set; }
        public List<RoleAccess> RoleAccessList { get; set; }

        public RoleAccessModel()
        {
            RoleData = new Role();
            RoleAccessList = new List<RoleAccess>();
        }
    }
}
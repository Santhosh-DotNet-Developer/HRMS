using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    [CheckSessionTimeOut]
    public class DashboardController : Controller
    {
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoManualAttendance
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Manual Attendance
        public List<MANUAL_ATTENDANCE> GetList()
        {
            List<MANUAL_ATTENDANCE> manualAttdList = new List<MANUAL_ATTENDANCE>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                manualAttdList = db.MANUAL_ATTENDANCE.Where(w => w.MA_BE == BE && w.MA_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoManualAttendance - GetList : " + ex.Message);
            }
            return manualAttdList;
        }
        #endregion

        #region Get Details for Manual Attendance
        public MANUAL_ATTENDANCE GetDetails(int manualAttdId)
        {
            MANUAL_ATTENDANCE manualAttdDet = new MANUAL_ATTENDANCE();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (manualAttdId > 0)
                {
                    manualAttdDet = db.MANUAL_ATTENDANCE.Where(w => w.MA_BE == BE && w.MA_BU == BU && w.MA_ID == manualAttdId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoManualAttendance - GetDetails : " + ex.Message);
            }
            return manualAttdDet;
        }
        #endregion

        #region Save for Manual Attendance
        public string Save(MANUAL_ATTENDANCE manualAttd)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
            try
            {
                manualAttd.MA_BE = BE;
                manualAttd.MA_BU = BU;
                manualAttd.MA_EMP_CODE = empCode;
                manualAttd.MA_CRE_BY = user;
                manualAttd.MA_CRE_DATE = DateTime.Now;
                db.MANUAL_ATTENDANCE.Add(manualAttd);
                db.SaveChanges();
                message = "Insert";
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoManualAttendance - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Manual Attendance Exists
        public bool CheckAttendanceExists(DateTime date)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
            try
            {
                result = db.MANUAL_ATTENDANCE.Where(w => w.MA_BE == BE && w.MA_BU == BU && w.MA_EMP_CODE == empCode && w.MA_DATE == date).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoManualAttendance - CheckAttendanceExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
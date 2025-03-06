using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoLeave
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Leave
        public List<LEAVE> GetList()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            List<LEAVE> leaveList = new List<LEAVE>();
            try
            {
                leaveList = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeave - GetList : " + ex.Message);
            }
            return leaveList;
        }
        #endregion

        #region Get Details for Leave
        public LEAVE GetDetails(string leaveCode)
        {
            LEAVE leaveDet = new LEAVE();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (leaveCode != "")
                {
                    leaveDet = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU && w.LEV_CODE == leaveCode).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeave - GetDetails : " + ex.Message);
            }
            return leaveDet;
        }
        #endregion

        #region Save for Leave
        public string Save(LEAVE leave)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                if (leave.LEV_CRE_BY == null)
                {
                    leave.LEV_BE = BE;
                    leave.LEV_BU = BU;
                    leave.LEV_CRE_BY = User;
                    leave.LEV_CRE_DATE = DateTime.Now;
                    db.LEAVEs.Add(leave);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    LEAVE leaveDet = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU && w.LEV_CODE == leave.LEV_CODE).SingleOrDefault();
                    leaveDet.LEV_NAME = leave.LEV_NAME;
                    leaveDet.LEV_UPD_BY = User;
                    leaveDet.LEV_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeave - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete for Leave
        public string Delete(string leaveCode)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                LEAVE leaveDet = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU && w.LEV_CODE == leaveCode).SingleOrDefault();
                if(leaveDet != null)
                {
                    db.LEAVEs.Remove(leaveDet);
                    db.SaveChanges();
                    message = "Delete";
                }
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoLeave - Delete : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Code Exists
        public bool CheckCodeExists(string leaveCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU && w.LEV_CODE == leaveCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeave - CheckCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Leave Exists
        public bool CheckLeaveExists(string leave, string leaveCode, string creBy)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (creBy != "")
                {
                    result = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU && w.LEV_CODE != leaveCode && w.LEV_NAME == leave).Count() > 0;
                }
                else
                {
                    result = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU && w.LEV_NAME == leave).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeave - CheckLeaveExists : " + ex.Message);
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
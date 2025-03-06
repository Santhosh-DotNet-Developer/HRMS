using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoLeaveRequest
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Leave Request/History
        public LeaveRequestModel GetList()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string role = HttpContext.Current.Request.Cookies[Constant.Role].Value.ToLower();
            string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
            LeaveRequestModel levReq = new LeaveRequestModel();
            try
            {
                if (role == "employee")
                {
                    levReq.LeaveRequest = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS == "N" && w.LR_EMP_CODE == empCode).ToList();
                    levReq.LeaveRequestHist = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS != "N" && w.LR_EMP_CODE == empCode).OrderByDescending(o => o.LR_CRE_DATE).ToList();
                }
                else if (role == "manager")
                {
                    string managerDept = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == empCode).Select(s => s.EMP_DEPT_CODE).Single();
                    levReq.LeaveRequest = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS == "N" && w.LR_EMP_DEPT_CODE == managerDept).ToList();
                    levReq.LeaveRequestHist = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS != "N" && w.LR_EMP_DEPT_CODE == managerDept).OrderByDescending(o => o.LR_CRE_DATE).ToList();
                }
                else if (role == "hr manager")
                {
                    levReq.LeaveRequest = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS == "MA").ToList();
                    levReq.LeaveRequestHist = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS != "N").OrderByDescending(o => o.LR_CRE_DATE).ToList();
                }
                else
                {
                    levReq.LeaveRequest = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS == "N").ToList();
                    levReq.LeaveRequestHist = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_STATUS != "N").OrderByDescending(o => o.LR_CRE_DATE).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeaveRequest - GetList : " + ex.Message);
            }
            return levReq;
        }
        #endregion

        #region Get Details for Leave Request
        public LEAVE_REQUEST GetDetails(int levReqId)
        {
            LEAVE_REQUEST levReqDet = new LEAVE_REQUEST();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (levReqId > 0)
                {
                    levReqDet = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_ID == levReqId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeaveRequest - GetDetails : " + ex.Message);
            }
            return levReqDet;
        }
        #endregion

        #region Save for Leave Request
        public string Save(LEAVE_REQUEST levReq)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
            string empDept = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == empCode).Select(s => s.EMP_DEPT_CODE).Single();
            try
            {
                levReq.LR_BE = BE;
                levReq.LR_BU = BU;
                levReq.LR_EMP_CODE = empCode;
                levReq.LR_EMP_DEPT_CODE = empDept;
                levReq.LR_STATUS = "N";
                levReq.LR_CRE_BY = user;
                levReq.LR_CRE_DATE = DateTime.Now;
                db.LEAVE_REQUEST.Add(levReq);
                db.SaveChanges();
                message = "Success";
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoLeaveRequest - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Cancel/Reject
        public string CancelReject(LEAVE_REQUEST levReq, string type)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
            try
            {
                LEAVE_REQUEST levReqDet = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_ID == levReq.LR_ID).SingleOrDefault();
                if (levReqDet != null)
                {
                    if (type == "Cancel")
                    {
                        levReqDet.LR_STATUS = "C";
                        levReqDet.LR_REJ_REASON = levReq.LR_REJ_REASON;
                        levReqDet.LR_REJ_BY = empCode;
                    }
                    else if (type == "Reject")
                    {
                        levReqDet.LR_STATUS = "R";
                        levReqDet.LR_REJ_REASON = levReq.LR_REJ_REASON;
                        levReqDet.LR_REJ_BY = empCode;
                    }
                    levReqDet.LR_UPD_BY = user;
                    levReqDet.LR_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    if (type == "Cancel")
                    {
                        message = "Cancel";
                    }
                    else if (type == "Reject")
                    {
                        message = "Reject";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeaveRequest - CancelReject : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Approve Leave Request
        public string Approve(int levReqId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
                LEAVE_REQUEST levReqDet = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_ID == levReqId).SingleOrDefault();
                if (levReqDet != null)
                {
                    levReqDet.LR_STATUS = "A";
                    levReqDet.LR_APR_BY = empCode;
                    levReqDet.LR_UPD_BY = User;
                    levReqDet.LR_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Approve";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeaveRequest - Approve : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Leave Exists
        public bool CheckLeaveExists(string empCode, DateTime leaveDate)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.LEAVE_REQUEST.Where(w => w.LR_BE == BE && w.LR_BU == BU && w.LR_EMP_CODE == empCode && (w.LR_START_DATE == leaveDate || w.LR_END_DATE == leaveDate)).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoLeaveRequest - CheckLeaveExists : " + ex.Message);
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
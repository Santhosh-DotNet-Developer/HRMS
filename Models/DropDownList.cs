using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Repository;

namespace HRMS.Models
{
    public class DropDownList
    {
        public static HRMSEntities db = new HRMSEntities();
        public static Logger log = new Logger();

        public static IEnumerable GetCities()
        {
            var lst = db.CITIES.AsEnumerable().Select(s => new { id = s.CITY_ID.ToString(), text = s.CITY_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetStates()
        {
            var lst = db.STATES.Select(o => new { id = o.STATE_CODE, text = o.STATE_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetCountries()
        {
            var lst = db.COUNTRIES.Select(o => new { id = o.CNTRY_CODE, text = o.CNTRY_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        //public static string GetCountryName(string inCountryCode)
        //{
        //    string inCountryName = db.COUNTRIES.Where(w => w.CNTRY_CODE == inCountryCode).Select(s => s.CNTRY_NAME).SingleOrDefault();
        //    return inCountryName;
        //}

        //public static string GetStateName(string inStateCode)
        //{
        //    string inStateName = db.STATES.Where(w => w.STATE_CODE == inStateCode).Select(s => s.STATE_NAME).SingleOrDefault();
        //    return inStateName;
        //}

        //public static string GetCityName(int inCityId)
        //{
        //    string inCityName = db.CITIES.Where(w => w.CITY_ID == inCityId).Select(s => s.CITY_NAME).SingleOrDefault();
        //    return inCityName;
        //}

        public static IEnumerable GetUnits()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            var lst = db.BUS_UNITS.Where(w => w.BU_BE == BE).AsEnumerable().Select(o => new { id = o.BU_ID.ToString(), text = o.BU_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetEmployees()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_STATUS == true).Select(s => new { id = s.EMP_CODE, text = s.EMP_FIRST_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static string GetEmployeeName(string inEmpCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string inEmpName = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == inEmpCode).Select(s => s.EMP_FIRST_NAME).SingleOrDefault();
            return inEmpName;
        }

        public static string GetEmployeeCodeName(string inEmpCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string inEmpName = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == inEmpCode).Select(s => s.EMP_CODE + " - " + s.EMP_FIRST_NAME).SingleOrDefault();
            return inEmpName;
        }

        public static IEnumerable GetRoles()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU).AsEnumerable().Select(s => new { id = s.ROLE_ID.ToString(), text = s.ROLE_DESC }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static string GetRoleDesc(int inRoleId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string inRoleDesc = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_ID == inRoleId).Select(s => s.ROLE_DESC).SingleOrDefault();
            return inRoleDesc;
        }

        public static IEnumerable GetBanks()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            var lst = db.BANKS.Where(w => w.BANK_BE == BE).Select(s => new { id = s.BANK_CODE, text = s.BANK_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static string GetBankName(string inBankCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string inBankName = db.BANKS.Where(w => w.BANK_BE == BE && w.BANK_CODE == inBankCode).Select(s => s.BANK_NAME).SingleOrDefault();
            return inBankName;
        }

        public static string GetUnitName(Int16 inBUId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            string inUnitName = db.BUS_UNITS.Where(w => w.BU_BE == BE && w.BU_ID == inBUId).Select(s => s.BU_NAME).SingleOrDefault();
            return inUnitName;
        }

        public static IEnumerable GetValues(string inTypeId)
        {
            var lst = db.DATA_MANAGER.Where(w => w.TYPE_ID == inTypeId).Select(s => new { id = s.VALUE_ID, text = s.VALUE_DESC }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetLanguages(string inTypeId)
        {
            var lst = db.DATA_MANAGER.Where(w => w.TYPE_ID == inTypeId).Select(s => new { id = s.VALUE_ID, text = s.VALUE_DESC }).ToList();
            return lst;
        }

        public static string GetValueDescription(string inValueId, string inTypeId)
        {
            string inValueDesc = db.DATA_MANAGER.Where(w => w.VALUE_ID == inValueId && w.TYPE_ID == inTypeId).Select(s => s.VALUE_DESC).SingleOrDefault();
            return inValueDesc;
        }

        public static IEnumerable GetFinYear()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            var lst = db.FINANCIAL_YEAR.Where(w => w.FY_BE == BE).AsEnumerable().Select(s => new { id = s.FY_YEAR.ToString(), text = s.FY_YEAR.ToString() }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetDepartments()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU).Select(s => new { id = s.DEPT_CODE, text = s.DEPT_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static string GetDeptName(string inDeptCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string inDeptName = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU && w.DEPT_CODE == inDeptCode).Select(s => s.DEPT_NAME).SingleOrDefault();
            return inDeptName;
        }

        public static IEnumerable GetDesignations()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU).Select(s => new { id = s.DESG_CODE, text = s.DESG_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static string GetDesgName(string inDesgCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string inDesgName = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU && w.DESG_CODE == inDesgCode).Select(s => s.DESG_NAME).SingleOrDefault();
            return inDesgName;
        }

        public static IEnumerable GetSalaryGroup()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU).Select(s => new { id = s.SG_CODE, text = s.SG_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetEmployeeGrade()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == BU).Select(s => new { id = s.EG_CODE, text = s.EG_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static string GetSalaryGroupName(string inSalaryGrpCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string inSalaryGrpName = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU && w.SG_CODE == inSalaryGrpCode).Select(s => s.SG_NAME).SingleOrDefault();
            return inSalaryGrpName;
        }

        public static IEnumerable GetBankBranches()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.BANK_BRANCHES.AsEnumerable().Where(w => w.BB_BE == BE && w.BB_BU == BU).Select(s => new { id = s.BB_ID.ToString(), text = s.BB_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetLeaveTypes()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            var lst = db.LEAVEs.Where(w => w.LEV_BE == BE && w.LEV_BU == BU).Select(s => new { id = s.LEV_CODE, text = s.LEV_NAME }).ToList();
            lst.Insert(0, new { id = "", text = "" });
            return lst;
        }

        public static IEnumerable GetStateCountry(int cityId)
        {
            List<string> list = new List<string>();
            try
            {
                var stateCntryDet = db.VIEW_CITY_STATE_COUNTRY.Where(w => w.CITY_ID == cityId).Select(s => new { s.STATE_CODE, s.CNTRY_CODE }).SingleOrDefault();
                list.Add(stateCntryDet.STATE_CODE);
                list.Add(stateCntryDet.CNTRY_CODE);
            }
            catch (Exception ex)
            {
                log.Debug("Exception in DropDownList - GetStateCountry : " + ex.Message);
            }
            return list;
        }

        #region Dispose
        private bool disposed = false;
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

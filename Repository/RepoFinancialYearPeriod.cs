using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoFinancialYearPeriod
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Financial Year
        public List<FINANCIAL_YEAR> GetList()
        {
            List<FINANCIAL_YEAR> finYearList = new List<FINANCIAL_YEAR>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                finYearList = db.FINANCIAL_YEAR.Where(w => w.FY_BE == BE).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoFinancialYearPeriod - GetList : " + ex.Message);
            }
            return finYearList;
        }
        #endregion

        #region Get Details for Financial Year/Period
        public Tuple<FINANCIAL_YEAR, List<FINANCIAL_PERIOD>> GetDetails(int year)
        {
            FINANCIAL_YEAR finYearDet = new FINANCIAL_YEAR();
            List<FINANCIAL_PERIOD> finPeriodLst = new List<FINANCIAL_PERIOD>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            try
            {
                if (year > 0)
                {
                    finYearDet = db.FINANCIAL_YEAR.Where(w => w.FY_BE == BE && w.FY_YEAR == year).SingleOrDefault();
                    finPeriodLst = db.FINANCIAL_PERIOD.Where(w => w.FP_BE == BE && w.FP_YEAR == year).ToList();
                }
                else
                {
                    db.Database.ExecuteSqlCommand("EXEC GENERATE_FINANCIAL_YEAR_AND_PERIOD '" + User + "'");
                    int finYear = db.FINANCIAL_YEAR.Where(w => w.FY_BE == BE).Max(m => m.FY_YEAR);
                    finYearDet = db.FINANCIAL_YEAR.Where(w => w.FY_BE == BE && w.FY_YEAR == finYear).SingleOrDefault();
                    finPeriodLst = db.FINANCIAL_PERIOD.Where(w => w.FP_BE == BE && w.FP_YEAR == finYear).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoFinancialYearPeriod - GetSingle : " + ex.Message);
            }
            return new Tuple<FINANCIAL_YEAR, List<FINANCIAL_PERIOD>>(finYearDet, finPeriodLst);
        }
        #endregion

        #region Update
        public string Update(FINANCIAL_YEAR finYear, List<FINANCIAL_PERIOD> finPeriodLst)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;

                FINANCIAL_YEAR finYearDet = db.FINANCIAL_YEAR.Where(w => w.FY_BE == BE & w.FY_YEAR == finYear.FY_YEAR).SingleOrDefault();
                finYearDet.FY_STATUS = finYear.FY_STATUS;
                finYearDet.FY_UPD_BY = User;
                finYearDet.FY_UPD_DATE = DateTime.Now;
                db.SaveChanges();

                for (int i = 0; i < finPeriodLst.Count(); i++)
                {
                    List<FINANCIAL_PERIOD> finPeriodList = db.FINANCIAL_PERIOD.Where(w => w.FP_BE == BE && w.FP_YEAR == finYear.FY_YEAR).ToList();
                    for (int j = 0; j < finPeriodList.Count(); j++)
                    {
                        if (finPeriodLst[i].FP_PERIOD == finPeriodList[j].FP_PERIOD)
                        {
                            Int16 finPeriod = finPeriodLst[i].FP_PERIOD;
                            FINANCIAL_PERIOD inFinPeriod = db.FINANCIAL_PERIOD.Where(w => w.FP_BE == BE && w.FP_YEAR == finYear.FY_YEAR && w.FP_PERIOD == finPeriod).SingleOrDefault();
                            inFinPeriod.FP_STATUS = finPeriodLst[j].FP_STATUS;
                            inFinPeriod.FP_UPD_BY = User;
                            inFinPeriod.FP_UPD_DATE = DateTime.Now;
                            db.SaveChanges();
                            break;
                        }
                    }
                }
                message = "Update";
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoFinancialYearPeriod - Update : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Status
        public bool CheckStatus(string status)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            try
            {
                result = db.FINANCIAL_YEAR.Where(w => w.FY_BE == BE & w.FY_STATUS == status).Count() > 0;
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoFinancialYearPeriod - CheckStatus : " + ex.Message);
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
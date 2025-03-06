using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoWorkDayCalendar
    {
        #region Declaration
        string message = "";
        bool result = false;
        Logger log = new Logger();
        public WORK_DAY_CALENDAR_HD hdSingle { get; set; }
        public List<WORK_DAY_CALENDAR_LN> lnLst { get; set; }
        public WORK_DAY_CALENDAR_LN lnSingle { get; set; }
        public List<WORK_DAY_CALENDAR_HOLIDAYS> holidayLst { get; set; }
        public WORK_DAY_CALENDAR_HOLIDAYS holidaySingle { get; set; }
        public bool? Sunday { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        #endregion

        #region GetList
        public List<WORK_DAY_CALENDAR_HD> GetList()
        {
            List<WORK_DAY_CALENDAR_HD> lst = new List<WORK_DAY_CALENDAR_HD>();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                using(HRMSEntities db = new HRMSEntities())
                {
                    lst = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_BE == BE && w.WDCH_BU == BU).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - GetList : " + ex.Message);
            }
            return lst;
        }
        #endregion

        #region GetSingle
        public RepoWorkDayCalendar GetSingle(int docNo)
        {
            RepoWorkDayCalendar rp = new RepoWorkDayCalendar();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                using(HRMSEntities db = new HRMSEntities())
                {
                    if (docNo == 0)
                    {
                        rp.hdSingle = new WORK_DAY_CALENDAR_HD();
                    }
                    else
                    {
                        rp.hdSingle = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_BE == BE && w.WDCH_BU == BU && w.WDCH_DOC_NO == docNo).SingleOrDefault();
                        rp.lnLst = db.WORK_DAY_CALENDAR_LN.Where(w => w.WDCL_BE == BE && w.WDCL_BU == BU && w.WDCL_DOC_NO == docNo).ToList();
                        rp.holidayLst = db.WORK_DAY_CALENDAR_HOLIDAYS.Where(w => w.WDCHO_BE == BE && w.WDCHO_BU == BU && w.WDCHO_DOC_NO == docNo).ToList();

                        rp.Sunday = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == docNo && w.WE_DAY == "SU").Select(s => s.WE_DAY_CK).SingleOrDefault();
                        rp.Monday = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == docNo && w.WE_DAY == "MO").Select(s => s.WE_DAY_CK).SingleOrDefault();
                        rp.Tuesday = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == docNo && w.WE_DAY == "TU").Select(s => s.WE_DAY_CK).SingleOrDefault();
                        rp.Wednesday = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == docNo && w.WE_DAY == "WE").Select(s => s.WE_DAY_CK).SingleOrDefault();
                        rp.Thursday = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == docNo && w.WE_DAY == "TH").Select(s => s.WE_DAY_CK).SingleOrDefault();
                        rp.Friday = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == docNo && w.WE_DAY == "FR").Select(s => s.WE_DAY_CK).SingleOrDefault();
                        rp.Saturday = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == docNo && w.WE_DAY == "SA").Select(s => s.WE_DAY_CK).SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - GetSingle : " + ex.Message);
            }
            return rp;
        }
        #endregion

        #region Get Holiday Single
        public WORK_DAY_CALENDAR_HOLIDAYS GetHolidaySingle(int docNo, int seqNo)
        {
            WORK_DAY_CALENDAR_HOLIDAYS holiday = null;
            try
            {
                holiday = new WORK_DAY_CALENDAR_HOLIDAYS();
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                using (HRMSEntities db = new HRMSEntities())
                {
                    if (seqNo == 0)
                    {
                        holiday = new WORK_DAY_CALENDAR_HOLIDAYS();
                        holiday.WDCHO_DOC_NO = docNo;
                    }
                    else
                    {
                        holiday = db.WORK_DAY_CALENDAR_HOLIDAYS.Where(w => w.WDCHO_BE == BE && w.WDCHO_BU == BU && w.WDCHO_DOC_NO == docNo && w.WDCHO_SEQ_NO == seqNo).SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - GetSingle : " + ex.Message);
            }
            return holiday;
        }
        #endregion

        #region Save Header
        public string SaveHeader(RepoWorkDayCalendar Data)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                int docNo = 0;
                using (HRMSEntities db = new HRMSEntities())
                {
                    if (Data.hdSingle.WDCH_DOC_NO == 0)
                    {
                        docNo = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_BE == BE && w.WDCH_BU == BU).Select(s => s.WDCH_DOC_NO).DefaultIfEmpty(0).Max();
                        docNo = docNo + 1;
                        WORK_DAY_CALENDAR_HD ins = new WORK_DAY_CALENDAR_HD();
                        ins.WDCH_BE = BE;
                        ins.WDCH_BU = Data.hdSingle.WDCH_BU;
                        ins.WDCH_DOC_NO = docNo;
                        ins.WDCH_YEAR = Data.hdSingle.WDCH_YEAR;
                        ins.WDCH_START_DATE = Data.hdSingle.WDCH_START_DATE;
                        ins.WDCH_END_DATE = Data.hdSingle.WDCH_END_DATE;
                        ins.WDCH_STATUS = "N";
                        ins.WDCH_CRE_BY = User;
                        ins.WDCH_CRE_DATE = DateTime.Now;
                        db.WORK_DAY_CALENDAR_HD.Add(ins);
                        db.SaveChanges();

                        for (int i = 1; i <= 7; i++)
                        {
                            WEEK_ENDS we = new WEEK_ENDS();
                            we.WE_BE = BE;
                            we.WE_BU = Data.hdSingle.WDCH_BU;
                            we.WE_YEAR = Data.hdSingle.WDCH_YEAR;
                            we.WE_CAL_NO = docNo;
                            if (i == 1)
                                we.WE_DAY = "SU";
                            else if (i == 2)
                                we.WE_DAY = "MO";
                            else if (i == 3)
                                we.WE_DAY = "TU";
                            else if (i == 4)
                                we.WE_DAY = "WE";
                            else if (i == 5)
                                we.WE_DAY = "TH";
                            else if (i == 6)
                                we.WE_DAY = "FR";
                            else if (i == 7)
                                we.WE_DAY = "SA";
                            we.WE_DAY_CK = false;
                            we.WE_CRE_BY = User;
                            we.WE_CRE_DATE = DateTime.Now;
                            db.WEEK_ENDS.Add(we);
                            db.SaveChanges();
                        }
                        message = "Insert";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - SaveHeader : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Generate Calendar
        public string GenerateCalendar(int docNo)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                using (HRMSEntities db = new HRMSEntities())
                {
                    if (docNo > 0)
                    {
                        db.Database.ExecuteSqlCommand("EXEC GENERATE_WORK_DAY_CALENDAR '" + BE + "','" + BU + "' ,'" + docNo + "'");
                    }
                }
                message = "Generated";
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - GenerateCalendar : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save Holiday
        public string SaveHoliday(WORK_DAY_CALENDAR_HOLIDAYS Data)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                using (HRMSEntities db = new HRMSEntities())
                {
                    if (Data.WDCHO_SEQ_NO == 0)
                    {
                        var year = db.WORK_DAY_CALENDAR_HD.Where(s => s.WDCH_BE == BE && s.WDCH_BU == BU && s.WDCH_DOC_NO == Data.WDCHO_DOC_NO).Select(s => s.WDCH_YEAR).FirstOrDefault();
                        int seqNo = db.WORK_DAY_CALENDAR_HOLIDAYS.Where(w => w.WDCHO_BE == BE && w.WDCHO_BU == BU && w.WDCHO_DOC_NO == Data.WDCHO_DOC_NO).Select(s => s.WDCHO_SEQ_NO).DefaultIfEmpty(0).Max();
                        WORK_DAY_CALENDAR_HOLIDAYS ins = new WORK_DAY_CALENDAR_HOLIDAYS();
                        ins.WDCHO_BE = BE;
                        ins.WDCHO_BU = BU;
                        ins.WDCHO_YEAR = year;
                        ins.WDCHO_DOC_NO = Data.WDCHO_DOC_NO;
                        ins.WDCHO_SEQ_NO = seqNo + 1;
                        ins.WDCHO_HOLIDAY = Data.WDCHO_HOLIDAY;
                        ins.WDCHO_REF = Data.WDCHO_REF;
                        ins.WDCHO_CRE_BY = User;
                        ins.WDCHO_CRE_DATE = DateTime.Now;
                        db.WORK_DAY_CALENDAR_HOLIDAYS.Add(ins);
                        db.SaveChanges();
                        message = "Insert";
                    }
                    else
                    {
                        WORK_DAY_CALENDAR_HOLIDAYS upd = db.WORK_DAY_CALENDAR_HOLIDAYS.Where(e => e.WDCHO_BE == BE && e.WDCHO_BU == BU && e.WDCHO_DOC_NO == Data.WDCHO_DOC_NO && e.WDCHO_SEQ_NO == Data.WDCHO_SEQ_NO).SingleOrDefault();
                        upd.WDCHO_REF = Data.WDCHO_REF;
                        upd.WDCHO_UPD_BY = Data.WDCHO_UPD_BY;
                        upd.WDCHO_UPD_DATE = Data.WDCHO_UPD_DATE;
                        db.SaveChanges();
                        message = "Update";
                    }

                    #region Update Work Day Calendar LN
                    WORK_DAY_CALENDAR_LN updData = db.WORK_DAY_CALENDAR_LN.Where(w => w.WDCL_BE == BE && w.WDCL_BU == BU && w.WDCL_DAY == Data.WDCHO_HOLIDAY && w.WDCL_DOC_NO == Data.WDCHO_DOC_NO).SingleOrDefault();
                    updData.WDCL_DAY_TYPE = "H";
                    updData.WDCL_REF = Data.WDCHO_REF;
                    updData.WDCL_WORK_OFF = true;
                    db.SaveChanges();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - SaveHoliday : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region UpdateWeekOff
        public string UpdateWeekOff(RepoWorkDayCalendar Data)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                using (HRMSEntities db = new HRMSEntities())
                {
                    List<WEEK_ENDS> lst = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == Data.hdSingle.WDCH_DOC_NO).ToList();
                    foreach (var day in lst)
                    {
                        WEEK_ENDS upd = db.WEEK_ENDS.Where(w => w.WE_BE == BE && w.WE_BU == BU && w.WE_CAL_NO == Data.hdSingle.WDCH_DOC_NO && w.WE_DAY == day.WE_DAY).SingleOrDefault();
                        if (day.WE_DAY == "SU")
                        {
                            upd.WE_DAY_CK = Data.Sunday;
                        }
                        else if (day.WE_DAY == "MO")
                        {
                            upd.WE_DAY_CK = Data.Monday;
                        }
                        else if (day.WE_DAY == "TU")
                        {
                            upd.WE_DAY_CK = Data.Tuesday;
                        }
                        else if (day.WE_DAY == "WE")
                        {
                            upd.WE_DAY_CK = Data.Wednesday;
                        }
                        else if (day.WE_DAY == "TH")
                        {
                            upd.WE_DAY_CK = Data.Thursday;
                        }
                        else if (day.WE_DAY == "FR")
                        {
                            upd.WE_DAY_CK = Data.Friday;
                        }
                        else if (day.WE_DAY == "SA")
                        {
                            upd.WE_DAY_CK = Data.Saturday;
                        }
                        upd.WE_UPD_BY = User;
                        upd.WE_UPD_DATE = DateTime.Now;
                        db.SaveChanges();
                    }
                    this.GenerateCalendar(Data.hdSingle.WDCH_DOC_NO);
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - UpdateWeekOff : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Activate Calendar
        public string ActivateCalendar(int docNo)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                using (HRMSEntities db = new HRMSEntities())
                {
                    WORK_DAY_CALENDAR_HD hdData = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_BE == BE && w.WDCH_BU == BU && w.WDCH_DOC_NO == docNo).SingleOrDefault();
                    if (hdData != null)
                    {
                        hdData.WDCH_STATUS = "A";
                        hdData.WDCH_UPD_BY = User;
                        hdData.WDCH_UPD_DATE = DateTime.Now;
                        db.SaveChanges();
                    }
                    message = "Activate";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - ActivateCalendar : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region DeActivate Calendar
        public string DeActivateCalendar(int docNo)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                using (HRMSEntities db = new HRMSEntities())
                {
                    WORK_DAY_CALENDAR_HD hdData = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_BE == BE && w.WDCH_BU == BU && w.WDCH_DOC_NO == docNo).SingleOrDefault();
                    if (hdData != null)
                    {
                        hdData.WDCH_STATUS = "N";
                        hdData.WDCH_UPD_BY = User;
                        hdData.WDCH_UPD_DATE = DateTime.Now;
                        db.SaveChanges();
                    }
                    message = "DeActivate";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - DeActivateCalendar : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete Holiday
        public string DeleteHoliday(int docNo, int seqNo)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                using (HRMSEntities db = new HRMSEntities())
                {
                    WORK_DAY_CALENDAR_HOLIDAYS remove = db.WORK_DAY_CALENDAR_HOLIDAYS.Where(w => w.WDCHO_BE == BE && w.WDCHO_BU == BU && w.WDCHO_DOC_NO == docNo && w.WDCHO_SEQ_NO == seqNo).SingleOrDefault();
                    db.WORK_DAY_CALENDAR_HOLIDAYS.Remove(remove);
                    db.SaveChanges();
                    this.GenerateCalendar(docNo);
                    message = "Delete";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - DeleteHoliday : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Exists
        public bool CheckExists(Int16 unit, int year)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                using (HRMSEntities db = new HRMSEntities())
                {
                    result = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_BE == BE && w.WDCH_BU == unit && w.WDCH_YEAR == year).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - CheckExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Holiday Exists
        public bool CheckHolidayExists(int docNo, DateTime date)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                using (HRMSEntities db = new HRMSEntities())
                {
                    result = db.WORK_DAY_CALENDAR_HOLIDAYS.Where(w => w.WDCHO_BE == BE && w.WDCHO_BU == BU && w.WDCHO_DOC_NO == docNo && w.WDCHO_HOLIDAY == date).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - CheckHolidayExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Active Calendar
        public bool CheckActiveCalendar()
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                using (HRMSEntities db = new HRMSEntities())
                {
                    result = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_BE == BE && w.WDCH_BU == BU && w.WDCH_STATUS == "A").Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoWorkDayCalendar - CheckActiveCalendar : " + ex.Message);
            }
            return result;
        }
        #endregion
    }
}
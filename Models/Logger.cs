using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Logger));

        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Error(string format, params object[] args)
        {
            log.Error(string.Format(format, args));
        }

        public void Info(string format, params object[] args)
        {
            log.Info(string.Format(format, args));
        }

        public void Debug(string format, params object[] args)
        {
            log.Debug(string.Format(format, args));
        }
    }
}
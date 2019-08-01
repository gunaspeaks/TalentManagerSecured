using log4net;
using System;

namespace Agilisium.TalentManager.Tools.WindowsServiceExecutor
{
    public static class MyLogger
    {
        private static readonly ILog logger;

        static MyLogger()
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void LogMessage(string message, Exception exception = null)
        {
            if (string.IsNullOrWhiteSpace(message) == false)
            {
                logger.Info(message);
            }
            else
            {
                logger.Info("");
            }

            if (exception != null)
            {
                logger.Error(exception.Message, exception);
            }
        }
    }
}

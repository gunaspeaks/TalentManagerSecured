using Agilisium.TalentManager.ServiceProcessors;
using log4net;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace Agilisium.TalentManager.WindowsServices.ManagementNotifications
{
    internal partial class ManagementNotificationsService : ServiceBase
    {
        private Timer serviceTimer;
        private readonly ILog logger;
        private int dayOfExecution = 22;
        private string appTempDirectory = @"D:\OfficeApps\Temp";

        public ManagementNotificationsService()
        {
            InitializeComponent();
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
        }

        private void InitializeTimer()
        {
            serviceTimer = new Timer();
            int defaultScheduledMin = 22 * 60 * 60 * 1000;

            try
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["serviceTriggerInterval"]))
                {
                    defaultScheduledMin = Convert.ToInt32(ConfigurationManager.AppSettings["serviceTriggerInterval"]) * 60 * 60 * 1000;
                }

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["dayOfExecution"]))
                {
                    dayOfExecution = Convert.ToInt32(ConfigurationManager.AppSettings["dayOfExecution"]);
                }

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["appTempDirectory"]))
                {
                    appTempDirectory = ConfigurationManager.AppSettings["appTempDirectory"];
                }

                serviceTimer.Elapsed += new ElapsedEventHandler(ServiceTimer_Elapsed);
                serviceTimer.Interval = defaultScheduledMin;
            }
            catch (Exception exp)
            {
                logger.Error("Error while reading configuration");
                logger.Error(exp);
            }

            serviceTimer.Start();
            logger.Info($"Service will be triggered for every {defaultScheduledMin} Milli Seconds");
        }

        private void ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info("*********************************************************************************************");
            logger.Info("Service execution triggered");

            if (ProcessorHelper.IsExecutionCompleted(ServiceProcessors.WindowsServices.ManagementNotifications))
            {
                logger.Info("Service execution completed already. It will not be processed again.");
                return;
            }

            try
            {
                if (dayOfExecution != DateTime.Now.Day)
                {
                    logger.Info($"Service will not be processed today as today is not {dayOfExecution} as configured");
                    return;
                }

                int reportingDay = 27;
                if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                {
                    reportingDay += 1;
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                {
                    reportingDay += 2;
                }

                ManagementNotificationsProcessor processor = new ManagementNotificationsProcessor();
                processor.GenerateManagementNotifications(appTempDirectory, reportingDay);
                
            }
            catch (Exception exp)
            {
                logger.Error("Error while executing the service");
                logger.Error(exp);
            }
            finally
            {
                logger.Info("Service execution completed");
                logger.Info("*********************************************************************************************");
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //ExecuteServiceLocally();

                logger.Info("Service has been started");
                InitializeTimer();
            }
            catch (Exception exp)
            {
                logger.Error("Error while starting the service");
                logger.Error(exp);
            }
        }

        protected override void OnStop()
        {
            logger.Info("Service has been stopped");
            logger.Info("");
            serviceTimer.Enabled = false;
            serviceTimer.Stop();
            serviceTimer.Dispose();
        }

        private void ExecuteServiceLocally()
        {
            ManagementNotificationsProcessor processor = new ManagementNotificationsProcessor();
            processor.GenerateManagementNotifications(appTempDirectory, 27);
        }
    }
}

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
                defaultScheduledMin = Convert.ToInt32(ConfigurationManager.AppSettings["serviceTriggerInterval"]) * 60 * 60 * 1000;
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
            try
            {
                //AllocationsUpdaterServiceProcessor processor = new AllocationsUpdaterServiceProcessor();
                logger.Info("Processing allocations");
                //int newEntries = processor.ProcessAllocations();
                //logger.Info($"There are {newEntries} entries had been processed");
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
                // ExecuteServiceLocally();
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
        }
    }
}

using Agilisium.TalentManager.ServiceProcessors;
using log4net;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace Agilisium.TalentManager.WindowsServices
{
    public partial class AllocationsUpdaterService : ServiceBase
    {
        private Timer serviceTimer;
        private readonly ILog logger;

        public AllocationsUpdaterService()
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

        private void ExecuteServiceLocally()
        {
            AllocationsUpdaterServiceProcessor processor = new AllocationsUpdaterServiceProcessor();
            logger.Info("Identifying new allocation entries for notification");
            int newEntries = processor.ProcessAllocations();
            logger.Info($"There are {newEntries} entries added for notification");
            processor.ProcessExpiredAllocations();
        }

        protected override void OnStop()
        {
            logger.Info("Service has been stopped");
            serviceTimer.Stop();
            serviceTimer.Dispose();
        }

        private void ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info("");
            logger.Info("*********************************************************************************************");
            logger.Info("Service execution triggered");
            try
            {
                AllocationsUpdaterServiceProcessor processor = new AllocationsUpdaterServiceProcessor();
                logger.Info("Processing allocations");
                int newEntries = processor.ProcessAllocations();
                logger.Info($"There are {newEntries} entries had been processed");
                processor.ProcessExpiredAllocations();
            }
            catch (Exception exp)
            {
                logger.Error("Error while executing the service");
                logger.Error(exp);
            }
            finally
            {
                logger.Info("Execution completed");
                logger.Info("*********************************************************************************************");
            }
        }
    }
}

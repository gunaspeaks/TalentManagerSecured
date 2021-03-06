﻿using Agilisium.TalentManager.ServiceProcessors;
using log4net;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace Agilisium.TalentManager.WindowsServices
{
    public partial class AllocationsMessengerService : ServiceBase
    {
        private Timer serviceTimer;
        private readonly ILog logger;
        private int dayOfWeek = 1;
        private string appTempDirectory = @"D:\OfficeApps\Temp";

        public AllocationsMessengerService()
        {
            InitializeComponent();
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
        }

        private void InitializeService()
        {
            serviceTimer = new Timer();
            int defaultScheduledMin = 12 * 60 * 60 * 1000;

            try
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["serviceTriggerInterval"]))
                {
                    defaultScheduledMin = Convert.ToInt32(ConfigurationManager.AppSettings["serviceTriggerInterval"]) * 60 * 60 * 1000;
                }

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["serviceExecutionDayOfWeek"]))
                {
                    dayOfWeek = Convert.ToInt32(ConfigurationManager.AppSettings["serviceExecutionDayOfWeek"]);
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

        protected override void OnStart(string[] args)
        {
            try
            {
                // ExecuteServiceActions();

                logger.Info("Service has been started");
                InitializeService();
            }
            catch (Exception exp)
            {
                logger.Error("Error while initializing the service");
                logger.Error(exp);
            }
        }

        private void ExecuteServiceActions()
        {
            logger.Info("");
            logger.Info("*********************************************************************************************");
            logger.Info($"Service execution triggered on {DateTime.Today.DayOfWeek}");

            if (ProcessorHelper.IsExecutionCompleted(ServiceProcessors.WindowsServices.WeeklyAllocationsMailer))
            {
                logger.Info("Service execution completed already. It will not be processed again.");
                return;
            }

            try
            {
                if (dayOfWeek != (int)DateTime.Today.DayOfWeek)
                {
                    logger.Info($"Skipping service execution as it is {DateTime.Today.DayOfWeek.ToString()}.");
                    return;
                }
                logger.Info("Initiating processor");
                AllocationsMessengerServiceProcessor processor = new AllocationsMessengerServiceProcessor();
                processor.GenerateResourceAllocationReport(appTempDirectory);
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

        private void ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteServiceActions();
        }

        protected override void OnStop()
        {
            logger.Info("Service has been stopped");
            serviceTimer.Stop();
            serviceTimer.Dispose();
        }
    }
}

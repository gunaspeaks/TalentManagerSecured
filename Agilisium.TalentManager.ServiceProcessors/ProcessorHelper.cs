using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agilisium.TalentManager.ServiceProcessors
{
    public static class ProcessorHelper
    {
        public const string EMAIL_PROXY_SERVER = "Email Proxy Server";
        public const string EMAIL_PROXY_PORT = "Email Proxy Port";
        public const string CONTRACTOR_REQ_EMAIL_OWNER = "Contractor Request Email Owner";
        public const string CONTRACTOR_REQ_BCC_RECEIPIENTS = "Contractor Request Email BCC Email IDs";
        public const string EMAIL_OWNERS_PASSWORD = "Owner's Outlook EMAIL Password";
        public const string TEMPLATE_FOLDER_PATH = "Email Templates Folder Path";
        public const string MANAGERS_EMAIL_GROUP = "Agilisium Managers Email Group";
        public const string MANAGERS_EMAIL_GROUP_BCC = "Agilisium Managers BCC Email Group";

        private static SystemSettingRepository settingsRepo;
        private static List<Dto.SystemSettingDto> systemSettings;

        static ProcessorHelper()
        {
            settingsRepo = new SystemSettingRepository();
            systemSettings = settingsRepo.GetAll().ToList();
        }

        public static string GetSettingsValue(string settingsName)
        {
            return systemSettings.FirstOrDefault(e => e.SettingName == settingsName)?.SettingValue;
        }

        public static bool IsExecutionCompleted(WindowsServices serviceName)
        {
            WindowsServiceSettingsDto serviceSettings = settingsRepo.GetServiceSettings((int)serviceName);

            if (serviceSettings.ExecutionInterval == "Daily")
            {
                DateTime executedDate = serviceSettings.ExecutedDate.Value;
                return executedDate.Day == DateTime.Today.Day && executedDate.Month == DateTime.Today.Month && executedDate.Year == DateTime.Today.Year;
            }

            if (serviceSettings.ExecutionInterval == "Weekly")
            {
                DateTime startDate = DateTime.Today, endDate = DateTime.Today;
                switch (DateTime.Today.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        startDate = DateTime.Today;
                        endDate = DateTime.Today.AddDays(6);
                        break;
                    case DayOfWeek.Monday:
                        startDate = DateTime.Today.AddDays(-1);
                        endDate = DateTime.Today.AddDays(5);
                        break;
                    case DayOfWeek.Tuesday:
                        startDate = DateTime.Today.AddDays(-2);
                        endDate = DateTime.Today.AddDays(4);
                        break;
                    case DayOfWeek.Wednesday:
                        startDate = DateTime.Today.AddDays(-3);
                        endDate = DateTime.Today.AddDays(3);
                        break;
                    case DayOfWeek.Thursday:
                        startDate = DateTime.Today.AddDays(-4);
                        endDate = DateTime.Today.AddDays(2);
                        break;
                    case DayOfWeek.Friday:
                        startDate = DateTime.Today.AddDays(-5);
                        endDate = DateTime.Today.AddDays(1);
                        break;
                    case DayOfWeek.Saturday:
                        DateTime.Today.AddDays(-6);
                        endDate = DateTime.Today;
                        break;
                }

                return serviceSettings.ExecutedDate.Value >= startDate && serviceSettings.ExecutedDate.Value <= endDate;
            }

            if (serviceSettings.ExecutionInterval == "Monthly")
            {
                DateTime startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 28);
                return serviceSettings.ExecutedDate.Value >= startDate && serviceSettings.ExecutedDate.Value <= endDate;
            }

            return false;
        }
    }

    public enum WindowsServices
    {
        WeeklyAllocationsMailer = 1,
        ManagementNotifications,
        DailyAllocationsUpdater
    }

}

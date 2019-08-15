using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Repository.Repositories;
using Agilisium.TalentManager.ServerUtilities;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Agilisium.TalentManager.ServiceProcessors
{
    public class ManagementNotificationsProcessor
    {
        private readonly AllocationRepository allocationService;
        private readonly EmployeeRepository empService;
        private readonly ProjectRepository projectRepository;
        private readonly PracticeRepository practiceRepository;
        private readonly string dmEmailID = "satish.srinivasan @agilisium.com";

        private readonly ILog logger;

        public ManagementNotificationsProcessor()
        {
            allocationService = new AllocationRepository();
            empService = new EmployeeRepository();
            projectRepository = new ProjectRepository();
            practiceRepository = new PracticeRepository();

            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
        }

        public void GenerateManagementNotifications(string appTempDirectory, int reportingDay)
        {
            logger.Info("Reading system settings");
            string emailClientIP = ProcessorHelper.GetSettingsValue(ProcessorHelper.EMAIL_PROXY_SERVER);
            string ownerEmailID = ProcessorHelper.GetSettingsValue(ProcessorHelper.CONTRACTOR_REQ_EMAIL_OWNER);
            string templateFilePath = ProcessorHelper.GetSettingsValue(ProcessorHelper.TEMPLATE_FOLDER_PATH) + "\\PODWiseEmployees.html";
            string toEmailID = null;
            string outlookPwd = ProcessorHelper.GetSettingsValue(ProcessorHelper.EMAIL_OWNERS_PASSWORD);
            string emailSubject = "RMT Alert - Please Confirm, Employees Under your POD";

            EmailHandler emailHandler = new EmailHandler(ownerEmailID, outlookPwd);

            List<PracticeDto> pods = practiceRepository.GetAll().ToList();
            logger.Info($"There are {pods.Count} PODs");
            foreach (PracticeDto pod in pods)
            {
                string attachmentFilePath = CreateFileAttachment(appTempDirectory, pod.PracticeID);
                string emailContent = GenerateEmailBody(templateFilePath, pod.PracticeID, pod.PracticeName, pod.ManagerName, reportingDay);
                string managerEmailID = null;
                if (pod.ManagerID.HasValue)
                {
                    managerEmailID = empService.GetByID(pod.ManagerID.Value)?.EmailID;
                }

                if (string.IsNullOrWhiteSpace(managerEmailID))
                {
                    managerEmailID = dmEmailID;
                }
                toEmailID = managerEmailID;

                logger.Info("Sending email with attachment to " + pod.ManagerName);
                emailHandler.SendEmail(emailClientIP, toEmailID, emailSubject, emailContent, dmEmailID, attachmentFilePath, System.Net.Mail.MailPriority.High);
            }
        }

        private string GenerateEmailBody(string templateFilePath, int podID, string podName, string managerName, int reportingDay)
        {
            logger.Info("Generating email body");

            string emailTemplateContent = FilesHandler.GetFileContent(templateFilePath);
            StringBuilder emailBody = new StringBuilder(emailTemplateContent);
            emailBody.Replace("__POD__", podName);
            emailBody.Replace("__MANAGER_NAME__", managerName);
            emailBody.Replace("__DAY__", reportingDay.ToString());
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            string monthName = ci.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            emailBody.Replace("__MONTH_NAME__", monthName);
            return emailBody.ToString();
        }

        private string CreateFileAttachment(string appTempDirectory, int podID)
        {
            logger.Info("Generate CSV file for the attachment");
            string filePath = "";
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,POD,Project Name{Environment.NewLine}");
            try
            {
                logger.Info($"Retrieve employess for the POD ID {podID}");
                var emps = empService.GetAllByPractice(podID);
                string podName = "";
                foreach (var emp in emps)
                {
                    recordString.Append($"{emp.EmployeeID},");
                    recordString.Append($"{emp.FirstName} {emp.LastName},");
                    podName = emp.PracticeName;
                    recordString.Append($"{podName},");
                    List<CustomAllocationDto> allocations = allocationService.GetAllocatedProjectsByEmployeeID(emp.EmployeeEntryID).ToList();
                    string projectName = "";
                    foreach (CustomAllocationDto prj in allocations)
                    {
                        projectName += projectName + prj.ProjectName;
                    }
                    recordString.Append($"{projectName}{Environment.NewLine}");
                }

                logger.Info("create the file");
                string fileName = $"EmployeesMappedUnder-{podName}-AsOn-{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.csv";
                filePath = FilesHandler.CreateFile(appTempDirectory, fileName, recordString.ToString());
            }
            catch (Exception exp)
            {
                logger.Error("Error while generating CSV file", exp);
            }

            return filePath;
        }
    }
}

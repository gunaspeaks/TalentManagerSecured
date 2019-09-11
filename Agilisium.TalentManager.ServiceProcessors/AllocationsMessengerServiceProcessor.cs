using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Repository.Repositories;
using Agilisium.TalentManager.ServerUtilities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilisium.TalentManager.ServiceProcessors
{
    public class AllocationsMessengerServiceProcessor
    {
        private readonly AllocationRepository allocationService;
        private readonly EmployeeRepository empService;
        private readonly SystemSettingRepository settingRepository;
        private readonly ILog logger;

        public AllocationsMessengerServiceProcessor()
        {

            allocationService = new AllocationRepository();
            empService = new EmployeeRepository();
            settingRepository = new SystemSettingRepository();
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
        }

        public void GenerateResourceAllocationReport(string appTempDirectory)
        {
            logger.Info("Reading system settings");
            string emailClientIP = ProcessorHelper.GetSettingsValue(ProcessorHelper.EMAIL_PROXY_SERVER);
            string ownerEmailID = ProcessorHelper.GetSettingsValue(ProcessorHelper.CONTRACTOR_REQ_EMAIL_OWNER);
            string templateFilePath = ProcessorHelper.GetSettingsValue(ProcessorHelper.TEMPLATE_FOLDER_PATH) + "\\ResourceAllocationReportTemplate.html";
            string toEmailID = ProcessorHelper.GetSettingsValue(ProcessorHelper.MANAGERS_EMAIL_GROUP);
            string outlookPwd = ProcessorHelper.GetSettingsValue(ProcessorHelper.EMAIL_OWNERS_PASSWORD);
            string emailSubject = "Agilisium - Resource Allocation Report";
            string bccEmailIDs = ProcessorHelper.GetSettingsValue(ProcessorHelper.CONTRACTOR_REQ_BCC_RECEIPIENTS);
            EmailHandler emailHandler = new EmailHandler(ownerEmailID, outlookPwd);

            string emailContent = GenerateEmailBody(templateFilePath);
            string attachmentFilePath = GenerateAllocationReportAsCsvFile(appTempDirectory);
            logger.Info("Sending email with attachment");
            emailHandler.SendEmail(emailClientIP, toEmailID, emailSubject, emailContent, bccEmailIDs, attachmentFilePath);

            WindowsServiceSettingsDto windowsService = new WindowsServiceSettingsDto
            {
                ExecutionInterval = "Weekly",
                ServiceID = (int)WindowsServices.WeeklyAllocationsMailer,
                ServiceName = WindowsServices.WeeklyAllocationsMailer.ToString(),
            };
            settingRepository.UpdateWindowsServiceStatus(windowsService);
        }

        private string GenerateEmailBody(string templateFilePath)
        {
            logger.Info("Generating email body");
            List<BillabilityWiseAllocationSummaryDto> allocationSummary = allocationService.GetBillabilityWiseAllocationSummary().ToList();
            ResourceCountDto dto = empService.GetEmployeesCountSummary();
            string emailTemplateContent = FilesHandler.GetFileContent(templateFilePath);
            StringBuilder emailBody = new StringBuilder(emailTemplateContent);
            emailBody.Replace("__TODAY__", DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day);
            emailBody.Replace("__TOTAL_COUNT__", dto.TotalCount.ToString());
            emailBody.Replace("__DELIVERY_COUNT__", dto.DeliveryCount.ToString());
            emailBody.Replace("__BD_COUNT__", dto.BdCount.ToString());
            emailBody.Replace("__BO_COUNT__", dto.BoCount.ToString());
            emailBody.Replace("__BILLABLE__", allocationSummary.FirstOrDefault(e => e.AllocationType == "Billable")?.NumberOfEmployees.ToString());
            emailBody.Replace("__COMMITED_BUFFER__", allocationSummary.FirstOrDefault(e => e.AllocationType == "Committed Buffer")?.NumberOfEmployees.ToString());
            emailBody.Replace("__NON_COMMITED_BUFFER__", allocationSummary.FirstOrDefault(e => e.AllocationType == "Non-Committed Buffer")?.NumberOfEmployees.ToString());
            emailBody.Replace("__NOT_ALLOCATED_YET_DELIVERY__", allocationSummary.FirstOrDefault(e => e.AllocationType == "Not Allocated - Delivery")?.NumberOfEmployees.ToString());
            emailBody.Replace("__NOT_ALLOCATED_YET_OTHERS__", allocationSummary.FirstOrDefault(e => e.AllocationType == "BD & BO")?.NumberOfEmployees.ToString());
            emailBody.Replace("__BENCH_AVAILABLE__", allocationSummary.FirstOrDefault(e => e.AllocationType == "Bench (Available)")?.NumberOfEmployees.ToString());
            emailBody.Replace("__BENCH_EARMARKED__", allocationSummary.FirstOrDefault(e => e.AllocationType == "Bench (Earmarked)")?.NumberOfEmployees.ToString());
            return emailBody.ToString();
        }

        private string GenerateAllocationReportAsCsvFile(string appTempDirectory)
        {
            logger.Info("Generate CSV file for the attachment");
            string filePath = "";
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Primary Skills,Secondary Skills,Business Unit,POD,Project Name,Account Name,Allocation Type,Allocation Start Date,Allocation End Date,Project Manager,Comments{Environment.NewLine}");
            try
            {
                List<BillabilityWiseAllocationDetailDto> detailsDtos = allocationService.GetBillabilityWiseAllocationDetail("all", "all").ToList();
                foreach (BillabilityWiseAllocationDetailDto dto in detailsDtos)
                {
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.PrimarySkills?.Replace(",", "")},");
                    recordString.Append($"{dto.SecondarySkills?.Replace(",", "")},");
                    recordString.Append($"{dto.BusinessUnit},");
                    recordString.Append($"{dto.POD},");
                    recordString.Append($"{dto.ProjectName},");
                    recordString.Append($"{dto.AccountName},");
                    recordString.Append($"{dto.AllocationType},");
                    recordString.Append($"{dto.AllocationStartDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.AllocationEndDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.ProjectManager},");
                    recordString.Append($"{dto.Comments}{Environment.NewLine}");
                }

                
                string fileName = $"ResourceAllocationReport-AsOn-{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.csv";
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

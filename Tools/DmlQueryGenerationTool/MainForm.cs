using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agilisium.TalentManager.Tools.DmlQueryGenerationTool
{
    public partial class MainForm : Form
    {
        private const string NULL_STRING = "NULL";
        private const string TRUE_STRING = "True";
        private const string FALSE_STRING = "False";

        private Agilisium.TalentManager.PostgresModel.TalentManagerDataContext postgressDataContext = new Agilisium.TalentManager.PostgresModel.TalentManagerDataContext();

        public MainForm()
        {
            InitializeComponent();
        }

        private async void MigrateDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (clbTablesList.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Please select one or more table to generate the SQL scripts");
                    clbTablesList.Focus();
                    return;
                }

                foreach (var item in clbTablesList.CheckedItems)
                {
                    switch (item.ToString())
                    {
                        case "BuLevel":
                            UpdateApplicationStatus("Processing BuLevel Table ...");
                            await ProcessBuLevelTable();
                            break;
                        case "Certification":
                            UpdateApplicationStatus("Processing Certification Table ...");
                            await ProcessCertificationTable();
                            break;
                        case "DropDownCategory":
                            UpdateApplicationStatus("Processing DropDownCategoty Table ...");
                            await ProcessDropDownCategoryTable();
                            break;
                        case "DropDownSubCategory":
                            UpdateApplicationStatus("Processing DropDownSubCategory Table ...");
                            await ProcessDropDownSubCategoryTable();
                            break;
                        case "EmpAssetDetail":
                            UpdateApplicationStatus("Processing EmpAssetDetail Table ...");
                            await ProcessEmpAssetDetailTable();
                            break;
                        case "EmpCertification":
                            UpdateApplicationStatus("Processing EmpCertification Table ...");
                            await ProcessEmpCertificationTable();
                            break;
                        case "Employee":
                            UpdateApplicationStatus("Processing Employee Table ...");
                            await ProcessEmployeeTable();
                            break;
                        case "EmployeeSkill":
                            UpdateApplicationStatus("Processing EmployeeSkill Table ...");
                            await ProcessEmployeeSkillTable();
                            break;
                        case "Project":
                            UpdateApplicationStatus("Processing Project Table ...");
                            await ProcessProjectTable();
                            break;
                        case "ProjectAccount":
                            UpdateApplicationStatus("Processing ProjectAccount Table ...");
                            await ProcessAccountTable();
                            break;
                        case "ProjectAllocation":
                            UpdateApplicationStatus("Processing ProjectAllocation Table ...");
                            await ProcessProjectAllocationTable();
                            break;
                        case "ResourceLevel":
                            UpdateApplicationStatus("Processing ResourceLevel Table ...");
                            await ProcessResourceLevelTable();
                            break;
                        case "TechSkill":
                            UpdateApplicationStatus("Processing TechSkill Table ...");
                            await ProcessTechSkillTable();
                            break;
                        case "TechSkillCategory":
                            UpdateApplicationStatus("Processing TechSkillCategory Table ...");
                            await ProcessTechSkillCategoryTable();
                            break;
                    }
                }
                UpdateApplicationStatus();

            }
            catch (Exception exp)
            {
                UpdateApplicationStatus();
                MessageBox.Show(exp.Message);
            }
            finally
            {
                UpdateApplicationStatus();
            }
        }

        private async Task<int> ProcessDropDownCategoryTable()
        {
            List<DropDownCategory> records = postgressDataContext.DropDownCategories.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"DropDownCategory\" (\"CategoryName\", \"ShortName\",\"Description\",\"IsReserved\",\"IsDeleted\",\"CategoryID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (DropDownCategory rec in records)
                {
                    dmlQry.Append($"{baseQry}'{rec.CategoryName}','{rec.ShortName}','{rec.Description}','{rec.IsReserved}','{rec.IsDeleted}',{rec.CategoryID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\DropDownCategory.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing Categories table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessDropDownSubCategoryTable()
        {
            List<DropDownSubCategory> records = postgressDataContext.DropDownSubCategories.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"DropDownSubCategory\" (\"SubCategoryName\", \"ShortName\",\"Description\",\"IsReserved\",\"IsDeleted\",\"CategoryID\",\"SubCategoryID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (DropDownSubCategory rec in records)
                {
                    dmlQry.Append($"{baseQry}'{rec.SubCategoryName}','{rec.ShortName}','{rec.Description}','{rec.IsReserved}','{rec.IsDeleted}',{rec.CategoryID},{rec.SubCategoryID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\DropDownSubCategory.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing Sub Categories table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessBuLevelTable()
        {
            List<BuLevel> records = postgressDataContext.BuLevels.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"BuLevel\" (\"ItemEntryID\", \"BusinessUnitID\",\"ItemName\",\"IsDeleted\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (BuLevel rec in records)
                {
                    dmlQry.Append($"{baseQry}{rec.ItemEntryID},{rec.BusinessUnitID},'{rec.ItemName}','{rec.IsDeleted}');{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\BuLevel.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing BuLevel Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessEmpAssetDetailTable()
        {
            List<EmpAssetDetail> records = postgressDataContext.EmpAssetDetails.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"EmpAssetDetail\" (\"LocationID\", \"PrimarySkills\",\"SecondarySkills\",\"Designation\",\"VisaStatusID\",\"OverallExperience\",\"EmployeeID\",\"IsDeleted\",\"EmployeeEntryID\",\"EmpAssetDetailID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (EmpAssetDetail rec in records)
                {
                    dmlQry.Append($"{baseQry}{rec.LocationID},'{rec.PrimarySkills}','{rec.SecondarySkills}','{rec.Designation}',{rec.VisaStatusID},'{rec.OverallExperience}','{rec.EmployeeID}','{rec.IsDeleted}',{rec.EmployeeEntryID},{rec.EmpAssetDetailID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\EmpAssetDetail.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing EmpAssetDetails table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessEmployeeTable()
        {
            List<Employee> records = postgressDataContext.Employees.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"Employee\" (\"EmployeeID\", \"FirstName\",\"LastName\",\"EmailID\",\"BusinessUnitID\",\"DateOfJoin\",\"LastWorkingDay\",\"PrimarySkills\",\"SecondarySkills\",\"ReportingManagerID\",\"UtilizationTypeID\",\"EmploymentTypeID\",\"IsDeleted\",\"VisaCategoryID\",\"VisaValidUpto\",\"PassportNo\",\"PassportValidUpto\",\"StrengthAreaID\",\"OverallExperience\",\"TechnicalRank\",\"EmployeeEntryID\",\"IsTechResource\",\"Level1ID\",\"Level2ID\",\"Level3ID\",\"Level4ID\",\"Level5ID\",\"IsManager\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (Employee rec in records)
                {
                    dmlQry.Append($"{baseQry}'{rec.EmployeeID}','{rec.FirstName.Trim()}','{rec.LastName.Trim()}','{rec.EmailID}',{rec.BusinessUnitID},{GetStringForDate(rec.DateOfJoin)},{GetStringForNullableDate(rec.LastWorkingDay)},'{rec.PrimarySkills}','{rec.SecondarySkills}',{(rec.ReportingManagerID.HasValue ? rec.ReportingManagerID.ToString() : NULL_STRING)},{(rec.UtilizationTypeID.HasValue ? rec.UtilizationTypeID.ToString() : NULL_STRING)},{rec.EmploymentTypeID},'False',{(rec.VisaCategoryID.HasValue ? rec.VisaCategoryID.ToString() : NULL_STRING)},{GetStringForNullableDate(rec.VisaValidUpto)},'{rec.PassportNo}',{GetStringForNullableDate(rec.PassportValidUpto)},{(rec.StrengthAreaID.HasValue ? rec.StrengthAreaID.ToString() : NULL_STRING)},'{rec.OverallExperience}',{(rec.TechnicalRank.HasValue ? rec.TechnicalRank.ToString() : NULL_STRING)},{rec.EmployeeEntryID},'{(rec.IsTechResource.HasValue ? rec.IsTechResource.ToString() : FALSE_STRING)}',{(rec.Level1ID.HasValue ? rec.Level1ID.ToString() : NULL_STRING)},{(rec.Level2ID.HasValue ? rec.Level2ID.ToString() : NULL_STRING)},{(rec.Level3ID.HasValue ? rec.Level3ID.ToString() : NULL_STRING)},{(rec.Level4ID.HasValue ? rec.Level4ID.ToString() : NULL_STRING)},{(rec.Level5ID.HasValue ? rec.Level5ID.ToString() : NULL_STRING)},'{(rec.IsManager.HasValue ? rec.IsManager.ToString() : FALSE_STRING)}');{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\Employee.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing Employee table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessEmployeeSkillTable()
        {
            List<EmployeeSkill> records = postgressDataContext.EmployeeSkills.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"EmployeeSkill\" (\"EmployeeEntryID\", \"TechSkillID\",\"SkillCategoryID\",\"RatingID\",\"IsDeleted\",\"EmployeeSkillID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (EmployeeSkill rec in records)
                {
                    dmlQry.Append($"{baseQry}{rec.EmployeeEntryID},{rec.TechSkillID},{rec.SkillCategoryID},{rec.RatingID},'{rec.IsDeleted}',{rec.EmployeeSkillID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\EmployeeSkill.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing EmployeeSkill Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessProjectTable()
        {
            List<Project> records = postgressDataContext.Projects.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"Project\" (\"ProjectName\", \"ProjectCode\",\"DeliveryManagerID\",\"ProjectManagerID\",\"ProjectTypeID\",\"StartDate\",\"EndDate\",\"Remarks\",\"BusinessUnitID\",\"IsSowAvailable\",\"SowStartDate\",\"SowEndDate\",\"ProjectAccountID\",\"IsReserved\",\"IsDeleted\",\"ProjectID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (Project rec in records)
                {
                    dmlQry.Append($"{baseQry}'{rec.ProjectName}','{rec.ProjectCode}',{(rec.DeliveryManagerID.HasValue ? rec.DeliveryManagerID.ToString() : NULL_STRING)},{rec.ProjectManagerID},{rec.ProjectTypeID},{GetStringForDate(rec.StartDate)},{GetStringForDate(rec.EndDate)},'{rec.Remarks}',{rec.BusinessUnitID},'{rec.IsSowAvailable}',{GetStringForNullableDate(rec.SowStartDate)},{GetStringForNullableDate(rec.SowEndDate)},{rec.ProjectAccountID},'{rec.IsReserved}','{rec.IsDeleted}',{rec.ProjectID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\Project.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing Project Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessAccountTable()
        {
            List<ProjectAccount> records = postgressDataContext.ProjectAccounts.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"ProjectAccount\" (\"AccountName\", \"ShortName\",\"OnshoreManager\",\"OffshoreManagerID\",\"PartnerManager\",\"CountryID\",\"IsDeleted\",\"AccountID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (ProjectAccount rec in records)
                {
                    dmlQry.Append($"{baseQry}'{rec.AccountName}','{rec.ShortName}','{rec.OnshoreManager}',{rec.OffshoreManagerID},'{rec.PartnerManager}',{rec.CountryID},'{rec.IsDeleted}',{rec.AccountID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\ProjectAccount.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing ProjectAccount Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessProjectAllocationTable()
        {
            List<ProjectAllocation> records = postgressDataContext.ProjectAllocations.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"ProjectAllocation\" (\"EmployeeID\", \"AllocationStartDate\",\"AllocationEndDate\",\"AllocationTypeID\",\"ProjectID\",\"Remarks\",\"PercentageOfAllocation\",\"IsActive\",\"IsDeleted\",\"BenchCategoryID\",\"AllocationEntryID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (ProjectAllocation rec in records)
                {
                    dmlQry.Append($"{baseQry}{rec.EmployeeID},{GetStringForDate(rec.AllocationStartDate)},{GetStringForDate(rec.AllocationEndDate)},{rec.AllocationTypeID},{rec.ProjectID},'{(String.IsNullOrWhiteSpace(rec.Remarks) == false ? rec.Remarks.Trim().Replace("'", "-").Replace(Environment.NewLine, " ") : "")}',{rec.PercentageOfAllocation},'{rec.IsActive}','{rec.IsDeleted}',{(rec.BenchCategoryID.HasValue ? rec.BenchCategoryID.ToString() : NULL_STRING)},{rec.AllocationEntryID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\ProjectAllocation.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing ProjectAllocation Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessResourceLevelTable()
        {
            List<ResourceLevel> records = postgressDataContext.ResourceLevels.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"ResourceLevel\" (\"ItemEntryID\", \"ParentLevelID\",\"ItemName\",\"IsDeleted\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (ResourceLevel rec in records)
                {
                    dmlQry.Append($"{baseQry}{rec.ItemEntryID},{rec.ParentLevelID},'{rec.ItemName}','{rec.IsDeleted}');{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\ResourceLevel.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing ResourceLevel Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessTechSkillTable()
        {
            List<TechSkill> records = postgressDataContext.TechSkills.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"TechSkill\" (\"TechSkillName\", \"TechSkillCategoryID\",\"IsDeleted\",\"TechSkillID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (TechSkill rec in records)
                {
                    dmlQry.Append($"{baseQry}'{rec.TechSkillName}',{rec.TechSkillCategoryID},'{rec.IsDeleted}',{rec.TechSkillID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\TechSkill.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing TechSkill Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessTechSkillCategoryTable()
        {
            List<TechSkillCategory> records = postgressDataContext.TechSkillCategories.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"TechSkillCategory\" (\"CategoryName\", \"IsDeleted\",\"CategoryID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (TechSkillCategory rec in records)
                {
                    dmlQry.Append($"{baseQry}'{rec.CategoryName}','{rec.IsDeleted}',{rec.CategoryID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\TechSkillCategory.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing TechSkillCategory Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessCertificationTable()
        {
            List<Certification> records = postgressDataContext.Certifications.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"Certification\" (\"TechnologyAreaID\", \"Name\",\"IsDeleted\",\"ShortName\",\"CertificationID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (Certification rec in records)
                {
                    dmlQry.Append($"{baseQry}{rec.TechnologyAreaID},'{rec.Name}','{rec.IsDeleted}','{rec.ShortName}',{rec.CertificationID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\Certification.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing Certification Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private async Task<int> ProcessEmpCertificationTable()
        {
            List<EmpCertification> records = postgressDataContext.EmpCertifications.ToList();
            int recCount = 0;
            if (records.Count == 0) return 0;

            FileStream fileStream = null;
            StreamWriter ws = null;

            try
            {
                string baseQry = "INSERT INTO \"TalentManager\".\"EmpCertification\" (\"EmployeeID\", \"CertificationID\",\"ValidUpto\",\"CertifiedOn\",\"IsDeleted\",\"EntryID\") VALUES (";
                StringBuilder dmlQry = new StringBuilder();
                foreach (EmpCertification rec in records)
                {
                    dmlQry.Append($"{baseQry}{rec.EmployeeID},{rec.CertificationID},{GetStringForNullableDate(rec.ValidUpto)},{GetStringForNullableDate(rec.CertifiedOn)},'{rec.IsDeleted}',{rec.EntryID});{Environment.NewLine}");
                }

                fileStream = File.OpenWrite($"{txtPath.Text}\\EmpCertification.txt");
                ws = new StreamWriter(fileStream);
                await ws.WriteAsync(dmlQry.ToString());

                recCount = records.Count;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Error while processing EmpCertification Table. The error is {Environment.NewLine}Main Exception: {exp.Message}{Environment.NewLine}Inner Exception: {exp.InnerException?.Message}");
            }
            finally
            {
                if (ws != null)
                {
                    ws.Flush();
                    ws.Close();
                    ws.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return recCount;
        }

        private string GetStringForNullableDate(DateTime? dateValue)
        {
            if (dateValue.HasValue == false) return NULL_STRING;
            return $"'{dateValue.Value.Year}-{dateValue.Value.Month}-{dateValue.Value.Day}'";
        }

        private string GetStringForDate(DateTime dateValue)
        {
            if (dateValue == null) return NULL_STRING;
            return $"'{dateValue.Year}-{dateValue.Month}-{dateValue.Day}'";
        }

        private void UpdateApplicationStatus(string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                statusLabel.Text = "Ready";
                statusLabel.ForeColor = System.Drawing.Color.Black;
                Cursor = Cursors.Default;
                migrateDataButton.Enabled = true;
                statusLabel.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                statusLabel.ForeColor = System.Drawing.Color.Yellow;
                migrateDataButton.Enabled = false;
                statusLabel.BackColor = System.Drawing.Color.Red;
                statusLabel.Text = message;
            }
            Refresh();
            Invalidate();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                DialogResult res = dlg.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                txtPath.Text = dlg.SelectedPath;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

        }
    }
}

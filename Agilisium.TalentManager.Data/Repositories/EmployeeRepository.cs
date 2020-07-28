using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.PostgresDbHelper;
using Agilisium.TalentManager.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly PostgresSqlProcessor postgresSqlProcessor = null;

        public EmployeeRepository()
        {
            postgresSqlProcessor = new PostgresSqlProcessor();
        }

        #region Constants

        private const string YET_TO_JOIN_EMP_TYPE_NAME = "Yet to Join";
        private const string PERMANENT_EMP_TYPE_NAME = "Permanent";

        #endregion

        #region Public Methods - Employee Repository

        public bool Exists(string itemName)
        {
            return Entities.Any(e => e.FirstName.ToLower() == itemName.ToLower()
                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                && e.IsDeleted == false);
        }

        public bool Exists(int id)
        {
            return Entities.Any(e => e.EmployeeEntryID == id
            && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
            && e.IsDeleted == false);
        }

        public EmployeeDto GetByID(int id)
        {
            return (from emp in Entities
                    join bc in DataContext.DropDownSubCategories on emp.BusinessUnitID equals bc.SubCategoryID into bue
                    from bcd in bue.DefaultIfEmpty()
                    join ut in DataContext.DropDownSubCategories on emp.UtilizationTypeID equals ut.SubCategoryID into ute
                    from utd in ute.DefaultIfEmpty()
                    join et in DataContext.DropDownSubCategories on emp.EmploymentTypeID equals et.SubCategoryID into ete
                    from etd in ete.DefaultIfEmpty()
                    join vt in DataContext.DropDownSubCategories on emp.VisaCategoryID equals vt.SubCategoryID into vte
                    from vtd in vte.DefaultIfEmpty()
                    join pm in DataContext.Employees on emp.EmployeeEntryID equals pm.EmployeeEntryID into pme
                    from pmd in pme.DefaultIfEmpty()
                    where emp.EmployeeEntryID == id
                    select new EmployeeDto
                    {
                        BusinessUnitID = emp.BusinessUnitID,
                        DateOfJoin = emp.DateOfJoin,
                        EmployeeEntryID = emp.EmployeeEntryID,
                        EmailID = emp.EmailID,
                        EmployeeID = emp.EmployeeID,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        LastWorkingDay = emp.LastWorkingDay,
                        PrimarySkills = emp.PrimarySkills,
                        ProjectManagerID = pmd.EmployeeEntryID,
                        ProjectManagerName = pmd.LastName + ", " + pmd.FirstName,
                        ReportingManagerID = emp.ReportingManagerID,
                        SecondarySkills = emp.SecondarySkills,
                        UtilizationTypeID = emp.UtilizationTypeID,
                        EmploymentTypeID = emp.EmploymentTypeID,
                        EmploymentTypeName = etd.SubCategoryName,
                        PassportNo = emp.PassportNo,
                        PassportValidUpto = emp.PassportValidUpto,
                        TechnicalRank = emp.TechnicalRank,
                        PastExperience = emp.PastExperience,
                        VisaCategoryID = emp.VisaCategoryID,
                        VisaValidUpto = emp.VisaValidUpto,
                        VisaCategory = vtd.SubCategoryName,
                        Level1ID = emp.Level1ID,
                        Level2ID = emp.Level2ID,
                        Level3ID = emp.Level3ID,
                        Level4ID = emp.Level4ID,
                        Level5ID = emp.Level5ID,
                        IsManager = emp.IsManager,
                        StrengthAreaID = emp.StrengthAreaID,
                        IsTechResource = emp.IsTechResource,
                    }).FirstOrDefault();
        }

        public IEnumerable<EmployeeDto> GetAll(string searchText, int pageSize = -1, int pageNo = -1)
        {
            IEnumerable<EmployeeDto> employees = null;
            try
            {
                employees = GetAllActiveEmployees(searchText?.ToLower());

                if (pageSize <= 0 || pageNo < 1)
                {
                    return employees;
                }

            }
            catch (Exception)
            {

            }
            return employees.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<EmployeeDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            IEnumerable<EmployeeDto> employees = GetAllActiveEmployees("");

            if (pageSize <= 0 || pageNo < 1)
            {
                return employees;
            }

            return employees.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<EmployeeDto> GetAllPastEmployees(int pageSize = -1, int pageNo = -1)
        {
            List<EmployeeDto> employees = new List<EmployeeDto>();

            List<Employee> pastEmployees = (from emp in Entities
                                            where emp.IsDeleted == true
                                            || (emp.LastWorkingDay.HasValue == true && emp.LastWorkingDay.Value < DateTime.Today)
                                            select emp).ToList();

            foreach (Employee emp in pastEmployees)
            {
                employees.Add(new EmployeeDto
                {
                    BusinessUnitName = DataContext.DropDownSubCategories.FirstOrDefault(b => b.SubCategoryID == emp.BusinessUnitID)?.SubCategoryName,
                    DateOfJoin = emp.DateOfJoin,
                    EmployeeEntryID = emp.EmployeeEntryID,
                    EmployeeID = emp.EmployeeID,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    PrimarySkills = emp.PrimarySkills,
                    UtilizationTypeName = DataContext.DropDownSubCategories.FirstOrDefault(b => b.SubCategoryID == emp.UtilizationTypeID)?.SubCategoryName,
                    EmploymentTypeName = DataContext.DropDownSubCategories.FirstOrDefault(b => b.SubCategoryID == emp.EmploymentTypeID)?.SubCategoryName,
                    LastWorkingDay = emp.LastWorkingDay,
                    ReportingManagerName = Entities.FirstOrDefault(e => e.ReportingManagerID == emp.ReportingManagerID)?.FirstName,
                });
            }

            if (pageSize <= 0 || pageNo < 1)
            {
                return employees;
            }

            return employees.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public int GetPastEmployeesCount()
        {
            return Entities.Count(emp => emp.IsDeleted == true || (emp.LastWorkingDay.HasValue == true && emp.LastWorkingDay.Value < DateTime.Today));
        }

        public bool IsDuplicateName(string firstName, string lastName)
        {
            return Entities.Any(e =>
                e.FirstName.ToLower() == firstName.ToLower() &&
                e.LastName.ToLower() == lastName.ToLower() && e.IsDeleted == false);
        }

        public bool IsDuplicateName(int employeeEntryID, string firstName, string lastName)
        {
            return Entities.Any(e =>
                e.EmployeeEntryID != employeeEntryID &&
                e.FirstName.ToLower() == firstName.ToLower() &&
                e.LastName.ToLower() == lastName.ToLower() && e.IsDeleted == false);
        }

        public bool IsDuplicateEmployeeID(string employeeID)
        {
            return Entities.Any(e => e.EmployeeID.ToLower() == employeeID.ToLower() && e.IsDeleted == false);
        }

        public void Add(EmployeeDto entity)
        {
            Employee employee = CreateBusinessEntity(entity, true);
            Entities.Add(employee);
            DataContext.Entry(employee).State = EntityState.Added;
            DataContext.SaveChanges();

            UpdateEmployeeIDTracker(entity.EmploymentTypeID, entity.EmployeeID);
        }

        public void Update(EmployeeDto entity)
        {
            Employee oldEntity = Entities.FirstOrDefault(e => e.EmployeeEntryID == entity.EmployeeEntryID);
            if (!CanEmployeeBeUpdated(entity, oldEntity)) return;

            MigrateEntity(entity, oldEntity);
            oldEntity.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(oldEntity);
            DataContext.Entry(oldEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        private bool CanEmployeeBeUpdated(EmployeeDto updatedEmp, Employee oldEmp)
        {
            if (updatedEmp == null || oldEmp == null) return true;

            if (oldEmp.BusinessUnitID != updatedEmp.BusinessUnitID)
            {
                if (DataContext.ProjectAllocations.Any(a => a.IsDeleted == false && a.AllocationEndDate >= DateTime.Today
                 && a.EmployeeID == oldEmp.EmployeeEntryID))
                {
                    string oldBuName = DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryID == oldEmp.BusinessUnitID)?.SubCategoryName;
                    throw new InvalidOperationException($"You can't change the Business Unit as there is an active alloaction under {oldBuName} BU");
                }
            }

            if (oldEmp.BusinessUnitID == (int)BusinessUnit.Delivery)
            {
                if (oldEmp.Level1ID != updatedEmp.Level1ID)
                {
                    List<Project> projects = DataContext.Projects.Where(p => p.ProjectAccountID == oldEmp.Level1ID).ToList();
                    string oldAccName = DataContext.ProjectAccounts.FirstOrDefault(a => a.AccountID == oldEmp.Level1ID)?.AccountName;
                    foreach (Project prj in projects)
                    {
                        if (DataContext.ProjectAllocations.Any(a => a.IsDeleted == false && a.AllocationEndDate >= DateTime.Today
                         && a.ProjectID == prj.ProjectID))
                        {
                            throw new InvalidOperationException($"You can't change the Account when there is an active allocation with the previous account {oldAccName}");
                        }
                    }
                }

                if (oldEmp.Level2ID != updatedEmp.Level2ID)
                {
                    Project project = DataContext.Projects.Where(p => p.ProjectAccountID == oldEmp.Level2ID).FirstOrDefault();
                    if (DataContext.ProjectAllocations.Any(a => a.IsDeleted == false && a.AllocationEndDate >= DateTime.Today
                     && a.ProjectID == project.ProjectID))
                    {
                        throw new InvalidOperationException($"You can't change the Project when there is an active allocation with the previous Project {project.ProjectName}");
                    }
                }

            }

            return true;
        }

        public void Delete(EmployeeDto entity)
        {
            Employee buzEntity = Entities.FirstOrDefault(e => e.EmployeeEntryID == entity.EmployeeEntryID);
            buzEntity.IsDeleted = true;
            buzEntity.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public bool IsDuplicateEmployeeID(int employeeEntryID, string employeeID)
        {
            return Entities.Any(e => e.EmployeeID.ToLower() == employeeID.ToLower() &&
            e.EmployeeEntryID != employeeEntryID && e.IsDeleted == false);
        }

        public IEnumerable<EmployeeDto> GetAllManagers()
        {
            return (from emp in Entities
                    where emp.IsDeleted == false && emp.IsManager == true
                    && (emp.LastWorkingDay.HasValue == false || (emp.LastWorkingDay.HasValue == true && emp.LastWorkingDay.Value >= DateTime.Today))
                    orderby emp.EmployeeID
                    select new EmployeeDto
                    {
                        EmployeeEntryID = emp.EmployeeEntryID,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName
                    });
        }

        public IEnumerable<EmployeeDto> GetAllAccountManagers()
        {
            return GetAllManagers();
        }

        public string GenerateNewEmployeeID(int employeeTypeID)
        {
            string employeeID = string.Empty;

            EmployeeIDTracker tracker = DataContext.EmployeeIDTrackers.FirstOrDefault(e => e.EmploymentTypeID == employeeTypeID);
            string empType = DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryID == employeeTypeID)?.SubCategoryName;

            bool isDuplicate = true;

            int runningID = tracker.RunningID;
            int newRunningID = tracker.RunningID + 1;
            string idPrefix = tracker.IDPrefix;
            while (isDuplicate)
            {
                if (empType == PERMANENT_EMP_TYPE_NAME)
                {
                    employeeID = newRunningID.ToString();
                }
                else
                {
                    employeeID = $"{tracker.IDPrefix}{newRunningID.ToString().PadLeft(3, '0')}";
                }

                if (!Entities.Any(e => e.EmployeeID.ToLower() == employeeID.ToLower()))
                {
                    isDuplicate = false;
                }
                else
                {
                    newRunningID += 1;
                }
            }

            UpdateEmployeeIDTracker(employeeTypeID, runningID);

            return employeeID.ToUpper();
        }

        public int TotalRecordsCount(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return Entities.Count(e => e.IsDeleted == false
                && (e.LastWorkingDay.HasValue == false
                || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today)));
            }
            else
            {
                return Entities.Count(e => e.IsDeleted == false
                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                && (e.FirstName.ToLower().StartsWith(searchText.ToLower())
                || e.LastName.ToLower().StartsWith(searchText.ToLower())));
            }
        }

        public IEnumerable<PracticeHeadCountDto> GetPracticeWiseHeadCount()
        {
            if (DataContext.IsPostgresDB)
            {
                return postgresSqlProcessor.GetPracticeWiseHeadCountPostgres();
            }

            DbCommand cmd = DataContext.Database.Connection.CreateCommand();
            cmd.CommandText = "dbo.GetPracticeWiseHeadCount";
            cmd.CommandType = CommandType.StoredProcedure;
            DataContext.Database.Connection.Open();
            DbDataReader reader = cmd.ExecuteReader();
            ObjectResult<PracticeHeadCountDto> items = ((IObjectContextAdapter)DataContext).ObjectContext.Translate<PracticeHeadCountDto>(reader);

            List<PracticeHeadCountDto> records = items.ToList();
            DataContext.Database.Connection.Close();
            return records;
        }

        public IEnumerable<SubPracticeHeadCountDto> GetSubPracticeWiseHeadCount()
        {
            if (DataContext.IsPostgresDB)
            {
                return postgresSqlProcessor.GetSubPracticeWiseHeadCountFromPostgres();
            }

            DbCommand cmd = DataContext.Database.Connection.CreateCommand();
            cmd.CommandText = "dbo.GetSubPracticeWiseHeadCount";
            cmd.CommandType = CommandType.StoredProcedure;
            DataContext.Database.Connection.Open();
            DbDataReader reader = cmd.ExecuteReader();
            ObjectResult<SubPracticeHeadCountDto> items = ((IObjectContextAdapter)DataContext).ObjectContext.Translate<SubPracticeHeadCountDto>(reader);

            List<SubPracticeHeadCountDto> records = items.ToList();
            DataContext.Database.Connection.Close();
            return records;
        }

        public IEnumerable<EmployeeDto> GetAllByPractice(int practiceID, int pageSize = -1, int pageNo = -1)
        {
            IEnumerable<EmployeeDto> employees = GetAllActiveEmployees("");

            if (pageSize <= 0 || pageNo < 1)
            {
                return employees;
            }

            return employees.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<EmployeeDto> GetAllBySubPractice(int subPracticeID, int pageSize = -1, int pageNo = -1)
        {
            IEnumerable<EmployeeDto> employees = GetAllActiveEmployees("");

            if (pageSize <= 0 || pageNo < 1)
            {
                return employees;
            }

            return employees.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public int PracticeWiseRecordsCount(int practiceID)
        {
            return Entities.Count(e => e.IsDeleted == false
                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today)));
        }

        public int SubPracticeWiseRecordsCount(int subPracticeID)
        {
            return Entities.Count(e => e.IsDeleted == false
                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today)));
        }

        public ResourceCountDto GetEmployeesCountSummary()
        {
            ResourceCountDto dto = new ResourceCountDto
            {

                TotalCount = (from e in Entities
                              where e.IsDeleted == false
              && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay.Value >= DateTime.Today))
                              select e).Count(),
                DeliveryCount = GetEmployeesCountByBU(BusinessUnit.Delivery),
                BoCount = GetEmployeesCountByBU(BusinessUnit.BusinessOperations),
                BdCount = GetEmployeesCountByBU(BusinessUnit.BusinessDevelopment),
            };

            return dto;
        }

        public BillabilityWiseResourceCountDto GetBillabilityCountSummary()
        {
            BillabilityWiseResourceCountDto dto = new BillabilityWiseResourceCountDto
            {
                BillableCount = GetEmployeesCountByAllocationType(AllocationType.Billable),
                CommittedBufferCount = GetEmployeesCountByAllocationType(AllocationType.CommittedBuffer),
                NonCommittedBufferCount = GetEmployeesCountByAllocationType(AllocationType.NonCommittedBuffer),
            };

            return dto;
        }

        public string GetNameByEmployeeID(string empID)
        {
            return Entities.FirstOrDefault(e => e.EmployeeID == empID)?.FirstName;
        }

        public string GetEmailID(int employeeID)
        {
            return Entities.FirstOrDefault(e => e.EmployeeEntryID == employeeID)?.EmailID;
        }

        public IEnumerable<EmployeeVisaDto> GetVisaHoldingEmployees()
        {
            IEnumerable<EmployeeVisaDto> entries = null;

            entries = (
                from e in Entities
                join vc in DataContext.DropDownSubCategories on e.VisaCategoryID equals vc.SubCategoryID into vce
                from vcd in vce.DefaultIfEmpty()
                join al in DataContext.ProjectAllocations on e.EmployeeEntryID equals al.EmployeeID into ale
                from ald in ale.DefaultIfEmpty()
                join at in DataContext.DropDownSubCategories on ald.AllocationTypeID equals at.SubCategoryID into ate
                from atd in ate.DefaultIfEmpty()
                join pr in DataContext.Projects on ald.ProjectID equals pr.ProjectID into pre
                from prd in pre.DefaultIfEmpty()
                where e.VisaCategoryID.HasValue && e.IsDeleted == false
                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay.Value >= DateTime.Today))
                && ald.AllocationEndDate >= DateTime.Today && ald.IsDeleted == false
                orderby e.FirstName, e.LastName
                select new EmployeeVisaDto
                {
                    AllocationType = atd.SubCategoryName,
                    EmployeeID = e.EmployeeID,
                    EmployeeEntryID = e.EmployeeEntryID,
                    EmployeeName = e.FirstName + " " + e.LastName,
                    PrimarySkills = e.PrimarySkills,
                    ProjectName = prd.ProjectName,
                    SecondarySkills = e.SecondarySkills,
                    VisaCategory = vcd.SubCategoryName,
                    VisaValidity = e.VisaValidUpto
                });
            return entries;
        }

        public IEnumerable<EmpCertificationDto> GetCertificationsByEmployeeID(int id)
        {
            return (from ec in DataContext.EmpCertifications
                    join c in DataContext.Certifications on ec.CertificationID equals c.CertificationID into ce
                    from cd in ce.DefaultIfEmpty()
                    where ec.IsDeleted == false && ec.EmployeeID == id && cd.IsDeleted == false
                    select new EmpCertificationDto
                    {
                        CertificationID = cd.CertificationID,
                        CertificationName = cd.Name,
                        EmployeeID = ec.CertificationID,
                        EntryID = ec.EntryID,
                        ShortName = cd.ShortName,
                        ValidUpto = ec.ValidUpto,
                        CertifiedOn = ec.CertifiedOn,
                    });
        }

        public void AddCertification(EmpCertificationDto empCertification)
        {
            EmpCertification empCert = new EmpCertification
            {
                CertificationID = empCertification.CertificationID,
                CertifiedOn = empCertification.CertifiedOn,
                EmployeeID = empCertification.EmployeeID,
                ValidUpto = empCertification.ValidUpto,
            };
            empCert.UpdateTimeStamp(empCertification.LoggedInUserName, true);

            DataContext.EmpCertifications.Add(empCert);
            DataContext.Entry(empCert).State = EntityState.Added;
            DataContext.SaveChanges();
        }

        public void DeleteCertification(EmpCertificationDto empCertification)
        {
            EmpCertification empCert = DataContext.EmpCertifications.FirstOrDefault(ec => ec.EntryID == empCertification.EntryID);

            empCert.IsDeleted = true;
            empCert.UpdateTimeStamp(empCertification.LoggedInUserName);
            DataContext.EmpCertifications.Add(empCert);
            DataContext.Entry(empCert).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public EmployeeDto GetByEmployeeID(string employeeID)
        {
            return (from e in Entities
                    where e.IsDeleted == false && e.EmployeeID == employeeID
                    select new EmployeeDto
                    {
                        EmployeeEntryID = e.EmployeeEntryID,
                        EmployeeID = e.EmployeeID,
                        EmailID = e.EmailID,
                    }).FirstOrDefault();
        }

        public IEnumerable<EmpAndAllocationDto> GetAllEmployeesWithAllocationDetails()
        {
            List<EmpAndAllocationDto> emps = (from emp in Entities
                                              join rm in Entities on emp.ReportingManagerID equals rm.EmployeeEntryID into rme
                                              from rmd in rme.DefaultIfEmpty()
                                              where emp.IsDeleted == false && emp.LastWorkingDay.HasValue == false
                                              || (emp.LastWorkingDay.HasValue == true && emp.LastWorkingDay.Value >= DateTime.Today)
                                              orderby emp.EmployeeID
                                              select new EmpAndAllocationDto
                                              {
                                                  EmployeeEntryID = emp.EmployeeEntryID,
                                                  EmployeeID = emp.EmployeeID,
                                                  EmployeeName = emp.FirstName + " " + emp.LastName,
                                                  EmploymentType = DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryID == emp.EmploymentTypeID).SubCategoryName,
                                                  BusinessUnit = DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryID == emp.BusinessUnitID).SubCategoryName,
                                                  PrimarySkills = emp.PrimarySkills,
                                                  VisaCategory = emp.VisaCategoryID.HasValue ? DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryID == emp.VisaCategoryID).SubCategoryName : "",
                                                  VisaValidUpto = emp.VisaValidUpto,
                                                  PastExperience = emp.PastExperience,
                                                  ProjectManager = "",
                                                  ReportingManager = rmd.FirstName + " " + rmd.LastName,
                                                  DateOfJoin = emp.DateOfJoin,
                                              }).Distinct().ToList();

            foreach (EmpAndAllocationDto emp in emps)
            {
                EmpAssetDetail asset = DataContext.EmpAssetDetails.FirstOrDefault(ea => ea.EmployeeEntryID == emp.EmployeeEntryID);
                emp.PrimarySkills = asset?.PrimarySkills + ";" + asset?.SecondarySkills;
                emp.PastExperience = asset?.PastExperience;
                DateTime today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                ProjectAllocation allocation = DataContext.ProjectAllocations.Where(pa => pa.EmployeeID == emp.EmployeeEntryID && pa.IsDeleted == false
                && pa.AllocationEndDate >= today).OrderByDescending(al => al.AllocationEndDate).FirstOrDefault();

                if (allocation != null)
                {
                    Project project = DataContext.Projects.FirstOrDefault(e => e.ProjectID == allocation.ProjectID && e.IsDeleted == false);
                    if (project != null)
                    {
                        emp.ProjectName = project.ProjectName;

                        Employee pm = Entities.FirstOrDefault(e => e.EmployeeEntryID == project.ProjectManagerID);
                        emp.ProjectManager = $"{pm?.FirstName} {pm?.LastName}";
                    }

                    DropDownSubCategory allType = DataContext.DropDownSubCategories.FirstOrDefault(s => s.IsDeleted == false && s.SubCategoryID == allocation.AllocationTypeID);
                    emp.AllocationStartDate = allocation.AllocationStartDate;
                    emp.AllocationEndDate = allocation.AllocationEndDate;
                }
            }
            return emps;
        }

        #endregion

        #region Private Methods

        private void UpdateEmployeeIDTracker(int trackerID, int runningID)
        {
            EmployeeIDTracker tracker = DataContext.EmployeeIDTrackers.FirstOrDefault(e => e.EmploymentTypeID == trackerID);
            tracker.RunningID = runningID;
            DataContext.EmployeeIDTrackers.Add(tracker);
            DataContext.Entry(tracker).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        private bool UpdateEmployeeIDTracker(int employeeType, string employeeID)
        {
            EmployeeIDTracker tracker = DataContext.EmployeeIDTrackers.FirstOrDefault(e => e.EmploymentTypeID == employeeType);

            if (string.IsNullOrEmpty(tracker.IDPrefix) == false)
            {
                employeeID = employeeID.Replace(tracker.IDPrefix, "");
            }

            if (int.TryParse(employeeID, out int runningID))
            {
                tracker.RunningID = runningID;
                DataContext.EmployeeIDTrackers.Add(tracker);
                DataContext.Entry(tracker).State = EntityState.Modified;
                DataContext.SaveChanges();
                return true;
            }

            return false;
        }

        private IEnumerable<EmployeeDto> GetAllActiveEmployees(string searchText)
        {
            IQueryable<EmployeeDto> employees = null;
            employees = from emp in Entities
                        join bc in DataContext.DropDownSubCategories on emp.BusinessUnitID equals bc.SubCategoryID into bue
                        from bcd in bue.DefaultIfEmpty()
                        join ut in DataContext.DropDownSubCategories on emp.UtilizationTypeID equals ut.SubCategoryID into ute
                        from utd in ute.DefaultIfEmpty()
                        join et in DataContext.DropDownSubCategories on emp.EmploymentTypeID equals et.SubCategoryID into ete
                        from etd in ete.DefaultIfEmpty()
                        join rm in Entities on emp.ReportingManagerID equals rm.EmployeeEntryID into rme
                        from rmd in rme.DefaultIfEmpty()

                        where emp.IsDeleted == false && emp.LastWorkingDay.HasValue == false
                        || (emp.LastWorkingDay.HasValue == true && emp.LastWorkingDay.Value >= DateTime.Today)
                        orderby emp.EmployeeID
                        select new EmployeeDto
                        {
                            BusinessUnitName = bcd.SubCategoryName,
                            DateOfJoin = emp.DateOfJoin,
                            EmployeeEntryID = emp.EmployeeEntryID,
                            EmployeeID = emp.EmployeeID,
                            FirstName = emp.FirstName,
                            LastName = emp.LastName,
                            PrimarySkills = emp.PrimarySkills,
                            UtilizationTypeName = utd.SubCategoryName,
                            EmploymentTypeName = etd.SubCategoryName,
                            ReportingManagerName = rmd.FirstName + " " + rmd.LastName,
                            Certifications = DataContext.EmpCertifications.Count(ec => ec.IsDeleted == false && ec.EmployeeID == emp.EmployeeEntryID),
                            IsTechResource = emp.IsTechResource,
                            OverallExperience = emp.OverallExperience,
                            PastExperience = emp.PastExperience,
                        };

            if (string.IsNullOrEmpty(searchText) == false)
            {
                employees = employees.Where(e => e.FirstName.ToLower().StartsWith(searchText) || e.LastName.ToLower().StartsWith(searchText));
            }
            return employees;
        }

        private Employee CreateBusinessEntity(EmployeeDto employeeDto, bool isNewEntity = false)
        {
            Employee employee = new Employee
            {
                BusinessUnitID = employeeDto.BusinessUnitID,
                DateOfJoin = new DateTime(employeeDto.DateOfJoin.Year, employeeDto.DateOfJoin.Month, employeeDto.DateOfJoin.Day),
                EmailID = employeeDto.EmailID,
                EmployeeID = employeeDto.EmployeeID,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                LastWorkingDay = employeeDto.LastWorkingDay,
                PrimarySkills = employeeDto.PrimarySkills,
                ReportingManagerID = employeeDto.ReportingManagerID,
                SecondarySkills = employeeDto.SecondarySkills,
                UtilizationTypeID = employeeDto.UtilizationTypeID,
                EmployeeEntryID = employeeDto.EmployeeEntryID,
                EmploymentTypeID = employeeDto.EmploymentTypeID,
                PassportNo = employeeDto.PassportNo,
                PassportValidUpto = employeeDto.PassportValidUpto,
                TechnicalRank = employeeDto.TechnicalRank,
                OverallExperience = employeeDto.OverallExperience,
                PastExperience = employeeDto.PastExperience,
                VisaCategoryID = employeeDto.VisaCategoryID,
                VisaValidUpto = employeeDto.VisaValidUpto,
                IsDeleted = false,
                StrengthAreaID = employeeDto.StrengthAreaID,
                Level1ID = employeeDto.Level1ID,
                Level2ID = employeeDto.Level2ID,
                Level3ID = employeeDto.Level3ID,
                Level4ID = employeeDto.Level4ID,
                Level5ID = employeeDto.Level5ID,
                IsManager = employeeDto.IsManager.HasValue ? employeeDto.IsManager : false,
                IsTechResource = employeeDto.IsTechResource.HasValue ? employeeDto.IsTechResource : false,
            };

            employee.UpdateTimeStamp(employeeDto.LoggedInUserName, true);
            return employee;
        }

        private void MigrateEntity(EmployeeDto sourceEntity, Employee targetEntity)
        {
            targetEntity.BusinessUnitID = sourceEntity.BusinessUnitID;
            targetEntity.DateOfJoin = new DateTime(sourceEntity.DateOfJoin.Year, sourceEntity.DateOfJoin.Month, sourceEntity.DateOfJoin.Day);
            targetEntity.EmailID = sourceEntity.EmailID;
            targetEntity.EmployeeID = sourceEntity.EmployeeID;
            targetEntity.FirstName = sourceEntity.FirstName;
            targetEntity.LastName = sourceEntity.LastName;
            targetEntity.LastWorkingDay = sourceEntity.LastWorkingDay;
            targetEntity.PrimarySkills = sourceEntity.PrimarySkills;
            targetEntity.ReportingManagerID = sourceEntity.ReportingManagerID;
            targetEntity.SecondarySkills = sourceEntity.SecondarySkills;
            targetEntity.UtilizationTypeID = sourceEntity.UtilizationTypeID;
            targetEntity.EmployeeEntryID = sourceEntity.EmployeeEntryID;
            targetEntity.EmploymentTypeID = sourceEntity.EmploymentTypeID;
            targetEntity.PassportNo = sourceEntity.PassportNo;
            targetEntity.PassportValidUpto = sourceEntity.PassportValidUpto;
            targetEntity.TechnicalRank = sourceEntity.TechnicalRank;
            targetEntity.OverallExperience = sourceEntity.OverallExperience;
            targetEntity.PastExperience = sourceEntity.PastExperience;
            targetEntity.VisaCategoryID = sourceEntity.VisaCategoryID;
            targetEntity.VisaValidUpto = sourceEntity.VisaValidUpto;
            targetEntity.Level1ID = sourceEntity.Level1ID;
            targetEntity.Level2ID = sourceEntity.Level2ID;
            targetEntity.Level3ID = sourceEntity.Level3ID;
            targetEntity.Level4ID = sourceEntity.Level4ID;
            targetEntity.Level5ID = sourceEntity.Level5ID;
            targetEntity.IsManager = sourceEntity.IsManager.HasValue ? sourceEntity.IsManager : false;
            targetEntity.IsTechResource = sourceEntity.IsTechResource.HasValue ? sourceEntity.IsTechResource : false;

            targetEntity.UpdateTimeStamp(sourceEntity.LoggedInUserName);
        }

        private int GetEmployeesCountByAllocationType(AllocationType allocationType)
        {
            return (from e in Entities
                    join a in DataContext.ProjectAllocations on e.EmployeeEntryID equals a.EmployeeID
                    where a.AllocationTypeID == (int)allocationType
                    && e.LastWorkingDay.HasValue == false
                    && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                    && e.IsDeleted == false && a.IsDeleted == false
                    && a.AllocationEndDate >= DateTime.Today
                    select a.EmployeeID).Distinct().Count();
        }

        private int GetEmployeesCountByBU(BusinessUnit bu)
        {
            return (from e in Entities
                    where e.IsDeleted == false
                   && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                   && e.BusinessUnitID == (int)bu
                    select e).Count();
        }

        #endregion
    }

    public interface IEmployeeRepository : IRepository<EmployeeDto>
    {
        bool IsDuplicateName(string firstName, string lastName);

        bool IsDuplicateName(int employeeEntryID, string firstName, string lastName);

        bool IsDuplicateEmployeeID(string employeeID);

        bool IsDuplicateEmployeeID(int employeeEntryID, string employeeID);

        string GenerateNewEmployeeID(int employeeTypeID);

        IEnumerable<EmployeeDto> GetAllManagers();

        IEnumerable<EmployeeDto> GetAllPastEmployees(int pageSize = -1, int pageNo = -1);

        int GetPastEmployeesCount();

        int TotalRecordsCount(string searchText);

        IEnumerable<EmployeeDto> GetAll(string searchText, int pageSize = -1, int pageNo = -1);

        IEnumerable<PracticeHeadCountDto> GetPracticeWiseHeadCount();

        IEnumerable<SubPracticeHeadCountDto> GetSubPracticeWiseHeadCount();

        IEnumerable<EmployeeDto> GetAllByPractice(int practiceID, int pageSize = -1, int pageNo = -1);

        IEnumerable<EmployeeDto> GetAllBySubPractice(int subPracticeID, int pageSize = -1, int pageNo = -1);

        int PracticeWiseRecordsCount(int practiceID);

        int SubPracticeWiseRecordsCount(int subPracticeID);

        ResourceCountDto GetEmployeesCountSummary();

        IEnumerable<EmployeeDto> GetAllAccountManagers();

        string GetNameByEmployeeID(string empID);

        BillabilityWiseResourceCountDto GetBillabilityCountSummary();

        string GetEmailID(int employeeID);

        IEnumerable<EmployeeVisaDto> GetVisaHoldingEmployees();

        IEnumerable<EmpCertificationDto> GetCertificationsByEmployeeID(int id);

        void AddCertification(EmpCertificationDto empCertification);

        void DeleteCertification(EmpCertificationDto empCertification);

        EmployeeDto GetByEmployeeID(string employeeID);

        IEnumerable<EmpAndAllocationDto> GetAllEmployeesWithAllocationDetails();
    }
}

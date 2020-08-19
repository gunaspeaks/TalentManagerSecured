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
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class AllocationRepository : RepositoryBase<ProjectAllocation>, IAllocationRepository
    {
        private readonly PostgresSqlProcessor postgresSqlProcessor = null;

        public AllocationRepository()
        {
            postgresSqlProcessor = new PostgresSqlProcessor();
        }

        #region Public Methods

        public void Add(ProjectAllocationDto entity)
        {
            //int prjBU = DataContext.Projects.FirstOrDefault(p => p.ProjectID == entity.ProjectID).BusinessUnitID;
            //int empBU= DataContext.Employees.FirstOrDefault(p => p.EmployeeEntryID == entity.EmployeeID).BusinessUnitID;
            //if(prjBU!=empBU)
            //{

            //}

            ProjectAllocation allocation = CreateBusinessEntity(entity, true);
            Entities.Add(allocation);
            DataContext.Entry(allocation).State = EntityState.Added;
            DataContext.SaveChanges();
        }

        public void Delete(ProjectAllocationDto entity)
        {
            ProjectAllocation allocation = Entities.FirstOrDefault(e => e.AllocationEntryID == entity.AllocationEntryID);
            allocation.IsDeleted = true;
            allocation.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(allocation);
            DataContext.Entry(allocation).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public int Exists(int empEntryID, int projectID)
        {
            return Entities.Count(a => a.EmployeeID == empEntryID &&
            a.ProjectID == projectID && a.IsDeleted == false);
        }

        public int Exists(int allocationID, int empEntryID, int projectID)
        {
            return Entities.Count(a => a.AllocationEntryID != allocationID &&
            a.EmployeeID == empEntryID && a.ProjectID == projectID && a.IsDeleted == false);
        }

        public bool Exists(int id)
        {
            return Entities.Any(a => a.AllocationEntryID == id && a.IsDeleted == false);
        }

        public bool Exists(string projectName)
        {
            return (from a in Entities
                    join p in DataContext.Projects on a.ProjectID equals p.ProjectID into pe
                    from ped in pe.DefaultIfEmpty()
                    where ped.ProjectName == projectName && a.IsDeleted == false && ped.IsDeleted == false
                    select a).Any();
        }

        public IEnumerable<ProjectAllocationDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            return GetAll("", 0, "", "", pageSize, pageNo);
        }

        public IQueryable<ProjectAllocationDto> GetAllRecords(int pageSize = -1, int pageNo = -1)
        {
            IQueryable<ProjectAllocationDto> allocations = from p in Entities
                                                           join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                                                           from emd in eme.DefaultIfEmpty()
                                                           join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                                                           from scd in sce.DefaultIfEmpty()
                                                           join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                                                           from prd in pre.DefaultIfEmpty()
                                                           join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                                                           from pmd in pme.DefaultIfEmpty()
                                                           join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                                                           from acd in ace.DefaultIfEmpty()
                                                           where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today
                                                           orderby prd.ProjectName, p.AllocationStartDate
                                                           select new ProjectAllocationDto
                                                           {
                                                               AllocationEndDate = p.AllocationEndDate,
                                                               AllocationEntryID = p.AllocationEntryID,
                                                               AllocationStartDate = p.AllocationStartDate,
                                                               AllocationTypeName = scd.SubCategoryName,
                                                               EmployeeName = emd.FirstName + " " + emd.LastName,
                                                               ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                                                               EmployeeID = p.EmployeeID,
                                                               ProjectName = prd.ProjectName,
                                                               Remarks = p.Remarks,
                                                               PercentageOfAllocation = p.PercentageOfAllocation,
                                                               AccountName = acd.AccountName,
                                                               ProjectID = prd.ProjectID
                                                           };
            return allocations;

        }

        public IEnumerable<ProjectAllocationDto> GetAll(string filterType, int filterValueID, string sortBy = "empname", string sortType = "asc", int pageSize = -1, int pageNo = -1)
        {
            IQueryable<ProjectAllocationDto> allocations = null;
            if (string.IsNullOrWhiteSpace(filterType))
            {
                allocations = GetAllRecords(pageSize, pageNo);
            }

            switch (filterType?.ToLower())
            {
                case "prj":
                    allocations = GetAllocationsByProjectID(filterValueID);
                    break;
                case "pm":
                    allocations = GetAllocationsByProjectManagerID(filterValueID);
                    break;
                case "emp":
                    allocations = GetAllocationsByEmployeeID(filterValueID);
                    break;
            }

            allocations = SortAllocationItems(allocations, sortBy, sortType);

            if (pageSize < 0)
            {
                return allocations;
            }
            return allocations.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<ProjectAllocationDto> GetAllAllocationsByProjectID(int projectID)
        {
            return from p in Entities
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   where p.IsDeleted == false && p.ProjectID == projectID
                   && p.AllocationEndDate >= DateTime.Today
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryID == p.AllocationTypeID).SubCategoryName,
                       EmployeeName = (from e in DataContext.Employees where e.EmployeeEntryID == p.EmployeeID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                       ProjectManagerName = (from e in DataContext.Employees where e.EmployeeEntryID == prd.ProjectManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = DataContext.ProjectAccounts.FirstOrDefault(a => a.AccountID == prd.ProjectAccountID).AccountName
                   };

        }

        private IQueryable<ProjectAllocationDto> GetAllocationsByEmployeeID(int empID)
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                   from emd in eme.DefaultIfEmpty()
                   join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                   from scd in sce.DefaultIfEmpty()
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                   from pmd in pme.DefaultIfEmpty()
                   join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                   from acd in ace.DefaultIfEmpty()
                   where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today && p.EmployeeID == empID
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = scd.SubCategoryName,
                       EmployeeName = emd.FirstName + " " + emd.LastName,
                       ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = acd.AccountName
                   };
        }

        private IQueryable<ProjectAllocationDto> GetAllocationsByProjectID(int projectID)
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                   from emd in eme.DefaultIfEmpty()
                   join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                   from scd in sce.DefaultIfEmpty()
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                   from pmd in pme.DefaultIfEmpty()
                   join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                   from acd in ace.DefaultIfEmpty()
                   where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today && p.ProjectID == projectID
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = scd.SubCategoryName,
                       EmployeeName = emd.FirstName + " " + emd.LastName,
                       ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = acd.AccountName
                   };
        }

        private IQueryable<ProjectAllocationDto> GetAllocationsByProjectManagerID(int managerID)
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                   from emd in eme.DefaultIfEmpty()
                   join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                   from scd in sce.DefaultIfEmpty()
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                   from pmd in pme.DefaultIfEmpty()
                   join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                   from acd in ace.DefaultIfEmpty()
                   where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today && prd.ProjectManagerID == managerID
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = scd.SubCategoryName,
                       EmployeeName = emd.FirstName + " " + emd.LastName,
                       ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = acd.AccountName
                   };
        }

        public ProjectAllocationDto GetByID(int id)
        {
            return (from p in Entities
                    where p.AllocationEntryID == id
                    select new ProjectAllocationDto
                    {
                        AllocationEndDate = p.AllocationEndDate,
                        AllocationEntryID = p.AllocationEntryID,
                        AllocationStartDate = p.AllocationStartDate,
                        AllocationTypeID = p.AllocationTypeID,
                        EmployeeID = p.EmployeeID,
                        ProjectID = p.ProjectID,
                        Remarks = p.Remarks,
                        PercentageOfAllocation = p.PercentageOfAllocation,
                        IsActive = p.IsActive,
                        BenchCategoryID = p.BenchCategoryID,
                    }).FirstOrDefault();
        }

        public void Update(ProjectAllocationDto entity)
        {
            ProjectAllocation buzEntity = Entities.FirstOrDefault(e => e.AllocationEntryID == entity.AllocationEntryID);
            MigrateEntity(entity, buzEntity);
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public int GetPercentageOfAllocation(int employeeID)
        {
            if (Entities.Any(a => a.EmployeeID == employeeID && a.AllocationEndDate >= DateTime.Today && a.IsDeleted == false))
            {
                return Entities.Where(a => a.EmployeeID == employeeID && a.AllocationEndDate >= DateTime.Today && a.IsDeleted == false)
                    .Sum(p => p.PercentageOfAllocation);
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<CustomAllocationDto> GetAllocatedProjectsByEmployeeID(int employeeID)
        {
            return (from a in Entities
                    join p in DataContext.Projects on a.ProjectID equals p.ProjectID into pe
                    from pd in pe.DefaultIfEmpty()
                    join sc in DataContext.DropDownSubCategories on a.AllocationTypeID equals sc.SubCategoryID into sce
                    from scd in sce.DefaultIfEmpty()
                    join e in DataContext.Employees on a.EmployeeID equals e.EmployeeEntryID into ee
                    from ed in ee.DefaultIfEmpty()
                    join bu in DataContext.DropDownSubCategories on ed.BusinessUnitID equals bu.SubCategoryID into bue
                    from bud in bue.DefaultIfEmpty()
                    join pm in DataContext.Employees on pd.ProjectManagerID equals pm.EmployeeEntryID into pme
                    from pmd in pme.DefaultIfEmpty()
                    join dm in DataContext.Employees on pd.ProjectManagerID equals dm.EmployeeEntryID into dme
                    from dmd in dme.DefaultIfEmpty()
                    where a.EmployeeID == employeeID && a.AllocationEndDate >= DateTime.Today && a.IsDeleted == false
                    select new CustomAllocationDto
                    {
                        AllocatedPercentage = a.PercentageOfAllocation,
                        EndDate = a.AllocationEndDate,
                        ProjectCode = pd.ProjectCode,
                        ProjectManager = string.IsNullOrEmpty(pmd.FirstName) ? "" : pmd.LastName + ", " + pmd.FirstName,
                        ProjectName = pd.ProjectName,
                        StartDate = a.AllocationStartDate,
                        UtilizatinType = scd.SubCategoryName,
                        BusinessUnit = bud.SubCategoryName,
                        DeliveryManager = string.IsNullOrEmpty(dmd.FirstName) ? "" : dmd.LastName + ", " + dmd.FirstName,
                    });
        }

        public int GetTotalCountForAllocationHistory(string filterType, int filterValue)
        {
            int recordsCount = 0;
            if (string.IsNullOrWhiteSpace(filterType) || filterType?.ToLower() == "please select")
            {
                recordsCount = (from a in Entities
                                where a.IsDeleted == false && a.AllocationEndDate <= DateTime.Today
                                select a).Count();
            }

            switch (filterType?.ToLower())
            {
                case "emp":
                    recordsCount = (from a in Entities
                                    where a.IsDeleted == false && a.AllocationEndDate < DateTime.Today
                                        && a.EmployeeID == filterValue
                                    select a).Count();
                    break;
                case "prj":
                    recordsCount = (from a in Entities
                                    where a.IsDeleted == false & a.AllocationEndDate < DateTime.Today
                                        && a.ProjectID == filterValue
                                    select a).Count();
                    break;
                case "pm":
                    recordsCount = (from a in Entities
                                    join p in DataContext.Projects on a.ProjectID equals p.ProjectID into pe
                                    from pd in pe.DefaultIfEmpty()
                                    where a.IsDeleted == false && a.AllocationEndDate < DateTime.Today
                                        && pd.ProjectManagerID == filterValue
                                    select a).Count();
                    break;
            }

            return recordsCount;
        }

        public IEnumerable<ProjectAllocationDto> GetAllocationHistory(string filterType, int filterValue, string sortBy, string sortType, int pageSize = -1, int pageNo = -1)
        {
            IQueryable<ProjectAllocationDto> allocations = null;

            if (string.IsNullOrWhiteSpace(filterType) || filterType?.ToLower() == "please select")
            {
                allocations = GetAllAllocationHistory();
            }

            switch (filterType)
            {
                case "emp":
                    allocations = GetAllAllocationHistoryByEmployeeID(filterValue);
                    break;
                case "prj":
                    allocations = GetAllAllocationHistoryByProject(filterValue);
                    break;
                case "pm":
                    allocations = GetAllAllocationHistoryByManagerID(filterValue);
                    break;
            }

            allocations = SortAllocationItems(allocations, sortBy, sortType);

            if (pageSize < 0)
            {
                return allocations;
            }
            return allocations.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<EmpArchitectDto> GetAllArchitectEmployees()
        {
            List<EmpArchitectDto> architects = new List<EmpArchitectDto>();
            List<Employee> emps = new List<Employee>();
                emps = (from e in DataContext.Employees
                        where e.IsDeleted == false
                        && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay >= DateTime.Today))
                        && (e.IsArchitect.HasValue == false || (e.IsArchitect.HasValue && e.IsArchitect == true))
                        select e).ToList();
            foreach (Employee emp in emps)
            {
                var arc = new EmpArchitectDto
                {
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = $"{emp.FirstName} {emp.LastName}",
                };

                var alloc = (from a in Entities
                             where a.IsDeleted == false
               && a.EmployeeID == emp.EmployeeEntryID && a.AllocationEndDate >= DateTime.Today
                             orderby a.AllocationEndDate descending
                             select a).FirstOrDefault();
                if (alloc != null)
                {
                    var prj = DataContext.Projects.FirstOrDefault(p => p.ProjectID == alloc.ProjectID);
                    if (prj != null)
                    {
                        arc.ProjectName = prj.ProjectName;
                        arc.AccountName = DataContext.ProjectAccounts.FirstOrDefault(a => a.AccountID == prj.ProjectAccountID)?.AccountName;
                    }
                    arc.AllocatedFrom = alloc.AllocationStartDate;
                    arc.AllocatedUpTo = alloc.AllocationEndDate;
                    arc.AllocationType = DataContext.DropDownSubCategories.FirstOrDefault(a => a.SubCategoryID == alloc.AllocationTypeID)?.SubCategoryName;
                }
                architects.Add(arc);
            }
            return architects;
        }

        private IQueryable<ProjectAllocationDto> SortAllocationItems(IQueryable<ProjectAllocationDto> allocations, string sortBy, string sortType)
        {
            switch (sortBy?.ToLower())
            {
                case "pname":
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.ProjectName) :
                        allocations.OrderByDescending(a => a.ProjectName);
                    break;
                case "pmname":
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.ProjectName) :
                        allocations.OrderByDescending(a => a.ProjectManagerName);
                    break;
                case "accname":
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.AccountName) :
                        allocations.OrderByDescending(a => a.AccountName);
                    break;
                case "altype":
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.AllocationTypeName) :
                        allocations.OrderByDescending(a => a.AllocationTypeName);
                    break;
                case "percent":
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.PercentageOfAllocation) :
                        allocations.OrderByDescending(a => a.PercentageOfAllocation);
                    break;
                case "sdate":
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.AllocationStartDate) :
                        allocations.OrderByDescending(a => a.AllocationStartDate);
                    break;
                case "edate":
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.AllocationEndDate) :
                       allocations.OrderByDescending(a => a.AllocationEndDate);
                    break;
                default:
                    allocations = sortType == "asc" ? allocations.OrderBy(a => a.EmployeeName) :
                        allocations.OrderByDescending(a => a.EmployeeName);
                    break;
            }

            return allocations;
        }

        private IQueryable<ProjectAllocationDto> GetAllAllocationHistory()
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                   from emd in eme.DefaultIfEmpty()
                   join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                   from scd in sce.DefaultIfEmpty()
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                   from pmd in pme.DefaultIfEmpty()
                   join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                   from acd in ace.DefaultIfEmpty()
                   where p.IsDeleted == false && p.AllocationEndDate < DateTime.Today
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = scd.SubCategoryName,
                       EmployeeName = emd.FirstName + " " + emd.LastName,
                       ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = acd.AccountName
                   };
        }

        private IQueryable<ProjectAllocationDto> GetAllAllocationHistoryByProject(int projectID)
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                   from emd in eme.DefaultIfEmpty()
                   join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                   from scd in sce.DefaultIfEmpty()
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                   from pmd in pme.DefaultIfEmpty()
                   join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                   from acd in ace.DefaultIfEmpty()
                   where p.IsDeleted == false
                        && p.AllocationEndDate < DateTime.Today
                        && p.ProjectID == projectID
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = scd.SubCategoryName,
                       EmployeeName = emd.FirstName + " " + emd.LastName,
                       ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = acd.AccountName
                   };
        }

        private IQueryable<ProjectAllocationDto> GetAllAllocationHistoryByManagerID(int managerID)
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                   from emd in eme.DefaultIfEmpty()
                   join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                   from scd in sce.DefaultIfEmpty()
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                   from pmd in pme.DefaultIfEmpty()
                   join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                   from acd in ace.DefaultIfEmpty()
                   where p.IsDeleted == false && p.AllocationEndDate < DateTime.Today && prd.ProjectManagerID == managerID
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = scd.SubCategoryName,
                       EmployeeName = emd.FirstName + " " + emd.LastName,
                       ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = acd.AccountName
                   };



        }

        private IQueryable<ProjectAllocationDto> GetAllAllocationHistoryByEmployeeID(int employeeID)
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID into eme
                   from emd in eme.DefaultIfEmpty()
                   join sc in DataContext.DropDownSubCategories on p.AllocationTypeID equals sc.SubCategoryID into sce
                   from scd in sce.DefaultIfEmpty()
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID into pre
                   from prd in pre.DefaultIfEmpty()
                   join pm in DataContext.Employees on prd.ProjectManagerID equals pm.EmployeeEntryID into pme
                   from pmd in pme.DefaultIfEmpty()
                   join ac in DataContext.ProjectAccounts on prd.ProjectAccountID equals ac.AccountID into ace
                   from acd in ace.DefaultIfEmpty()
                   where p.IsDeleted == false && p.AllocationEndDate < DateTime.Today && p.EmployeeID == employeeID
                   orderby prd.ProjectName, p.AllocationStartDate
                   select new ProjectAllocationDto
                   {
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationTypeName = scd.SubCategoryName,
                       EmployeeName = emd.FirstName + " " + emd.LastName,
                       ProjectManagerName = pmd.FirstName + " " + pmd.LastName,
                       EmployeeID = p.EmployeeID,
                       ProjectName = prd.ProjectName,
                       Remarks = p.Remarks,
                       PercentageOfAllocation = p.PercentageOfAllocation,
                       AccountName = acd.AccountName
                   };
        }

        public override int TotalRecordsCount()
        {
            return Entities.Count(e => e.IsDeleted == false && e.AllocationEndDate >= DateTime.Today);
        }

        public int TotalRecordsCount(string filterType, int filterValueID)
        {
            if (string.IsNullOrWhiteSpace(filterType))
            {
                return TotalRecordsCount();
            }

            int count = 0;
            switch (filterType)
            {
                case "emp":
                    count = GetAllocationsByEmployeeID(filterValueID).Count();
                    break;
                case "prj":
                    count = GetAllocationsByProjectID(filterValueID).Count();
                    break;
                case "pm":
                    count = GetAllocationsByProjectManagerID(filterValueID).Count();
                    break;
            }

            return count;
        }

        public bool AnyActiveBillableAllocations(int employeeID, int allocationID, DateTime startDate)
        {
            return (from a in Entities
                    join p in DataContext.Projects on a.ProjectID equals p.ProjectID
                    where a.IsDeleted == false && a.AllocationStartDate >= startDate
                        && p.ProjectName.ToLower() != "bench"
                        && a.EmployeeID == employeeID
                        && a.AllocationEntryID != allocationID
                    select a).Any();
        }

        public bool AnyActiveAllocationInBenchProject(int employeeID, DateTime startDate)
        {
            return (from a in Entities
                    join p in DataContext.Projects on a.ProjectID equals p.ProjectID
                    where a.IsDeleted == false && p.IsDeleted == false
                        && p.ProjectName.ToLower().Contains("bench")
                        && a.EmployeeID == employeeID && a.AllocationStartDate >= startDate
                    select a).Any();
        }

        public void EndAllocation(int allocationID)
        {
            ProjectAllocation buzEntity = Entities.FirstOrDefault(e => e.AllocationEntryID == allocationID);
            buzEntity.AllocationEndDate = DateTime.Today.AddDays(-1);
            buzEntity.IsActive = false;
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public IEnumerable<ManagerWiseAllocationDto> GetManagerWiseAllocationSummary()
        {
            if (DataContext.IsPostgresDB)
            {
                return postgresSqlProcessor.GetManagerWiseAllocationSummaryFromPostgres();
            }

            DbCommand cmd = DataContext.Database.Connection.CreateCommand();
            cmd.CommandText = "dbo.GetManagerWiseProjectsSummary";
            cmd.CommandType = CommandType.StoredProcedure;
            DataContext.Database.Connection.Open();
            DbDataReader reader = cmd.ExecuteReader();
            ObjectResult<ManagerWiseAllocationDto> items = ((IObjectContextAdapter)DataContext).ObjectContext.Translate<ManagerWiseAllocationDto>(reader);
            List<ManagerWiseAllocationDto> listItems = items.ToList();
            DataContext.Database.Connection.Close();
            return listItems;
        }

        public IEnumerable<BillabilityWiseAllocationSummaryDto> GetBillabilityWiseAllocationSummary()
        {
#pragma warning disable IDE0028 // Simplify collection initialization
            List<BillabilityWiseAllocationSummaryDto> items = new List<BillabilityWiseAllocationSummaryDto>();
#pragma warning restore IDE0028 // Simplify collection initialization

            items.Add(new BillabilityWiseAllocationSummaryDto
            {
                AllocationType = "Billable",
                AllocationTypeID = (int)AllocationType.Billable,
                NumberOfEmployees = GetEmployeesCountByAllocationType(AllocationType.Billable)
            });

            items.Add(new BillabilityWiseAllocationSummaryDto
            {
                AllocationType = "Committed Buffer",
                AllocationTypeID = (int)AllocationType.CommittedBuffer,
                NumberOfEmployees = GetEmployeesCountByAllocationType(AllocationType.CommittedBuffer)
            });

            items.Add(new BillabilityWiseAllocationSummaryDto
            {
                AllocationType = "Non-Committed Buffer",
                AllocationTypeID = (int)AllocationType.NonCommittedBuffer,
                NumberOfEmployees = GetEmployeesCountByAllocationType(AllocationType.NonCommittedBuffer)
            });

            items.Add(new BillabilityWiseAllocationSummaryDto
            {
                AllocationType = "Bench (Available)",
                AllocationTypeID = -5,
                NumberOfEmployees = GetEmployeesCountByAllocationType(AllocationType.Bench, BenchCategory.Available)
            });

            items.Add(new BillabilityWiseAllocationSummaryDto
            {
                AllocationType = "Bench (Earmarked)",
                AllocationTypeID = -6,
                NumberOfEmployees = GetEmployeesCountByAllocationType(AllocationType.Bench, BenchCategory.Earmarked)
            });

            items.Add(new BillabilityWiseAllocationSummaryDto
            {
                AllocationType = "Not Allocated - Delivery",
                AllocationTypeID = -1,
                NumberOfEmployees = GetNonAllocatedResourcesCount(true),
            });

            items.Add(new BillabilityWiseAllocationSummaryDto
            {
                AllocationType = "BD & BO",
                AllocationTypeID = -2,
                NumberOfEmployees = GetNonAllocatedResourcesCount(false),
            });

            return items;
        }

        public IEnumerable<BillabilityWiseAllocationDetailDto> GetBillabilityWiseAllocationDetail(string filterBy, string filterValue)
        {
            IEnumerable<BillabilityWiseAllocationDetailDto> allocationDetailDtos = null;

            switch (filterBy?.ToLower())
            {
                case "emp":
                case "psk":
                case "ssk":
                case "all":
                    // filter by employee name/ primary skills/ secondary skills /pod
                    allocationDetailDtos = GetAllAllocationDetailFilteredByEmployeeData(filterBy, filterValue);
                    break;
                case "prj":
                case "acc":
                    // filter by project's project name/account
                    break;
                case "alt":
                    // filter by allocation type
                    int.TryParse(filterValue, out int allocationTypeID);
                    switch (allocationTypeID)
                    {
                        case -1:
                            allocationDetailDtos = GetNonAllocatedDirectBuEmp(allocationTypeID);
                            break;
                        case -2:
                            allocationDetailDtos = GetNonAllocatedDirectBuEmp(allocationTypeID);
                            break;
                        case -5:
                            allocationDetailDtos = GetBlockedEmpsForBenchAvailable();
                            break;
                        case -6:
                            allocationDetailDtos = GetBlockedEmpsForBenchEarmarked();
                            break;
                        default:
                            allocationDetailDtos = GetAllocationEntriesByAllocationType(allocationTypeID);
                            break;
                    }

                    //if (DataContext.IsPostgresDB)
                    //{
                    //allocationDetailDtos = postgresSqlProcessor.GetAllocationEntriesByAllocationTypeFromPostgres(allocationTypeID);
                    //}
                    //else
                    //{
                    //    allocationDetailDtos = GetAllocationEntriesByAllocationType(allocationTypeID);
                    //}
                    break;
            }

            return allocationDetailDtos;
        }

        public List<BillabilityWiseAllocationDetailDto> GetNonAllocatedDirectBuEmp(int allocationTypeID)
        {

            List<BillabilityWiseAllocationDetailDto> entries = new List<BillabilityWiseAllocationDetailDto>();
            List<Employee> emps = new List<Employee>();

            if (allocationTypeID == -1)
            {
                // for Delivery 
                emps = (from e in DataContext.Employees
                        where e.IsDeleted == false
                        && (e.LastWorkingDay.HasValue == false || e.LastWorkingDay.HasValue && e.LastWorkingDay >= DateTime.Today)
                        && e.BusinessUnitID == (int)BusinessUnit.Delivery
                        select e).ToList();
            }
            else
            {
                // for BO and BD
                emps = (from e in DataContext.Employees
                        where e.IsDeleted == false
                        && (e.LastWorkingDay.HasValue == false || e.LastWorkingDay.HasValue && e.LastWorkingDay >= DateTime.Today)
                        && (e.BusinessUnitID == (int)BusinessUnit.BusinessDevelopment || e.BusinessUnitID == (int)BusinessUnit.BusinessOperations)
                        select e).ToList();
            }
            List<DropDownSubCategory> buItems = DataContext.DropDownSubCategories.Where(sc => sc.IsDeleted == false && sc.CategoryID == 1).ToList();
            foreach (Employee emp in emps)
            {
                if (Entities.Any(pa => pa.IsDeleted == false && pa.AllocationEndDate >= DateTime.Today && pa.EmployeeID == emp.EmployeeEntryID))
                    continue;
                entries.Add(
                    new BillabilityWiseAllocationDetailDto
                    {
                        AllocationTypeID = allocationTypeID,
                        AllocationType = allocationTypeID == -1 ? "Not Allocated Yet (Delivery)" : "Not Allocated Yet (BD & BO)",
                        EmployeeEntryID = emp.EmployeeEntryID,
                        EmployeeID = emp.EmployeeID,
                        EmployeeName = emp.FirstName + " " + emp.LastName,
                        PrimarySkills = emp.PrimarySkills,
                        SecondarySkills = emp.SecondarySkills,
                        BusinessUnitID = emp.BusinessUnitID,
                        BusinessUnit = buItems.FirstOrDefault(bu => bu.SubCategoryID == emp.BusinessUnitID).SubCategoryName,
                    });
            }
            return entries;
        }

        public List<BillabilityWiseAllocationDetailDto> GetBlockedEmpsForBenchAvailable()
        {
            List<BillabilityWiseAllocationDetailDto> entries = new List<BillabilityWiseAllocationDetailDto>();

            entries = (from pa in Entities
                       join p in DataContext.Projects on pa.ProjectID equals p.ProjectID
                       join e in DataContext.Employees on pa.EmployeeID equals e.EmployeeEntryID
                       where pa.IsDeleted == false && pa.AllocationEndDate >= DateTime.Today && pa.AllocationTypeID == (int)AllocationType.Bench && pa.BenchCategoryID == (int)BenchCategory.Available
                       select new BillabilityWiseAllocationDetailDto
                       {
                           AllocationTypeID = pa.AllocationTypeID,
                           AllocationType = "Bench (Available)",
                           EmployeeEntryID = pa.EmployeeID,
                           EmployeeID = e.EmployeeID,
                           EmployeeName = e.FirstName + " " + e.LastName,
                           PrimarySkills = e.PrimarySkills,
                           SecondarySkills = e.SecondarySkills,
                           BusinessUnitID = e.BusinessUnitID,
                           BusinessUnit = DataContext.DropDownSubCategories.FirstOrDefault(bu => bu.SubCategoryID == e.BusinessUnitID).SubCategoryName,
                           AccountName = (from ac in DataContext.ProjectAccounts where ac.AccountID == p.ProjectAccountID select ac.AccountName).FirstOrDefault(),
                           AllocationEndDate = pa.AllocationEndDate,
                           AllocationEntryID = pa.AllocationEntryID,
                           AllocationStartDate = pa.AllocationStartDate,
                           Comments = pa.Remarks,
                           ProjectID = p.ProjectID,
                           ProjectManager = (from e in DataContext.Employees where e.EmployeeEntryID == p.ProjectManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                           ProjectManagerID = p.ProjectManagerID,
                           ProjectName = p.ProjectName,
                       }).ToList();
            return entries;
        }

        public List<BillabilityWiseAllocationDetailDto> GetBlockedEmpsForBenchEarmarked()
        {
            List<BillabilityWiseAllocationDetailDto> entries = new List<BillabilityWiseAllocationDetailDto>();

            entries = (from pa in Entities
                       join p in DataContext.Projects on pa.ProjectID equals p.ProjectID
                       join e in DataContext.Employees on pa.EmployeeID equals e.EmployeeEntryID
                       where pa.IsDeleted == false && pa.AllocationEndDate >= DateTime.Today && pa.AllocationTypeID == (int)AllocationType.Bench && pa.BenchCategoryID == (int)BenchCategory.Earmarked
                       select new BillabilityWiseAllocationDetailDto
                       {
                           AllocationTypeID = pa.AllocationTypeID,
                           AllocationType = "Bench (Earmarked)",
                           EmployeeEntryID = pa.EmployeeID,
                           EmployeeID = e.EmployeeID,
                           EmployeeName = e.FirstName + " " + e.LastName,
                           PrimarySkills = e.PrimarySkills,
                           SecondarySkills = e.SecondarySkills,
                           BusinessUnitID = e.BusinessUnitID,
                           BusinessUnit = DataContext.DropDownSubCategories.FirstOrDefault(bu => bu.SubCategoryID == e.BusinessUnitID).SubCategoryName,
                           AccountName = (from ac in DataContext.ProjectAccounts where ac.AccountID == p.ProjectAccountID select ac.AccountName).FirstOrDefault(),
                           AllocationEndDate = pa.AllocationEndDate,
                           AllocationEntryID = pa.AllocationEntryID,
                           AllocationStartDate = pa.AllocationStartDate,
                           Comments = pa.Remarks,
                           ProjectID = p.ProjectID,
                           ProjectManager = (from e in DataContext.Employees where e.EmployeeEntryID == p.ProjectManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                           ProjectManagerID = p.ProjectManagerID,
                           ProjectName = p.ProjectName,
                       }).ToList();
            return entries;
        }

        public IEnumerable<UtilizedDaysSummaryDto> GetUtilizedDaysSummary(string filterBy, string filterValue, string sortBy, string sortType)
        {
            return PrepareUtilizedDaysSummary(filterBy, filterValue, sortBy, sortType);
        }

        public bool AnyOtherActiveAllocation(int allocationID, int employeeID, DateTime allocationEndDate)
        {
            return Entities.Any(a => a.IsDeleted == false && a.AllocationEntryID != allocationID
                        && a.EmployeeID == employeeID && a.AllocationEndDate >= allocationEndDate);
        }

        public IEnumerable<PodWiseHeadCountDto> GetPodWiseAllocationCount()
        {
            List<PodWiseHeadCountDto> podWiseCountResult = new List<PodWiseHeadCountDto>();
            List<Practice> pods = DataContext.Practices.Where(p => p.IsDeleted == false).OrderBy(p => p.PracticeName).ToList();
            List<PodWiseCountDto> allPodWiseCount = postgresSqlProcessor.GetPodWiseAllocationCount().ToList();
            IQueryable<DropDownSubCategory> allocationTypes = DataContext.DropDownSubCategories.Where(s => s.CategoryID == 2 && s.IsDeleted == false);

            foreach (Practice pod in pods)
            {
                List<PodWiseCountDto> podCount = allPodWiseCount.Where(p => p.PracticeName == pod.PracticeName).ToList();
                int ba = (from a in Entities
                          join e in DataContext.Employees on a.EmployeeID equals e.EmployeeEntryID
                          where a.IsDeleted == false && e.IsDeleted == false
                          && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay > DateTime.Today))
                          && a.AllocationEndDate > DateTime.Today && a.BenchCategoryID == (int)BenchCategory.Available
                          select a).Count();

                int be = (from a in Entities
                          join e in DataContext.Employees on a.EmployeeID equals e.EmployeeEntryID
                          where a.IsDeleted == false && e.IsDeleted == false
                          && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay > DateTime.Today))
                          && a.AllocationEndDate > DateTime.Today && a.BenchCategoryID == (int)BenchCategory.Earmarked
                          select a).Count();

                podWiseCountResult.Add(new PodWiseHeadCountDto
                {
                    PracticeID = pod.PracticeID,
                    PracticeName = pod.PracticeName,
                    BenchCount = podCount.Any(a => a.SubCategoryName == "Bench") ? podCount.FirstOrDefault(a => a.SubCategoryName == "Bench").Count : 0,
                    BillableCount = podCount.Any(a => a.SubCategoryName == "Billable") ? podCount.FirstOrDefault(a => a.SubCategoryName == "Billable").Count : 0,
                    ComBufferCount = podCount.Any(a => a.SubCategoryName == "Committed Buffer") ? podCount.FirstOrDefault(a => a.SubCategoryName == "Committed Buffer").Count : 0,
                    NonComBufferCount = podCount.Any(a => a.SubCategoryName == "Non-Committed Buffer") ? podCount.FirstOrDefault(a => a.SubCategoryName == "Non-Committed Buffer").Count : 0,
                    TotalCount = DataContext.Employees.Count(e => e.IsDeleted == false && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay >= DateTime.Today))),
                    BenchAvailableCount = ba,
                    BenchEarmarkedCount = be,
                });
            }

            return podWiseCountResult;
        }

        public List<int> GetCommittedBufferUnderSpecificProjects()
        {
            List<int> results = new List<int>();
            int allocationType = (int)AllocationType.CommittedBuffer;
            int labProjectTypeID = (int)ProjectType.Lab;

            int mngtCount = (from e in DataContext.Employees
                             join a in Entities on e.EmployeeEntryID equals a.EmployeeID
                             join p in DataContext.Projects on a.ProjectID equals p.ProjectID
                             where a.AllocationTypeID == allocationType
                             && p.ProjectName.ToLower().Contains("management")
                             && e.IsDeleted == false && a.IsDeleted == false
                             && a.AllocationEndDate >= DateTime.Today
                             && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                             select a.EmployeeID).Count();
            results.Add(mngtCount);

            int labCount = (from e in DataContext.Employees
                            join a in Entities on e.EmployeeEntryID equals a.EmployeeID
                            join p in DataContext.Projects on a.ProjectID equals p.ProjectID
                            where a.AllocationTypeID == allocationType
                            && p.ProjectTypeID == labProjectTypeID
                            && e.IsDeleted == false && a.IsDeleted == false
                            && a.AllocationEndDate >= DateTime.Today
                            && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                            select a.EmployeeID).Count();
            results.Add(labCount);

            return results;
        }

        public IEnumerable<BillabilityWiseAllocationDetailDto> GetAllocationsForDates(DateTime fromDate, DateTime uptoDate)
        {
            return from p in Entities
                   join em in DataContext.Employees on p.EmployeeID equals em.EmployeeEntryID
                   join pr in DataContext.Projects on p.ProjectID equals pr.ProjectID
                   where p.IsDeleted == false && p.AllocationTypeID == (int)AllocationType.Billable &&
                   (p.AllocationStartDate <= fromDate && p.AllocationEndDate >= fromDate)
                   || ((p.AllocationStartDate <= uptoDate && p.AllocationEndDate >= uptoDate))
                   orderby em.EmployeeID
                   select new BillabilityWiseAllocationDetailDto
                   {
                       AllocationEntryID = p.AllocationEntryID,
                       AllocationEndDate = p.AllocationEndDate,
                       AllocationStartDate = p.AllocationStartDate,
                       AllocationType = (from e in DataContext.DropDownSubCategories where e.SubCategoryID == p.AllocationTypeID select e.SubCategoryName).FirstOrDefault(),
                       AllocationTypeID = p.AllocationTypeID,
                       EmployeeEntryID = p.EmployeeID,
                       EmployeeID = em.EmployeeID,
                       EmployeeName = em.FirstName + " " + em.LastName,
                       ProjectID = p.ProjectID,
                       ProjectManager = (from e in DataContext.Employees where e.EmployeeEntryID == pr.ProjectManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                       ProjectManagerID = pr.ProjectManagerID,
                       ProjectName = pr.ProjectName,
                       ReportingManager = (from e in DataContext.Employees where e.EmployeeEntryID == em.ReportingManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                   };
        }

        #endregion

        #region Private Methods

        private List<UtilizedDaysSummaryDto> PrepareUtilizedDaysSummary(string filterBy, string filterValue, string sortBy, string sortType)
        {
            List<UtilizedDaysSummaryDto> entries = new List<UtilizedDaysSummaryDto>();

            List<Employee> employees = (from e in DataContext.Employees
                                        where e.BusinessUnitID == 3 && e.IsDeleted == false && e.LastWorkingDay.HasValue == false
                                        && (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today)
                                        select e).ToList();

            foreach (Employee emp in employees)
            {
                UtilizedDaysSummaryDto entry = new UtilizedDaysSummaryDto
                {
                    EmployeeEntryID = emp.EmployeeEntryID,
                    DateOfJoin = emp.DateOfJoin,
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = emp.FirstName + " " + emp.LastName,
                    AgingDays = 0
                };

                if (Entities.Count(a => a.IsDeleted == false && a.EmployeeID == emp.EmployeeEntryID) == 0)
                {
                    entry.AgingDays = DateTime.Today.Subtract(emp.DateOfJoin).Days;
                    entry.AnyAllocation = "No Allocations";
                    entries.Add(entry);
                }
                else if (Entities.Count(a => a.IsDeleted == false && a.EmployeeID == emp.EmployeeEntryID && a.AllocationEndDate <= DateTime.Today) > 0)
                {
                    DateTime? allocationEndDate = Entities.Where(a => a.IsDeleted == false
                        && a.EmployeeID == emp.EmployeeEntryID
                        && a.AllocationEndDate < DateTime.Today).OrderByDescending(a => a.AllocationEndDate).Take(1).FirstOrDefault()?.AllocationEndDate;
                    entry.AnyAllocation = $"{Entities.Count(a => a.IsDeleted == false && a.EmployeeID == emp.EmployeeEntryID && a.AllocationEndDate < DateTime.Today)} Allocation(s)";

                    if (allocationEndDate.HasValue)
                    {
                        entry.LastAllocatedDate = allocationEndDate;
                        entry.AgingDays = DateTime.Today.Subtract(allocationEndDate.Value).Days;
                    }
                    entries.Add(entry);
                }
                else
                {
                    // ignore the employee as he has active allocation
                }
            }

            sortType = string.IsNullOrEmpty(sortType) ? "asc" : sortType.ToLower();
            switch (sortBy?.ToLower())
            {
                case "aged":
                    entries = sortType == "asc" ? entries.OrderBy(o => o.AgingDays).ToList() : entries.OrderByDescending(o => o.AgingDays).ToList();
                    break;
                default:
                    entries = sortType == "asc" ? entries.OrderBy(o => o.EmployeeName).ToList() : entries.OrderByDescending(o => o.EmployeeName).ToList();
                    break;
            }

            return entries;
        }

        private List<BillabilityWiseAllocationDetailDto> GetAllocationEntriesByAllocationType(int allocationType)
        {
            List<BillabilityWiseAllocationDetailDto> allocationDetailDtos = null;

            allocationDetailDtos = (from pa in Entities
                                    join pr in DataContext.Projects on pa.ProjectID equals pr.ProjectID
                                    join em in DataContext.Employees on pa.EmployeeID equals em.EmployeeEntryID
                                    where pa.AllocationEndDate >= DateTime.Today && pa.IsDeleted == false
                                    where pa.AllocationTypeID == allocationType
                                    select new BillabilityWiseAllocationDetailDto
                                    {
                                        AccountName = (from acc in DataContext.ProjectAccounts where acc.AccountID == pr.ProjectAccountID select acc.AccountName).FirstOrDefault(),
                                        AllocationEndDate = pa.AllocationEndDate,
                                        AllocationStartDate = pa.AllocationStartDate,
                                        AllocationEntryID = pa.AllocationEntryID,
                                        AllocationType = (from ds in DataContext.DropDownSubCategories where ds.SubCategoryID == pa.AllocationTypeID select ds.SubCategoryName).FirstOrDefault(),
                                        AllocationTypeID = pa.AllocationTypeID,
                                        BusinessUnit = (from ds in DataContext.DropDownSubCategories where ds.SubCategoryID == pr.BusinessUnitID select ds.SubCategoryName).FirstOrDefault(),
                                        BusinessUnitID = pr.BusinessUnitID,
                                        Comments = pa.Remarks,
                                        EmployeeEntryID = pa.EmployeeID,
                                        EmployeeID = em.EmployeeID,
                                        EmployeeName = em.FirstName + " " + em.LastName,
                                        PrimarySkills = em.PrimarySkills,
                                        SecondarySkills = em.SecondarySkills,
                                        ProjectID = pa.ProjectID,
                                        ProjectManager = (from e in DataContext.Employees where e.EmployeeEntryID == pr.ProjectManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                                        ProjectManagerID = pr.ProjectManagerID,
                                        ProjectName = pr.ProjectName,
                                        ProjectType = (from ds in DataContext.DropDownSubCategories where ds.SubCategoryID == pr.ProjectTypeID select ds.SubCategoryName).FirstOrDefault(),
                                        ProjectTypeID = pr.ProjectTypeID,
                                        ReportingManager = (from e in DataContext.Employees where e.EmployeeEntryID == em.ReportingManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                                        ReportingManagerID = em.ReportingManagerID,
                                    }).ToList();
            //DbCommand cmd = DataContext.Database.Connection.CreateCommand();
            //cmd.CommandText = "dbo.GetBillabilityWiseDetails";
            //SqlParameter param = new SqlParameter
            //{
            //    ParameterName = "AllocationType",
            //    Value = allocationType
            //};
            //cmd.Parameters.Add(param);
            //cmd.CommandType = CommandType.StoredProcedure;
            //DataContext.Database.Connection.Open();
            //DbDataReader reader = cmd.ExecuteReader();
            //ObjectResult<BillabilityWiseAllocationDetailDto> items = ((IObjectContextAdapter)DataContext).ObjectContext.Translate<BillabilityWiseAllocationDetailDto>(reader);
            //allocationDetailDtos = items.ToList();
            //DataContext.Database.Connection.Close();
            return allocationDetailDtos;
        }

        private ProjectAllocation CreateBusinessEntity(ProjectAllocationDto projectDto, bool isNewEntity = false)
        {
            ProjectAllocation entity = new ProjectAllocation
            {
                AllocationEndDate = new DateTime(projectDto.AllocationEndDate.Year, projectDto.AllocationEndDate.Month, projectDto.AllocationEndDate.Day),
                AllocationStartDate = new DateTime(projectDto.AllocationStartDate.Year, projectDto.AllocationStartDate.Month, projectDto.AllocationStartDate.Day),
                AllocationTypeID = projectDto.AllocationTypeID,
                EmployeeID = projectDto.EmployeeID,
                ProjectID = projectDto.ProjectID,
                PercentageOfAllocation = projectDto.PercentageOfAllocation,
                AllocationEntryID = projectDto.AllocationEntryID,
                IsActive = projectDto.AllocationEndDate > DateTime.Today,
                Remarks = projectDto.Remarks,
                BenchCategoryID = projectDto.BenchCategoryID,
            };

            entity.UpdateTimeStamp(projectDto.LoggedInUserName, isNewEntity: true);
            return entity;
        }

        private void MigrateEntity(ProjectAllocationDto sourceEntity, ProjectAllocation targetEntity)
        {
            targetEntity.AllocationEndDate = new DateTime(sourceEntity.AllocationEndDate.Year, sourceEntity.AllocationEndDate.Month, sourceEntity.AllocationEndDate.Day);
            targetEntity.AllocationStartDate = new DateTime(sourceEntity.AllocationStartDate.Year, sourceEntity.AllocationStartDate.Month, sourceEntity.AllocationStartDate.Day);
            targetEntity.AllocationTypeID = sourceEntity.AllocationTypeID;
            targetEntity.EmployeeID = sourceEntity.EmployeeID;
            targetEntity.ProjectID = sourceEntity.ProjectID;
            targetEntity.PercentageOfAllocation = sourceEntity.PercentageOfAllocation;
            targetEntity.Remarks = sourceEntity.Remarks;
            targetEntity.BenchCategoryID = sourceEntity.BenchCategoryID;

            targetEntity.UpdateTimeStamp(sourceEntity.LoggedInUserName);
        }

        private int GetEmployeesCountByAllocationType(AllocationType allocationType, BenchCategory benchCategory = BenchCategory.Available)
        {
            if (allocationType == AllocationType.Bench)
            {
                return Entities.Count(a => a.IsDeleted == false && a.AllocationTypeID == (int)allocationType && a.BenchCategoryID == (int)benchCategory
                && a.AllocationEndDate >= DateTime.Today && a.AllocationStartDate <= DateTime.Today);
            }
            else
            {
                return Entities.Count(a => a.AllocationTypeID == (int)allocationType && a.IsDeleted == false
                && a.AllocationEndDate >= DateTime.Today && a.AllocationStartDate <= DateTime.Today);
            }
        }

        private List<BillabilityWiseAllocationDetailDto> GetAllAllocationDetailFilteredByProjectData(string filterBy, string filterValue)
        {
            List<BillabilityWiseAllocationDetailDto> allocationDetails = new List<BillabilityWiseAllocationDetailDto>();
            List<Project> projects = null;
            string filterByText = filterBy?.ToLower();
            int.TryParse(filterValue, out int filterValueID);

            if (filterByText == "prj")
            {
                projects = DataContext.Projects.Where(p => p.IsDeleted == false && p.ProjectID == filterValueID).ToList();
            }
            else if (filterByText == "acc")
            {
                projects = DataContext.Projects.Where(p => p.IsDeleted == false && p.ProjectAccountID == filterValueID).ToList();
            }

            if (projects.Count == 0)
            {
                return allocationDetails;
            }

            foreach (Project project in projects)
            {
                List<ProjectAllocation> allocations = DataContext.ProjectAllocations.Where(a => a.IsDeleted == false
                    && a.AllocationEndDate >= DateTime.Today && a.ProjectID == project.ProjectID).ToList();
                foreach (ProjectAllocation allocation in allocations)
                {
                    Employee emp = DataContext.Employees.FirstOrDefault(e => e.EmployeeEntryID == allocation.EmployeeID);
                    allocationDetails.Add(new BillabilityWiseAllocationDetailDto
                    {
                        AllocationTypeID = allocation.AllocationTypeID,
                        AllocationType = DataContext.DropDownSubCategories.FirstOrDefault(ds => ds.SubCategoryID == allocation.AllocationTypeID)?.SubCategoryName,
                        EmployeeEntryID = emp.EmployeeEntryID,
                        EmployeeID = emp.EmployeeID,
                        EmployeeName = emp.FirstName + " " + emp.LastName,
                        PrimarySkills = emp.PrimarySkills,
                        SecondarySkills = emp.SecondarySkills,
                        AllocationEndDate = allocation.AllocationEndDate,
                        AllocationStartDate = allocation.AllocationStartDate,
                        BusinessUnitID = project.BusinessUnitID,
                        BusinessUnit = DataContext.DropDownSubCategories.FirstOrDefault(bu => bu.SubCategoryID == project.BusinessUnitID)?.SubCategoryName,
                        ProjectID = project.ProjectID,
                        ProjectManager = (from e in DataContext.Employees where e.EmployeeEntryID == project.ProjectManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                        ProjectManagerID = project.ProjectManagerID,
                        ProjectName = project.ProjectName,
                        ProjectTypeID = project.ProjectTypeID,
                        ProjectType = DataContext.DropDownSubCategories.FirstOrDefault(bu => bu.SubCategoryID == project.ProjectTypeID)?.SubCategoryName,
                    });
                }
            }

            return allocationDetails;
        }

        private List<BillabilityWiseAllocationDetailDto> GetAllAllocationDetailFilteredByEmployeeData(string filterBy, string filterValue)
        {
            List<BillabilityWiseAllocationDetailDto> allocationDetails = new List<BillabilityWiseAllocationDetailDto>();
            List<Employee> employees = null;
            string filterByText = filterBy?.ToLower();

            switch (filterByText)
            {
                case "all":
                    employees = DataContext.Employees.Where(e => e.IsDeleted == false
                        && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))).ToList();
                    break;
                case "emp":
                    int.TryParse(filterValue, out int empID);
                    employees = DataContext.Employees.Where(e => e.IsDeleted == false
                        && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                        && e.EmployeeEntryID == empID).ToList();
                    break;
                case "psk":
                    employees = DataContext.Employees.Where(e => e.IsDeleted == false
                        && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                        && e.PrimarySkills.ToLower().Contains(filterValue.ToLower())).ToList();
                    break;
                case "ssk":
                    employees = DataContext.Employees.Where(e => e.IsDeleted == false
                        && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay.Value >= DateTime.Today))
                        && e.SecondarySkills.ToLower().Contains(filterValue.ToLower())).ToList();
                    break;
            }

            if (employees.Count() == 0)
            {
                return allocationDetails;
            }

            foreach (Employee emp in employees)
            {
                List<ProjectAllocation> allocations = DataContext.ProjectAllocations.Where(a => a.IsDeleted == false
                    && a.AllocationEndDate >= DateTime.Today && a.EmployeeID == emp.EmployeeEntryID).ToList();

                // if no active allocations found, creating an allocation entry with allocation type as not allocated
                if (allocations.Count() == 0)
                {
                    allocationDetails.Add(new BillabilityWiseAllocationDetailDto
                    {
                        AllocationTypeID = 6,
                        AllocationType = "Not Allocated Yet", // non-comitted buffer
                        Comments = "No allocations found",
                        EmployeeEntryID = emp.EmployeeEntryID,
                        EmployeeID = emp.EmployeeID,
                        EmployeeName = emp.FirstName + " " + emp.LastName,
                        PrimarySkills = emp.PrimarySkills,
                        SecondarySkills = emp.SecondarySkills,
                        ProjectManagerID = emp.ReportingManagerID,
                        ProjectManager = (from e in DataContext.Employees where e.EmployeeEntryID == emp.ReportingManagerID select e.FirstName + " " + e.LastName).FirstOrDefault(),
                        BusinessUnitID = emp.BusinessUnitID,
                        BusinessUnit = DataContext.DropDownSubCategories.FirstOrDefault(bu => bu.SubCategoryID == emp.BusinessUnitID)?.SubCategoryName
                    });
                }
                else
                {
                    foreach (ProjectAllocation allocation in allocations)
                    {
                        BillabilityWiseAllocationDetailDto allocationDetail = new BillabilityWiseAllocationDetailDto
                        {
                            AllocationTypeID = allocation.AllocationTypeID,
                            AllocationType = DataContext.DropDownSubCategories.FirstOrDefault(ds => ds.SubCategoryID == allocation.AllocationTypeID)?.SubCategoryName,
                            EmployeeEntryID = emp.EmployeeEntryID,
                            EmployeeID = emp.EmployeeID,
                            EmployeeName = emp.FirstName + " " + emp.LastName,
                            PrimarySkills = emp.PrimarySkills,
                            SecondarySkills = emp.SecondarySkills,
                            AllocationEndDate = allocation.AllocationEndDate,
                            AllocationStartDate = allocation.AllocationStartDate,
                            BusinessUnitID = emp.BusinessUnitID,
                            BusinessUnit = DataContext.DropDownSubCategories.FirstOrDefault(bu => bu.SubCategoryID == emp.BusinessUnitID)?.SubCategoryName,
                        };

                        Project prj = DataContext.Projects.FirstOrDefault(p => p.ProjectID == allocation.ProjectID);
                        if (prj == null)
                        {
                            allocationDetail.Comments = "Project is missing";
                        }
                        else
                        {
                            allocationDetail.ProjectID = prj.ProjectID;
                            allocationDetail.ProjectManager = (from e in DataContext.Employees where e.EmployeeEntryID == prj.ProjectManagerID select e.FirstName + " " + e.LastName)?.FirstOrDefault();
                            allocationDetail.ProjectManagerID = prj.ProjectManagerID;
                            allocationDetail.ProjectName = prj.ProjectName;
                            allocationDetail.ProjectTypeID = prj.ProjectTypeID;
                            allocationDetail.ProjectType = DataContext.DropDownSubCategories.FirstOrDefault(bu => bu.SubCategoryID == prj.ProjectTypeID)?.SubCategoryName;
                            allocationDetail.AccountName = DataContext.ProjectAccounts.FirstOrDefault(a => a.AccountID == prj.ProjectAccountID)?.AccountName;
                        }

                        allocationDetails.Add(allocationDetail);
                    }
                }

            }
            return allocationDetails;
        }

        private int GetNonAllocatedResourcesCount(bool forDeliveryBU)
        {
            int count = 0;
            List<int> emps = new List<int>();
            if (forDeliveryBU)
            {
                emps = DataContext.Employees.Where(e => e.IsDeleted == false
                  && e.BusinessUnitID == 3 && (e.LastWorkingDay.HasValue == false
                  || (e.LastWorkingDay.HasValue && e.LastWorkingDay.Value >= DateTime.Today))).Select(e => e.EmployeeEntryID).ToList();
            }
            else
            {
                emps = DataContext.Employees.Where(e => e.IsDeleted == false
                    && (e.BusinessUnitID == (int)BusinessUnit.BusinessOperations || e.BusinessUnitID == (int)BusinessUnit.BusinessDevelopment)
                    && (e.LastWorkingDay.HasValue == false
                    || (e.LastWorkingDay.HasValue && e.LastWorkingDay.Value >= DateTime.Today))).Select(e => e.EmployeeEntryID).ToList();

            }
            //StringBuilder ids = new StringBuilder();
            foreach (int emp in emps)
            {
                if (!Entities.Any(a => a.IsDeleted == false && a.EmployeeID == emp && a.AllocationEndDate >= DateTime.Today))
                {
                    count++;
                }
                //ids.Append($"{emp},");
            }
            return count;
        }

        #endregion
    }

    public interface IAllocationRepository : IRepository<ProjectAllocationDto>
    {
        int Exists(int empEntryID, int projectID);

        int Exists(int allocationID, int empEntryID, int projectID);

        int GetPercentageOfAllocation(int employeeID);

        IEnumerable<CustomAllocationDto> GetAllocatedProjectsByEmployeeID(int employeeID);

        IEnumerable<ProjectAllocationDto> GetAllAllocationsByProjectID(int projectID);

        int GetTotalCountForAllocationHistory(string filterType, int filterValue);

        IEnumerable<ProjectAllocationDto> GetAllocationHistory(string filterType, int filterValue, string sortBy, string sortType, int pageSize = -1, int pageNo = -1);

        IEnumerable<ProjectAllocationDto> GetAll(string filterType, int filterValueID, string sortBy, string sortType, int pageSize = -1, int pageNo = -1);

        int TotalRecordsCount(string filterType, int filterValueID);

        bool AnyActiveBillableAllocations(int employeeID, int allocationID, DateTime startDate);

        bool AnyActiveAllocationInBenchProject(int employeeID, DateTime startDate);

        void EndAllocation(int allocationID);

        IEnumerable<ManagerWiseAllocationDto> GetManagerWiseAllocationSummary();

        IEnumerable<BillabilityWiseAllocationSummaryDto> GetBillabilityWiseAllocationSummary();

        IEnumerable<BillabilityWiseAllocationDetailDto> GetBillabilityWiseAllocationDetail(string filterBy, string filterValue);

        IEnumerable<PodWiseHeadCountDto> GetPodWiseAllocationCount();

        IEnumerable<UtilizedDaysSummaryDto> GetUtilizedDaysSummary(string filterBy, string filterValue, string sortBy, string sortType);

        bool AnyOtherActiveAllocation(int allocationID, int employeeID, DateTime allocationEndDate);

        List<int> GetCommittedBufferUnderSpecificProjects();

        IEnumerable<BillabilityWiseAllocationDetailDto> GetAllocationsForDates(DateTime from, DateTime upto);

        IEnumerable<EmpArchitectDto> GetAllArchitectEmployees();
    }
}

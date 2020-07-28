using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class EmployeeTechRepository : RepositoryBase<EmpAssetDetail>, IEmployeeTechRepository
    {
        public List<TechSkillCategoryDto> GetAllAvailableSkillCategories()
        {
            return (from c in DataContext.TechSkillCategories
                    where c.IsDeleted == false
                    select new TechSkillCategoryDto
                    {
                        CategoryID = c.CategoryID,
                        CategoryName = c.CategoryName,
                    }).ToList();
        }

        public void Add(EmpAssetDetailDto entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(EmpAssetDetailDto entity)
        {
            throw new NotImplementedException();
            //EmpAssetDetail buzEntity = Entities.FirstOrDefault(e => e.EmployeeEntryID == entity.EmployeeEntryID);
            //buzEntity.IsDeleted = true;
            //buzEntity.UpdateTimeStamp(entity.LoggedInUserName);
            //Entities.Add(buzEntity);
            //DataContext.Entry(buzEntity).State = EntityState.Modified;
            //DataContext.SaveChanges();
        }

        public bool Exists(string itemName)
        {
            return Entities.Any(e => e.EmployeeID == itemName);
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmpAssetDetailDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            var assets = (from a in Entities
                          join e in DataContext.Employees on a.EmployeeID equals e.EmployeeID
                          where a.IsDeleted == false
                          orderby e.EmployeeID
                          select new EmpAssetDetailDto
                          {
                              Designation = a.Designation,
                              EmpAssetDetailID = a.EmpAssetDetailID,
                              EmployeeID = a.EmployeeID,
                              EmployeeName = e.FirstName + " " + e.LastName,
                              LocationID = a.LocationID,
                              OverallExperience = a.OverallExperience,
                              PrimarySkills = a.PrimarySkills,
                              SecondarySkills = a.SecondarySkills,
                              VisaStatusID = a.VisaStatusID,
                              EmployeeEntryID = e.EmployeeEntryID,
                              PastExperience = a.PastExperience,
                          });

            if (pageSize <= 0 || pageNo < 1)
            {
                return assets;
            }

            return assets.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<EmpSkillSummaryDto> GetAllSkillSummary(string findBy, int pageSize = -1, int pageNo = -1)
        {
            var employees = (from ts in DataContext.Employees where ts.IsDeleted == false orderby ts.EmployeeID select ts);
            List<Employee> selectedEmps = null;
            if (pageSize <= 0 || pageNo < 1)
            {
                selectedEmps = employees.ToList();
            }
            else
            {
                selectedEmps = employees.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }

            List<EmployeeSkill> employeeSkills = DataContext.EmployeeSkills.Where(r => r.IsDeleted == false).ToList();
            List<TechSkill> techSkills = DataContext.TechSkills.Where(s => s.IsDeleted == false).ToList();

            List<EmpSkillSummaryDto> empSkills = new List<EmpSkillSummaryDto>();

            foreach (var emp in selectedEmps)
            {
                EmpSkillSummaryDto empSkill = new EmpSkillSummaryDto
                {
                    EmployeeEntryID = emp.EmployeeEntryID,
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = emp.FirstName + " " + emp.LastName,
                };

                var advancedSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Advanced).ToList();
                var basicSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Basic).ToList();
                var limitedSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Limited).ToList();
                var profSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Proficient).ToList();

                StringBuilder aSkillText = new StringBuilder();
                foreach (var askill in advancedSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == askill.TechSkillID);
                    if (sk != null)
                    {
                        aSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int aLI = aSkillText.ToString().LastIndexOf(", ");

                if (String.IsNullOrWhiteSpace(aSkillText.ToString()))
                    empSkill.AdvanceSkills = "None";
                else
                    empSkill.AdvanceSkills = aSkillText.ToString().Substring(0, aLI);

                StringBuilder bSkillText = new StringBuilder();
                foreach (var bskill in basicSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == bskill.TechSkillID);
                    if (sk != null)
                    {
                        bSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int bLI = bSkillText.ToString().LastIndexOf(", ");
                if (String.IsNullOrWhiteSpace(bSkillText.ToString()))
                    empSkill.BasicSkills = "None";
                else
                    empSkill.BasicSkills = bSkillText.ToString().Substring(0, bLI);

                StringBuilder lSkillText = new StringBuilder();
                foreach (var lskill in limitedSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == lskill.TechSkillID);
                    if (sk != null)
                    {
                        lSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int lLI = lSkillText.ToString().LastIndexOf(", ");
                if (String.IsNullOrWhiteSpace(lSkillText.ToString()))
                    empSkill.LimitedSkills = "None";
                else
                    empSkill.LimitedSkills = lSkillText.ToString().Substring(0, lLI);

                StringBuilder pSkillText = new StringBuilder();
                foreach (var pskill in profSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == pskill.TechSkillID);
                    if (sk != null)
                    {
                        pSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int pLI = pSkillText.ToString().LastIndexOf(", ");
                if (String.IsNullOrWhiteSpace(pSkillText.ToString()))
                    empSkill.ProfSkills = "None";
                else
                    empSkill.ProfSkills = pSkillText.ToString().Substring(0, pLI);

                empSkills.Add(empSkill);
            }

            return empSkills;
        }

        public EmpAssetDetailDto GetByID(int id)
        {
            EmpAssetDetailDto emp = null;
            if (id == 0)
            {
                emp = new EmpAssetDetailDto();
            }
            else
            {
                emp = (from a in Entities
                       join e in DataContext.Employees on a.EmployeeID equals e.EmployeeID
                       where a.EmpAssetDetailID == id && a.IsDeleted == false
                       select new EmpAssetDetailDto
                       {
                           Designation = a.Designation,
                           EmpAssetDetailID = a.EmpAssetDetailID,
                           EmployeeID = a.EmployeeID,
                           EmployeeName = e.FirstName + " " + e.LastName,
                           LocationID = a.LocationID,
                           OverallExperience = a.OverallExperience,
                           PrimarySkills = a.PrimarySkills,
                           SecondarySkills = a.SecondarySkills,
                           VisaStatusID = a.VisaStatusID,
                           EmployeeEntryID = e.EmployeeEntryID,
                           PastExperience = a.PastExperience,

                       }).FirstOrDefault();
            }

            return emp;
        }

        public EmpAssetDetailDto GetByEmployeeID(string eid)
        {
            if (Entities.Any(e => e.EmployeeID == eid))
            {
                return (from a in Entities
                        join e in DataContext.Employees on a.EmployeeID equals e.EmployeeID
                        join r in DataContext.Employees on e.ReportingManagerID equals r.EmployeeEntryID into re
                        from rd in re.DefaultIfEmpty()
                        where a.EmployeeID == eid && a.IsDeleted == false
                        select new EmpAssetDetailDto
                        {
                            Designation = a.Designation,
                            EmpAssetDetailID = a.EmpAssetDetailID,
                            EmployeeID = a.EmployeeID,
                            EmployeeName = e.FirstName + " " + e.LastName,
                            LocationID = a.LocationID,
                            OverallExperience = a.OverallExperience,
                            PastExperience = a.PastExperience,
                            PrimarySkills = e.PrimarySkills,
                            SecondarySkills = e.SecondarySkills,
                            VisaStatusID = a.VisaStatusID,
                            EmployeeEntryID = e.EmployeeEntryID,
                            EmailID = e.EmailID,
                            ReportingTo = rd.FirstName + " " + rd.LastName,
                        }).FirstOrDefault();
            }
            else
            {
                Employee emp = DataContext.Employees.FirstOrDefault(e => e.EmployeeID == eid && e.IsDeleted == false);
                if (emp == null)
                {
                    throw new InvalidOperationException("Employee data does not exist. Please contact Satish Srinivasan.");
                }
                Employee rm = DataContext.Employees.FirstOrDefault(e => e.EmployeeEntryID == emp.ReportingManagerID && e.IsDeleted == false);
                string reportingManager = "";
                if (rm != null)
                {
                    reportingManager = rm.FirstName + " " + rm.LastName;
                }

                return new EmpAssetDetailDto
                {
                    EmployeeEntryID = emp.EmployeeEntryID,
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = emp.FirstName + " " + emp.LastName,
                    EmailID = emp.EmailID,
                    ReportingTo = reportingManager,
                    PrimarySkills = emp.PrimarySkills,
                    SecondarySkills = emp.SecondarySkills,
                };
            }
        }

        public EmpAssetDetailDto GetByEmployeeEntryID(int empEntryID)
        {
            if (Entities.Any(e => e.EmployeeEntryID == empEntryID))
            {
                return (from a in Entities
                        join e in DataContext.Employees on a.EmployeeID equals e.EmployeeID
                        join r in DataContext.Employees on e.ReportingManagerID equals r.EmployeeEntryID into re
                        from rd in re.DefaultIfEmpty()
                        where a.EmployeeEntryID == empEntryID && a.IsDeleted == false
                        select new EmpAssetDetailDto
                        {
                            Designation = a.Designation,
                            EmpAssetDetailID = a.EmpAssetDetailID,
                            EmployeeID = a.EmployeeID,
                            EmployeeName = e.FirstName + " " + e.LastName,
                            LocationID = a.LocationID,
                            OverallExperience = a.OverallExperience,
                            PastExperience = a.PastExperience,
                            PrimarySkills = e.PrimarySkills,
                            SecondarySkills = e.SecondarySkills,
                            VisaStatusID = a.VisaStatusID,
                            EmployeeEntryID = e.EmployeeEntryID,
                            EmailID = e.EmailID,
                            ReportingTo = rd.FirstName + " " + rd.LastName,
                        }).FirstOrDefault();
            }
            else
            {

                Employee emp = DataContext.Employees.FirstOrDefault(e => e.EmployeeEntryID == empEntryID && e.IsDeleted == false);
                if (emp == null)
                {
                    throw new InvalidOperationException("Employee data does not exist. Please contact Satish Srinivasan.");
                }

                Employee rm = DataContext.Employees.FirstOrDefault(e => e.EmployeeEntryID == emp.ReportingManagerID && e.IsDeleted == false);
                string reportingManager = "";
                if (rm != null)
                {
                    reportingManager = rm.FirstName + " " + rm.LastName;
                }

                return new EmpAssetDetailDto
                {
                    EmployeeEntryID = emp.EmployeeEntryID,
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = emp.FirstName + " " + emp.LastName,
                    EmailID = emp.EmailID,
                    ReportingTo = reportingManager,
                    PrimarySkills = emp.PrimarySkills,
                    SecondarySkills = emp.SecondarySkills,
                };
            }
        }

        public void Update(EmpAssetDetailDto entity)
        {
            throw new NotImplementedException();
        }

        public List<TechSkillDto> GetAllAvailableTechSkills()
        {
            return (from skil in DataContext.TechSkills
                    where skil.IsDeleted == false
                    select new TechSkillDto
                    {
                        TechSkillCategoryID = skil.TechSkillCategoryID,
                        TechSkillID = skil.TechSkillID,
                        TechSkillName = skil.TechSkillName
                    }).ToList();
        }

        public TechSkillDto GetTechSkillByID(int skillID)
        {
            return (from skill in DataContext.TechSkills
                    join c in DataContext.TechSkillCategories on skill.TechSkillCategoryID equals c.CategoryID into ce
                    from cd in ce.DefaultIfEmpty()
                    where skill.IsDeleted == false && skill.TechSkillID == skillID
                    select new TechSkillDto
                    {
                        TechSkillCategoryID = skill.TechSkillCategoryID,
                        TechSkillID = skill.TechSkillID,
                        TechSkillName = skill.TechSkillName,
                        SkillCategoryName = cd.CategoryName
                    }).FirstOrDefault();
        }

        public List<TechSkillDto> GetAllAvailableTechSkillsByCategory(int categoryID)
        {
            return (from skil in DataContext.TechSkills
                    where skil.IsDeleted == false && skil.TechSkillCategoryID == categoryID
                    select new TechSkillDto
                    {
                        TechSkillCategoryID = skil.TechSkillCategoryID,
                        TechSkillID = skil.TechSkillID,
                        TechSkillName = skil.TechSkillName
                    }).ToList();
        }

        public bool DoesEmployeeSkillExist(int id)
        {
            return DataContext.EmployeeSkills.Any(e => e.EmployeeSkillID == id && e.IsDeleted == false);
        }

        public List<EmployeeSkillDto> GetAllEmployeeSkills(int employeeID)
        {
            List<EmployeeSkillDto> skills = (from t in DataContext.EmployeeSkills
                                             join sc in DataContext.DropDownSubCategories on t.RatingID equals sc.SubCategoryID into sce
                                             from scd in sce.DefaultIfEmpty()
                                             join ts in DataContext.TechSkills on t.TechSkillID equals ts.TechSkillID into tse
                                             from tsd in tse.DefaultIfEmpty()
                                             where t.IsDeleted == false && t.EmployeeEntryID == employeeID
                                             select new EmployeeSkillDto
                                             {
                                                 EmployeeEntryID = t.EmployeeEntryID,
                                                 EmployeeSkillID = t.EmployeeSkillID,
                                                 RatingID = t.RatingID,
                                                 SkillCategoryID = t.SkillCategoryID,
                                                 TechSkillID = t.TechSkillID,
                                                 Rating = scd.SubCategoryName,
                                                 TechSkill = tsd.TechSkillName,
                                             }).ToList();

            return skills;
        }

        public int UpdateEmployeeDetails(EmpAssetDetailDto empDetails)
        {
            EmpAssetDetail assetEntity = new EmpAssetDetail();
            empDetails.PrimarySkills = empDetails.PrimarySkills?.Replace(",", ";");
            empDetails.SecondarySkills = empDetails.SecondarySkills?.Replace(",", ";");
            Employee emp = DataContext.Employees.FirstOrDefault(e => e.EmployeeID == empDetails.EmployeeID && e.IsDeleted == false);
            if (emp == null)
            {
                throw new InvalidOperationException("We couldn't find the underlying employee data");
            }

            if (Entities.Any(e => e.EmployeeEntryID == empDetails.EmployeeEntryID && e.IsDeleted == false))
            {
                assetEntity = Entities.FirstOrDefault(e => e.EmployeeEntryID == empDetails.EmployeeEntryID && e.IsDeleted == false);
                MigrateEntity(empDetails, assetEntity);
                Entities.Add(assetEntity);
                DataContext.Entry(assetEntity).State = EntityState.Modified;
            }
            else
            {
                assetEntity = CreateBusinessEntity(empDetails);
                assetEntity.EmployeeEntryID = emp.EmployeeEntryID;
                Entities.Add(assetEntity);
                DataContext.Entry(assetEntity).State = EntityState.Added;
            }

            emp.EmailID = empDetails.EmailID;
            emp.PrimarySkills = empDetails.PrimarySkills;
            emp.SecondarySkills = empDetails.SecondarySkills;
            emp.PastExperience = empDetails.PastExperience;
            DataContext.Employees.Add(emp);
            DataContext.Entry(emp).State = EntityState.Modified;

            DataContext.SaveChanges();

            return Entities.FirstOrDefault(e => e.EmployeeID == empDetails.EmployeeID && e.IsDeleted == false).EmpAssetDetailID;
        }

        public EmployeeSkillDto GetEmployeeSkillByID(int id)
        {
            return (from skill in DataContext.EmployeeSkills
                    join ts in DataContext.TechSkills on skill.TechSkillID equals ts.TechSkillID into tse
                    from tsd in tse.DefaultIfEmpty()
                    where skill.EmployeeSkillID == id && skill.IsDeleted == false
                    select new EmployeeSkillDto
                    {
                        EmployeeEntryID = skill.EmployeeEntryID,
                        EmployeeSkillID = skill.EmployeeSkillID,
                        RatingID = skill.RatingID,
                        SkillCategoryID = skill.SkillCategoryID,
                        TechSkillID = skill.TechSkillID,
                        TechSkill = tsd.TechSkillName,
                    }).FirstOrDefault();
        }

        public void AddEmpSkill(EmployeeSkillDto skill)
        {
            if (skill.EmployeeEntryID <= 0)
            {
                if (string.IsNullOrWhiteSpace(skill.EmployeeID))
                {
                    throw new InvalidOperationException("Invalid employee record. Please start from the Assets home page");
                }
                else
                {
                    skill.EmployeeEntryID = DataContext.Employees.FirstOrDefault(e => e.IsDeleted == false && e.EmployeeID == skill.EmployeeID).EmployeeEntryID;
                }
            }

            EmployeeSkill entity = new EmployeeSkill
            {
                EmployeeEntryID = skill.EmployeeEntryID,
                RatingID = skill.RatingID,
                TechSkillID = skill.TechSkillID,
                SkillCategoryID = skill.SkillCategoryID
            };
            DataContext.EmployeeSkills.Add(entity);
            DataContext.Entry(entity).State = EntityState.Added;
            DataContext.SaveChanges();
            DataContext.SaveChangesAsync();
        }

        public async Task AddEmpSkillAsync(EmployeeSkillDto skill)
        {
            if (skill.EmployeeEntryID <= 0)
            {
                if (string.IsNullOrWhiteSpace(skill.EmployeeID))
                {
                    throw new InvalidOperationException("Invalid employee record. Please start from the Assets home page");
                }
                else
                {
                    Task<Employee> obj = DataContext.Employees.FirstOrDefaultAsync(e => e.IsDeleted == false && e.EmployeeID == skill.EmployeeID);
                    skill.EmployeeEntryID = obj.Result.EmployeeEntryID;
                }
            }

            EmployeeSkill entity = new EmployeeSkill
            {
                EmployeeEntryID = skill.EmployeeEntryID,
                RatingID = skill.RatingID,
                TechSkillID = skill.TechSkillID,
                SkillCategoryID = skill.SkillCategoryID
            };
            DataContext.EmployeeSkills.Add(entity);
            DataContext.Entry(entity).State = EntityState.Added;
            await DataContext.SaveChangesAsync();
        }

        public void UpdateEmpSkill(EmployeeSkillDto skill)
        {
            if (skill.EmployeeEntryID <= 0)
            {
                throw new InvalidOperationException("Invalid employee record. Please start from the Assets home page");
            }

            EmployeeSkill empSkillEntity = DataContext.EmployeeSkills.FirstOrDefault(e => e.EmployeeSkillID == skill.EmployeeSkillID && e.IsDeleted == false);
            if (empSkillEntity != null)
            {
                empSkillEntity.EmployeeEntryID = skill.EmployeeEntryID;
                empSkillEntity.RatingID = skill.RatingID;
                empSkillEntity.SkillCategoryID = skill.SkillCategoryID;
                empSkillEntity.TechSkillID = skill.TechSkillID;
                empSkillEntity.UpdateTimeStamp(skill.LoggedInUserName);

                DataContext.EmployeeSkills.Add(empSkillEntity);
                DataContext.Entry(empSkillEntity).State = EntityState.Modified;
                DataContext.SaveChanges();
            }
        }

        public void DeleteEmpTechSkill(EmployeeSkillDto employeeSkillDto)
        {
            EmployeeSkill buzEntity = DataContext.EmployeeSkills.FirstOrDefault(e => e.EmployeeSkillID == employeeSkillDto.EmployeeSkillID);
            buzEntity.IsDeleted = true;
            buzEntity.UpdateTimeStamp(employeeSkillDto.LoggedInUserName);
            DataContext.EmployeeSkills.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public int TotalRecordsCount(string findBy)
        {
            if (string.IsNullOrWhiteSpace(findBy))
                return TotalRecordsCount();
            else
                return (from ts in DataContext.TechSkills
                        join es in DataContext.EmployeeSkills on ts.TechSkillID equals es.TechSkillID
                        join e in DataContext.Employees on es.EmployeeEntryID equals e.EmployeeEntryID
                        where ts.IsDeleted == false && es.IsDeleted == false && e.IsDeleted == false && e.EmployeeEntryID > 0
                        select es.EmployeeEntryID).Count();
        }

        public int GetEmployeeSkillsReportCount(string filterBy, string filterValue, string filterText)
        {
            int recCount = 0;
            int.TryParse(filterValue, out int filterVal);

            if (string.IsNullOrWhiteSpace(filterBy))
            {
                filterBy = "emp";
                filterValue = "alle";
            }

            switch (filterBy)
            {
                case "emp":
                    recCount = 1;
                    if (string.IsNullOrWhiteSpace(filterValue) || filterVal == 0)
                    {
                        recCount = (from e in DataContext.Employees
                                    where e.IsDeleted == false
                                    && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                    select e).Count();
                    }
                    break;
                case "prj":
                    recCount = (from e in DataContext.Employees
                                where e.IsDeleted == false
                                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                && (from p in DataContext.ProjectAllocations
                                    where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today && p.ProjectID == filterVal
                                    select p.EmployeeID).Contains(e.EmployeeEntryID)
                                select e).Count();
                    break;
                case "acc":
                    recCount = (from e in DataContext.Employees
                                where e.IsDeleted == false
                                 && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                 && (from al in DataContext.ProjectAllocations
                                     where al.IsDeleted == false && al.AllocationEndDate >= DateTime.Today
                                     && (from p in DataContext.Projects
                                         where p.IsDeleted == false && (from a in DataContext.ProjectAccounts
                                                                        where a.IsDeleted == false
                                                                      && a.AccountID == filterVal
                                                                        select a.AccountID)
                                                                      .Contains(p.ProjectAccountID)
                                         select p.ProjectID).Contains(al.ProjectID)
                                     select al.EmployeeID)
                                                                      .Contains(e.EmployeeEntryID)
                                select e).Count();
                    break;
                case "allc":
                    recCount = (from e in DataContext.Employees
                                where e.IsDeleted == false
                                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                && (from al in DataContext.ProjectAllocations
                                    where al.IsDeleted == false && al.AllocationEndDate >= DateTime.Today
                                    && al.AllocationTypeID == filterVal
                                    select al.EmployeeID).Contains(e.EmployeeEntryID)
                                select e).Count();
                    break;
                case "pm":
                    var projs = DataContext.Projects.Where(p => p.IsDeleted == false && p.ProjectManagerID == filterVal).ToList();
                    if (projs == null || (projs != null && projs.Count == 0)) recCount = 0;
                    foreach (var prj in projs)
                    {
                        recCount += (from e in DataContext.Employees
                                     where e.IsDeleted == false
                                     && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                     && (from p in DataContext.ProjectAllocations
                                         where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today && p.ProjectID == prj.ProjectID
                                         select p.EmployeeID).Contains(e.EmployeeEntryID)
                                     select e).Count();
                    }
                    break;
                case "rm":
                    recCount = (from e in DataContext.Employees
                                where e.IsDeleted == false && e.ReportingManagerID == filterVal
                                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                select e).Count();
                    break;
                case "tot":
                    float ox = 0;
                    float.TryParse(filterText, out ox);
                    recCount = (from e in DataContext.Employees
                                where e.IsDeleted == false
                                && (((float)DbFunctions.DiffDays(e.DateOfJoin, DateTime.Today) / 365) + (e.PastExperience ?? 0.0)) >= ox
                                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                select e).Count();
                    break;
                case "eid":
                    recCount = (from e in DataContext.Employees
                                where e.IsDeleted == false && e.EmployeeID.ToLower().Contains(filterText.ToLower())
                                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                select e).Count();
                    break;
                case "techs":
                    if (string.IsNullOrWhiteSpace(filterText))
                        recCount = 0;
                    else
                    {
                        string[] skills = filterText.ToLower().Split(';');
                        List<int> selectedSkillIDs = new List<int>();

                        if (skills != null && skills.Length > 0)
                        {
                            foreach (string str in skills)
                            {
                                if (DataContext.TechSkills.Any(ts => ts.TechSkillName.ToLower().Contains(str) && ts.IsDeleted == false))
                                {
                                    selectedSkillIDs.AddRange(
                                        DataContext.TechSkills.Where(ts => ts.TechSkillName.ToLower().Contains(str) && ts.IsDeleted == false)
                                        .Select(p => p.TechSkillID).ToList());
                                }
                            }

                            List<int> empIDs = new List<int>();
                            foreach (int sID in selectedSkillIDs)
                            {
                                empIDs.AddRange(DataContext.EmployeeSkills.Where(es => es.IsDeleted == false && es.TechSkillID == sID).Select(s => s.EmployeeEntryID).ToList());
                            }
                            recCount = (from i in empIDs select i).Distinct().Count();
                        }
                        else
                        {
                            recCount = 0;
                        }
                    }
                    break;
            }

            return recCount;
        }

        public IEnumerable<EmployeeSkillsReportDto> GetEmployeeSkillsReport(string filterBy, string filterValue, string filterText, int pageSize = -1, int pageNo = -1)
        {
            List<EmployeeSkillsReportDto> employeeSkillsReport = new List<EmployeeSkillsReportDto>();

            int.TryParse(filterValue, out int filterVal);

            if (string.IsNullOrWhiteSpace(filterBy))
            {
                filterBy = "emp";
                filterVal = 0;
            }
            List<Employee> selectedEmps = null;

            IOrderedQueryable<Employee> empEnum = null;
            switch (filterBy)
            {
                case "emp":
                    if (filterVal == 0)
                    {
                        empEnum = (from ts in DataContext.Employees
                                   where ts.IsDeleted == false
                                   && (ts.LastWorkingDay.HasValue == false || (ts.LastWorkingDay.HasValue && ts.LastWorkingDay > DateTime.Today))
                                   orderby ts.EmployeeID
                                   select ts);
                    }
                    else
                    {
                        empEnum = DataContext.Employees.Where(e => e.EmployeeEntryID == filterVal).OrderBy(e => e.EmployeeID);
                    }
                    break;
                case "eid":
                    empEnum = (from ts in DataContext.Employees
                               where ts.IsDeleted == false && ts.EmployeeID.ToLower().Contains(filterText.ToLower())
                               && (ts.LastWorkingDay.HasValue == false || (ts.LastWorkingDay.HasValue && ts.LastWorkingDay > DateTime.Today))
                               orderby ts.EmployeeID
                               select ts);
                    break;
                case "rm":
                    empEnum = (from ts in DataContext.Employees
                               where ts.IsDeleted == false && ts.ReportingManagerID == filterVal
                               && (ts.LastWorkingDay.HasValue == false || (ts.LastWorkingDay.HasValue && ts.LastWorkingDay > DateTime.Today))
                               orderby ts.EmployeeID
                               select ts);
                    break;
                case "tot":
                    float ox = 0;
                    float.TryParse(filterText, out ox);
                    empEnum = (from e in DataContext.Employees
                               where e.IsDeleted == false
                               && (((float)DbFunctions.DiffDays(e.DateOfJoin, DateTime.Today) / 365) + (e.PastExperience ?? 0.0)) >= ox
                               && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay > DateTime.Today))
                               orderby (((float)DbFunctions.DiffDays(e.DateOfJoin, DateTime.Today) / 365) + (e.PastExperience ?? 0.0)) descending
                               select e);
                    break;
                case "prj":
                    empEnum = (from e in DataContext.Employees
                               where e.IsDeleted == false
                               && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                               && (from p in DataContext.ProjectAllocations
                                   where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today && p.ProjectID == filterVal
                                   select p.EmployeeID).Contains(e.EmployeeEntryID)
                               orderby e.EmployeeID
                               select e);
                    break;
                case "pm":
                    var projs = DataContext.Projects.Where(p => p.IsDeleted == false && p.ProjectManagerID == filterVal).ToList();
                    List<int> prjIDs = new List<int>();
                    if (projs != null && projs.Count > 0) ;
                    {
                        projs.ForEach(e => prjIDs.Add(e.ProjectID));
                    }

                    empEnum = (from e in DataContext.Employees
                               where e.IsDeleted == false
                               && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                               && (from p in DataContext.ProjectAllocations
                                   where p.IsDeleted == false && p.AllocationEndDate >= DateTime.Today
                                   && prjIDs.Contains(p.ProjectID)
                                   select p.EmployeeID).Contains(e.EmployeeEntryID)
                               orderby e.EmployeeID
                               select e);
                    break;
                case "acc":
                    empEnum = (from e in DataContext.Employees
                               where e.IsDeleted == false
                                && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                                && (from al in DataContext.ProjectAllocations
                                    where al.IsDeleted == false && al.AllocationEndDate >= DateTime.Today
                                    && (from p in DataContext.Projects
                                        where p.IsDeleted == false && (from a in DataContext.ProjectAccounts
                                                                       where a.IsDeleted == false
                                                                     && a.AccountID == filterVal
                                                                       select a.AccountID)
                                                                     .Contains(p.ProjectAccountID)
                                        select p.ProjectID).Contains(al.ProjectID)
                                    select al.EmployeeID).Contains(e.EmployeeEntryID)
                               orderby e.EmployeeID
                               select e);
                    break;
                case "allc":
                    empEnum = (from e in DataContext.Employees
                               where e.IsDeleted == false
                               && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue == true && e.LastWorkingDay >= DateTime.Today))
                               && (from al in DataContext.ProjectAllocations
                                   where al.IsDeleted == false && al.AllocationEndDate >= DateTime.Today
                                   && al.AllocationTypeID == filterVal
                                   select al.EmployeeID).Contains(e.EmployeeEntryID)
                               orderby e.EmployeeID
                               select e);
                    break;
                case "techs":
                    selectedEmps = new List<Employee>();
                    if (string.IsNullOrWhiteSpace(filterText) == false)
                    {
                        string[] skills = filterText.ToLower().Split(';');
                        List<int> selectedSkillIDs = new List<int>();

                        if (skills != null && skills.Length > 0)
                        {
                            foreach (string str in skills)
                            {
                                if (DataContext.TechSkills.Any(ts => ts.TechSkillName.ToLower().Contains(str) && ts.IsDeleted == false))
                                {
                                    selectedSkillIDs.AddRange(
                                        DataContext.TechSkills.Where(ts => ts.TechSkillName.ToLower().Contains(str) && ts.IsDeleted == false)
                                        .Select(p => p.TechSkillID).ToList());
                                }
                            }

                            List<int> empIDs = new List<int>();
                            foreach (int sID in selectedSkillIDs)
                            {
                                if (DataContext.EmployeeSkills.Any(es => es.IsDeleted == false && es.TechSkillID == sID))
                                {
                                    empIDs.AddRange(DataContext.EmployeeSkills.Where(es => es.IsDeleted == false && es.TechSkillID == sID).Select(s => s.EmployeeEntryID).ToList());
                                }
                            }

                            List<int> selectEmpIDs = (from i in empIDs select i).Distinct().ToList();
                            foreach (int eID in selectEmpIDs)
                            {
                                if (DataContext.Employees.Any(e => e.EmployeeEntryID == eID && e.IsDeleted == false && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay >= DateTime.Today))))
                                {
                                    selectedEmps.Add(DataContext.Employees.FirstOrDefault(e => e.EmployeeEntryID == eID
                                    && e.IsDeleted == false
                                    && (e.LastWorkingDay.HasValue == false || (e.LastWorkingDay.HasValue && e.LastWorkingDay >= DateTime.Today))));
                                }
                            }
                        }
                    }
                    break;
            }

            if (filterBy == "techs")
            {
                if (selectedEmps != null)
                {
                    if (pageSize <= 0 || pageNo < 1)
                    {
                        selectedEmps = selectedEmps.OrderBy(e => e.EmployeeID).ToList();
                    }
                    else
                    {
                        selectedEmps = selectedEmps.OrderBy(e => e.EmployeeID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
            }
            else
            {
                if (pageSize <= 0 || pageNo < 1)
                {
                    selectedEmps = empEnum.ToList();
                }
                else
                {
                    selectedEmps = empEnum.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                }
            }

            List<EmployeeSkill> employeeSkills = DataContext.EmployeeSkills.Where(r => r.IsDeleted == false).ToList();
            List<TechSkill> techSkills = DataContext.TechSkills.Where(s => s.IsDeleted == false).ToList();

            foreach (var emp in selectedEmps)
            {
                if (emp == null) continue;
                EmployeeSkillsReportDto empSkill = new EmployeeSkillsReportDto
                {
                    EmployeeEntryID = emp.EmployeeEntryID,
                    EmployeeID = emp.EmployeeID,
                    EmployeeName = emp.FirstName + " " + emp.LastName,
                    LastWorkingDay = emp.LastWorkingDay,
                    PastExperience = emp.PastExperience,
                    DateOfJoin = emp.DateOfJoin,
                };

                #region Collect Tech Skills

                var advancedSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Advanced).ToList();
                var expertSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Expert).ToList();
                var profSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Proficient).ToList();
                var limSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Limited).ToList();
                var basicSkills = employeeSkills.Where(e => e.EmployeeEntryID == emp.EmployeeEntryID && e.RatingID == (int)SkillRating.Basic).ToList();


                StringBuilder aSkillText = new StringBuilder();
                foreach (var askill in advancedSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == askill.TechSkillID);
                    if (sk != null)
                    {
                        aSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int aLI = aSkillText.ToString().LastIndexOf(", ");

                if (String.IsNullOrWhiteSpace(aSkillText.ToString()))
                    empSkill.AdvancedSkills = "None";
                else
                {
                    empSkill.AdvancedSkills = aSkillText.ToString().Substring(0, aLI);
                    empSkill.PrimarySkills = empSkill.AdvancedSkills;
                }

                StringBuilder eSkillText = new StringBuilder();
                foreach (var bskill in expertSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == bskill.TechSkillID);
                    if (sk != null)
                    {
                        eSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int bLI = eSkillText.ToString().LastIndexOf(", ");
                if (String.IsNullOrWhiteSpace(eSkillText.ToString()))
                    empSkill.ExpertSkills = "None";
                else
                {
                    empSkill.ExpertSkills = eSkillText.ToString().Substring(0, bLI);
                    empSkill.PrimarySkills += ", " + empSkill.ExpertSkills;
                }

                StringBuilder pSkillText = new StringBuilder();
                foreach (var pskill in profSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == pskill.TechSkillID);
                    if (sk != null)
                    {
                        pSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int pLI = pSkillText.ToString().LastIndexOf(", ");
                if (String.IsNullOrWhiteSpace(pSkillText.ToString()))
                    empSkill.ProficientSkills = "None";
                else
                {
                    empSkill.ProficientSkills = pSkillText.ToString().Substring(0, pLI);
                    empSkill.PrimarySkills += "," + empSkill.ProficientSkills;
                }

                StringBuilder bSkillText = new StringBuilder();
                foreach (var bskill in basicSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == bskill.TechSkillID);
                    if (sk != null)
                    {
                        bSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int baLI = bSkillText.ToString().LastIndexOf(", ");
                if (String.IsNullOrWhiteSpace(bSkillText.ToString()))
                    empSkill.BasicSkills = "None";
                else
                {
                    empSkill.BasicSkills = bSkillText.ToString().Substring(0, baLI);
                    empSkill.SecondarySkills = empSkill.BasicSkills;
                }

                StringBuilder lSkillText = new StringBuilder();
                foreach (var lskill in limSkills)
                {
                    var sk = techSkills.FirstOrDefault(s => s.TechSkillID == lskill.TechSkillID);
                    if (sk != null)
                    {
                        lSkillText.Append($"{sk.TechSkillName}, ");
                    }
                }
                int lLI = lSkillText.ToString().LastIndexOf(", ");
                if (String.IsNullOrWhiteSpace(lSkillText.ToString()))
                    empSkill.LimitedSkills = "None";
                else
                {
                    empSkill.LimitedSkills = lSkillText.ToString().Substring(0, lLI);
                    empSkill.SecondarySkills += "," + empSkill.LimitedSkills;
                }

                #endregion "--------------------------------------------------------------------"

                #region Collect Allocation Details

                if (DataContext.ProjectAllocations.Any(a => a.IsDeleted == false && a.EmployeeID == emp.EmployeeEntryID
                             && a.AllocationEndDate >= DateTime.Today))
                {
                    ProjectAllocation allocation = (from al in DataContext.ProjectAllocations
                                                    where al.IsDeleted == false && al.EmployeeID == emp.EmployeeEntryID
                                                    && al.AllocationEndDate >= DateTime.Today
                                                    orderby al.AllocationEndDate descending
                                                    select al).FirstOrDefault();
                    Project prj = DataContext.Projects.FirstOrDefault(p => p.ProjectID == allocation.ProjectID);
                    if (prj != null)
                    {
                        ProjectAccount acc = DataContext.ProjectAccounts.FirstOrDefault(ac => ac.AccountID == prj.ProjectAccountID);
                        if (acc != null)
                        {
                            empSkill.AccountID = acc.AccountID;
                            empSkill.AccountName = acc.AccountName;
                            empSkill.ProjectID = prj.ProjectID;
                            empSkill.ProjectName = $"{prj.ProjectCode} - {prj.ProjectName}";
                            Employee pm = DataContext.Employees.FirstOrDefault(e => e.EmployeeEntryID == prj.ProjectManagerID);
                            if (pm != null)
                            {
                                empSkill.ProjectManagerID = pm.EmployeeEntryID;
                                empSkill.ProjectManager = $"{pm.FirstName} {pm.LastName}";
                            }
                            empSkill.AllocationEndDate = allocation.AllocationEndDate;
                            empSkill.AlloctionStartDate = allocation.AllocationStartDate;
                            empSkill.AllocationTypeID = allocation.AllocationTypeID;
                            empSkill.AllocationType = DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryID == allocation.AllocationTypeID)?.SubCategoryName;
                        }
                    }
                }
                #endregion

                employeeSkillsReport.Add(empSkill);
            }

            return employeeSkillsReport;
        }

        #region Private Methods

        private EmpAssetDetail CreateBusinessEntity(EmpAssetDetailDto assetDto, bool isNewEntity = false)
        {
            EmpAssetDetail entity = new EmpAssetDetail
            {
                Designation = assetDto.Designation,
                EmployeeID = assetDto.EmployeeID,
                LocationID = assetDto.LocationID,
                OverallExperience = assetDto.OverallExperience,
                PastExperience = assetDto.PastExperience,
                PrimarySkills = assetDto.PrimarySkills,
                SecondarySkills = assetDto.SecondarySkills,
                VisaStatusID = assetDto.VisaStatusID,
                EmployeeEntryID = assetDto.EmployeeEntryID,
                EmpAssetDetailID = assetDto.EmpAssetDetailID,
            };

            entity.UpdateTimeStamp(assetDto.LoggedInUserName, true);
            return entity;
        }

        private void MigrateEntity(EmpAssetDetailDto sourceEntity, EmpAssetDetail targetEntity)
        {
            targetEntity.Designation = sourceEntity.Designation;
            targetEntity.EmployeeID = sourceEntity.EmployeeID;
            targetEntity.LocationID = sourceEntity.LocationID;
            targetEntity.OverallExperience = sourceEntity.OverallExperience;
            targetEntity.PastExperience = sourceEntity.PastExperience;
            targetEntity.PrimarySkills = sourceEntity.PrimarySkills;
            targetEntity.SecondarySkills = sourceEntity.SecondarySkills;
            targetEntity.VisaStatusID = sourceEntity.VisaStatusID;
            targetEntity.EmployeeEntryID = sourceEntity.EmployeeEntryID;

            targetEntity.UpdateTimeStamp(sourceEntity.LoggedInUserName);
        }

        #endregion
    }

    #region Contract Members

    public interface IEmployeeTechRepository : IRepository<EmpAssetDetailDto>
    {
        int GetEmployeeSkillsReportCount(string filterBy, string filterValue, string filterText);

        List<TechSkillCategoryDto> GetAllAvailableSkillCategories();

        List<TechSkillDto> GetAllAvailableTechSkills();

        List<TechSkillDto> GetAllAvailableTechSkillsByCategory(int categoryID);

        TechSkillDto GetTechSkillByID(int skillID);

        List<EmployeeSkillDto> GetAllEmployeeSkills(int employeeID);

        EmpAssetDetailDto GetByEmployeeID(string eid);

        int UpdateEmployeeDetails(EmpAssetDetailDto empDetails);

        EmpAssetDetailDto GetByEmployeeEntryID(int empEntryID);

        void AddEmpSkill(EmployeeSkillDto skill);

        int TotalRecordsCount(string findBy);

        void DeleteEmpTechSkill(EmployeeSkillDto employeeSkillDto);

        EmployeeSkillDto GetEmployeeSkillByID(int id);

        bool DoesEmployeeSkillExist(int id);

        void UpdateEmpSkill(EmployeeSkillDto skill);

        IEnumerable<EmpSkillSummaryDto> GetAllSkillSummary(string findBy, int pageSize = -1, int pageNo = -1);

        IEnumerable<EmployeeSkillsReportDto> GetEmployeeSkillsReport(string filterBy, string filterValue, string filterText, int pageSize = -1, int pageNo = -1);
    }

    #endregion
}

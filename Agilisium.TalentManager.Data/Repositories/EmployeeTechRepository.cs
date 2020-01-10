using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmpAssetDetailDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            throw new NotImplementedException();
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
                           LogonID = a.LogonID,
                           EmployeeEntryID = e.EmployeeEntryID
                       }).FirstOrDefault();
            }

            return emp;
        }

        public EmpAssetDetailDto GetByEmployeeID(string eid)
        {
            return (from a in Entities
                    join e in DataContext.Employees on a.EmployeeID equals e.EmployeeID
                    where a.EmployeeID == eid && a.IsDeleted == false
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
                        LogonID = a.LogonID,
                        EmployeeEntryID = e.EmployeeEntryID
                    }).FirstOrDefault();
        }

        public EmpAssetDetailDto GetByLogonID(string logonID)
        {
            return (from a in Entities
                    join e in DataContext.Employees on a.EmployeeID equals e.EmployeeID
                    where a.LogonID == logonID && a.IsDeleted == false
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
                        LogonID = a.LogonID,
                        EmployeeEntryID = e.EmployeeEntryID
                    }).FirstOrDefault();
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
                                             where t.IsDeleted == false && t.EmployeeID == employeeID
                                             select new EmployeeSkillDto
                                             {
                                                 EmployeeID = t.EmployeeID,
                                                 EmployeeSkillID = t.EmployeeSkillID,
                                                 RatingID = t.RatingID,
                                                 SkillCategoryID = t.SkillCategoryID,
                                                 TechSkillID = t.TechSkillID,
                                                 Rating = scd.SubCategoryName,
                                                 TechSkill = tsd.TechSkillName,
                                             }).ToList();

            return skills;
        }

        public void UpdateEmployeeDetails(EmpAssetDetailDto empDetails)
        {
            EmpAssetDetail assetEntity = new EmpAssetDetail();
            if (Entities.Any(e => e.LogonID == empDetails.LogonID && e.IsDeleted == false))
            {
                assetEntity = Entities.FirstOrDefault(e => e.LogonID == empDetails.LogonID && e.IsDeleted == false);
                MigrateEntity(empDetails, assetEntity);
                Entities.Add(assetEntity);
                DataContext.Entry(assetEntity).State = EntityState.Modified;
            }
            else
            {
                assetEntity = CreateBusinessEntity(empDetails);
                Entities.Add(assetEntity);
                DataContext.Entry(assetEntity).State = EntityState.Added;
            }
            DataContext.SaveChanges();
        }

        public EmployeeSkillDto GetEmployeeSkillByID(int id)
        {
            return (from skill in DataContext.EmployeeSkills
                    join ts in DataContext.TechSkills on skill.TechSkillID equals ts.TechSkillID into tse
                    from tsd in tse.DefaultIfEmpty()
                    where skill.EmployeeSkillID == id && skill.IsDeleted == false
                    select new EmployeeSkillDto
                    {
                        EmployeeID = skill.EmployeeID,
                        EmployeeSkillID = skill.EmployeeSkillID,
                        EntryID = skill.EmployeeID,
                        RatingID = skill.RatingID,
                        SkillCategoryID = skill.SkillCategoryID,
                        TechSkillID = skill.TechSkillID,
                        TechSkill = tsd.TechSkillName,
                    }).FirstOrDefault();
        }

        public void AddEmpSkill(EmployeeSkillDto skill)
        {
            if (skill.EmployeeID <= 0)
            {
                string empID = Entities.FirstOrDefault(e => e.LogonID == skill.LogonID)?.EmployeeID;
                skill.EmployeeID = DataContext.Employees.FirstOrDefault(e => e.EmployeeID == empID).EmployeeEntryID;
            }

            EmployeeSkill entity = new EmployeeSkill
            {
                EmployeeID = skill.EntryID,
                RatingID = skill.RatingID,
                TechSkillID = skill.TechSkillID,
                SkillCategoryID = skill.SkillCategoryID
            };
            DataContext.EmployeeSkills.Add(entity);
            DataContext.Entry(entity).State = EntityState.Added;
            DataContext.SaveChanges();
        }

        public void UpdateEmpSkill(EmployeeSkillDto skill)
        {
            if (skill.EmployeeID <= 0)
            {
                string empID = Entities.FirstOrDefault(e => e.LogonID == skill.LogonID)?.EmployeeID;
                skill.EmployeeID = DataContext.Employees.FirstOrDefault(e => e.EmployeeID == empID).EmployeeEntryID;
            }

            EmployeeSkill empSkillEntity = DataContext.EmployeeSkills.FirstOrDefault(e => e.EmployeeSkillID == skill.EmployeeSkillID && e.IsDeleted == false);
            if (empSkillEntity != null)
            {
                empSkillEntity.EmployeeID = skill.EmployeeID;
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

        private EmpAssetDetail CreateBusinessEntity(EmpAssetDetailDto assetDto, bool isNewEntity = false)
        {
            EmpAssetDetail entity = new EmpAssetDetail
            {
                Designation = assetDto.Designation,
                EmployeeID = assetDto.EmployeeID,
                LocationID = assetDto.LocationID,
                OverallExperience = assetDto.OverallExperience,
                PrimarySkills = assetDto.PrimarySkills,
                SecondarySkills = assetDto.SecondarySkills,
                VisaStatusID = assetDto.VisaStatusID,
                LogonID = assetDto.LogonID,
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
            targetEntity.PrimarySkills = sourceEntity.PrimarySkills;
            targetEntity.SecondarySkills = sourceEntity.SecondarySkills;
            targetEntity.VisaStatusID = sourceEntity.VisaStatusID;
            targetEntity.LogonID = sourceEntity.LogonID;

            targetEntity.UpdateTimeStamp(sourceEntity.LoggedInUserName);
        }
    }

    public interface IEmployeeTechRepository : IRepository<EmpAssetDetailDto>
    {
        List<TechSkillCategoryDto> GetAllAvailableSkillCategories();

        List<TechSkillDto> GetAllAvailableTechSkills();

        List<TechSkillDto> GetAllAvailableTechSkillsByCategory(int categoryID);

        TechSkillDto GetTechSkillByID(int skillID);

        List<EmployeeSkillDto> GetAllEmployeeSkills(int employeeID);

        EmpAssetDetailDto GetByEmployeeID(string eid);

        void UpdateEmployeeDetails(EmpAssetDetailDto empDetails);

        EmpAssetDetailDto GetByLogonID(string logonID);

        void AddEmpSkill(EmployeeSkillDto skill);

        void DeleteEmpTechSkill(EmployeeSkillDto employeeSkillDto);

        EmployeeSkillDto GetEmployeeSkillByID(int id);

        bool DoesEmployeeSkillExist(int id);

        void UpdateEmpSkill(EmployeeSkillDto skill);
    }
}

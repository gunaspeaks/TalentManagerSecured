using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Repository.Repositories;
using Agilisium.TalentManager.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Service.Concreate
{
    public class EmployeeTechService : IEmployeeTechService
    {
        IEmployeeTechRepository repository;

        public EmployeeTechService(IEmployeeTechRepository repository)
        {
            this.repository = repository;
        }

        public List<EmpAssetDetailDto> GetAll(int pageSize, int pageNo)
        {
            return repository.GetAll(pageSize, pageNo).ToList();
        }

        public EmpAssetDetailDto GetByID(int id)
        {
            return repository.GetByID(id);
        }

        public List<TechSkillDto> GetAllAvailableTechSkills()
        {
            return repository.GetAllAvailableTechSkills();
        }

        public List<TechSkillDto> GetAllAvailableTechSkillsByCategory(int categoryID)
        {
            return repository.GetAllAvailableTechSkillsByCategory(categoryID);
        }

        public List<TechSkillCategoryDto> GetAllAvailableSkillCategories()
        {
            return repository.GetAllAvailableSkillCategories();
        }

        public List<EmployeeSkillDto> GetAllEmployeeSkills(int employeeID)
        {
            return repository.GetAllEmployeeSkills(employeeID);
        }

        public EmpAssetDetailDto GetByEmployeeID(string eid)
        {
            return repository.GetByEmployeeID(eid);
        }

        public void UpdateEmployeeDetails(EmpAssetDetailDto empDetails)
        {
            repository.UpdateEmployeeDetails(empDetails);
        }

        public EmpAssetDetailDto GetByEmployeeEntryID(int empEntryID)
        {
            return repository.GetByEmployeeEntryID(empEntryID);
        }

        public TechSkillDto GetTechSkillByID(int skillID)
        {
            return repository.GetTechSkillByID(skillID);
        }

        public void AddEmpSkill(EmployeeSkillDto skill)
        {
            repository.AddEmpSkill(skill);
        }

        public void DeleteEmpTechSkill(EmployeeSkillDto employeeSkillDto)
        {
            repository.DeleteEmpTechSkill(employeeSkillDto);
        }

        public EmployeeSkillDto GetEmployeeSkillByID(int id)
        {
            return repository.GetEmployeeSkillByID(id);
        }

        public bool DoesEmployeeSkillExist(int id)
        {
            return repository.DoesEmployeeSkillExist(id);
        }

        public void UpdateEmpSkill(EmployeeSkillDto skill)
        {
            repository.UpdateEmpSkill(skill);
        }

        public bool Exists(string itemName)
        {
            return repository.Exists(itemName);
        }

        public int TotalRecordsCount(string findBy)
        {
            return repository.TotalRecordsCount(findBy);
        }

        public List<EmpSkillSummaryDto> GetAllSkillSummary(string findBy, int pageSize = -1, int pageNo = -1)
        {
            return repository.GetAllSkillSummary(findBy, pageSize, pageNo).ToList();
        }

        public List<EmployeeSkillsReportDto> GetEmployeeSkillsReport(string filterBy, string filterValue, string filterText, int pageSize = -1, int pageNo = -1)
        {
            return repository.GetEmployeeSkillsReport(filterBy, filterValue, filterText, pageSize, pageNo).ToList();
        }

        public int GetEmployeeSkillsReportCount(string filterBy, string filterValue, string filterText)
        {
            return repository.GetEmployeeSkillsReportCount(filterBy, filterValue, filterText);
        }
    }
}

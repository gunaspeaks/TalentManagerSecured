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

        public EmpAssetDetailDto GetByLogonID(string logonID)
        {
            return repository.GetByLogonID(logonID);
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
    }
}

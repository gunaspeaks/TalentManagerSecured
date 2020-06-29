using Agilisium.TalentManager.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Service.Abstract
{
    public interface IEmployeeTechService
    {
        List<TechSkillCategoryDto> GetAllAvailableSkillCategories();

        List<TechSkillDto> GetAllAvailableTechSkills();

        List<TechSkillDto> GetAllAvailableTechSkillsByCategory(int categoryID);

        List<EmpAssetDetailDto> GetAll(int pageSize, int pageNo);

        EmpAssetDetailDto GetByID(int id);

        List<EmployeeSkillDto> GetAllEmployeeSkills(int employeeID);

        EmpAssetDetailDto GetByEmployeeID(string eid);

        void UpdateEmployeeDetails(EmpAssetDetailDto empDetails);

        EmpAssetDetailDto GetByEmployeeEntryID(int empEntryID);

        TechSkillDto GetTechSkillByID(int skillID);

        void AddEmpSkill(EmployeeSkillDto skill);

        void DeleteEmpTechSkill(EmployeeSkillDto employeeSkillDto);

        EmployeeSkillDto GetEmployeeSkillByID(int id);

        bool DoesEmployeeSkillExist(int id);

        void UpdateEmpSkill(EmployeeSkillDto skill);

        bool Exists(string itemName);

        int TotalRecordsCount(string findBy);

        List<EmpSkillSummaryDto> GetAllSkillSummary(string findBy, int pageSize = -1, int pageNo = -1);
    }
}

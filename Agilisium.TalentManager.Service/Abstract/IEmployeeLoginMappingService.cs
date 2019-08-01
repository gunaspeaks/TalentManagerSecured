using Agilisium.TalentManager.Dto;
using System.Collections.Generic;

namespace Agilisium.TalentManager.Service.Abstract
{
    public interface IEmployeeLoginMappingService
    {
        void Add(EmployeeLoginMappingDto entity);

        void Delete(EmployeeLoginMappingDto entity);

        bool Exists(string itemName);

        bool Exists(int id);

        bool Exists(string loginUserID, int employeeID);

        List<EmployeeLoginMappingDto> GetAll(int pageSize = -1, int pageNo = -1);

        EmployeeLoginMappingDto GetByID(int id);

        void Update(EmployeeLoginMappingDto entity);

        int TotalRecordsCount();
    }
}

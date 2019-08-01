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
    public class EmployeeLoginMappingService : IEmployeeLoginMappingService
    {
        private readonly IEmployeeLoginMappingRepository repo;

        public EmployeeLoginMappingService(IEmployeeLoginMappingRepository repo)
        {
            this.repo = repo;
        }

        public void Add(EmployeeLoginMappingDto entity)
        {
            repo.Add(entity);
        }

        public void Delete(EmployeeLoginMappingDto entity)
        {
            repo.Delete(entity);
        }

        public bool Exists(string itemName)
        {
            return repo.Exists(itemName);
        }

        public bool Exists(int id)
        {
            return repo.Exists(id);
        }

        public bool Exists(string loginUserID, int employeeID)
        {
            return repo.Exists(loginUserID, employeeID);
        }

        public List<EmployeeLoginMappingDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            return repo.GetAll(pageSize, pageNo).ToList();
        }

        public EmployeeLoginMappingDto GetByID(int id)
        {
            return repo.GetByID(id);
        }

        public void Update(EmployeeLoginMappingDto entity)
        {
            repo.Update(entity);
        }

        public int TotalRecordsCount()
        {
            return repo.TotalRecordsCount();
        }
    }
}

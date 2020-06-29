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
    public class BuLevelService : IBuLevelService
    {
        private readonly IBuLevelRepository repo;

        public BuLevelService(IBuLevelRepository repo)
        {
            this.repo = repo;
        }

        public void Add(BuLevelDto levelItem)
        {
            repo.Add(levelItem);
        }

        public bool CanBeDeleted(int id)
        {
            return repo.CanBeDeleted(id);
        }

        public void Delete(BuLevelDto levelItem)
        {
            repo.Delete(levelItem);
        }

        public bool Exists(string itemName)
        {
            return repo.Exists(itemName);
        }

        public bool Exists(int id)
        {
            return repo.Exists(id);
        }

        public bool Exists(string itemName, int id)
        {
            return repo.Exists(itemName, id);
        }

        public IEnumerable<BuLevelDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            return repo.GetAll(pageSize, pageNo);
        }

        public IEnumerable<BuLevelDto> GetAllByBU(int buID)
        {
            return repo.GetAllByBU(buID);
        }

        public BuLevelDto GetlevelItem(int id)
        {
            return repo.GetByID(id);
        }

        public int TotalRecordsCount()
        {
            return repo.TotalRecordsCount();
        }

        public void Update(BuLevelDto levelItem)
        {
            repo.Update(levelItem);
        }
    }
}

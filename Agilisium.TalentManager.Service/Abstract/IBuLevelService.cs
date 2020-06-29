using Agilisium.TalentManager.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Service.Abstract
{
    public interface IBuLevelService
    {
        bool Exists(string itemName);

        bool Exists(int id);

        bool Exists(string itemName, int id);

        IEnumerable<BuLevelDto> GetAll(int pageSize = -1, int pageNo = -1);

        BuLevelDto GetlevelItem(int id);

        void Add(BuLevelDto levelItem);

        void Update(BuLevelDto levelItem);

        void Delete(BuLevelDto levelItem);

        int TotalRecordsCount();

        bool CanBeDeleted(int id);

        IEnumerable<BuLevelDto> GetAllByBU(int buID);
    }
}

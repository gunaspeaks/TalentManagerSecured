using Agilisium.TalentManager.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Service.Abstract
{
    public interface IResourceLevelService
    {
        bool Exists(string itemName);

        bool Exists(int id);

        bool Exists(string itemName, int id);

        IEnumerable<ResourceLevelDto> GetAll(int pageSize = -1, int pageNo = -1);

        ResourceLevelDto GetLevelItem(int id);

        void Add(ResourceLevelDto levelItem);

        void Update(ResourceLevelDto levelItem);

        void Delete(ResourceLevelDto levelItem);

        int TotalRecordsCount();

        bool CanBeDeleted(int id);

        IEnumerable<ResourceLevelDto> GetAllByLevel(int levelID);
    }
}

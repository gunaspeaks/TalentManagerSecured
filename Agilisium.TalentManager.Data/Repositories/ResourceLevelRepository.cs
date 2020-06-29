using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.Repository.Abstract;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class ResourceLevelRepository : RepositoryBase<ResourceLevel>, IResourceLevelRepository
    {
        public void Add(ResourceLevelDto entity)
        {
            ResourceLevel ResourceLevel = CreateBusinessEntity(entity, true);
            Entities.Add(ResourceLevel);
            DataContext.Entry(ResourceLevel).State = EntityState.Added;
            DataContext.SaveChanges();
        }

        public void Delete(ResourceLevelDto entity)
        {
            ResourceLevel buzEntity = Entities.FirstOrDefault(e => e.ItemEntryID == entity.ItemEntryID);
            buzEntity.IsDeleted = true;
            buzEntity.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public bool Exists(string levelName, int id)
        {
            return Entities.Any(c => c.ItemName.ToLower() == levelName.ToLower() &&
            c.ItemEntryID != id && c.IsDeleted == false);
        }

        public bool Exists(string itemName)
        {
            return Entities.Any(c => c.ItemName.ToLower() == itemName.ToLower() && c.IsDeleted == false);
        }

        public bool Exists(int id)
        {
            return Entities.Any(c => c.ItemEntryID == id && c.IsDeleted == false);
        }

        public IEnumerable<ResourceLevelDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            IQueryable<ResourceLevelDto> ResourceLevels = from p in Entities
                                                          join s in DataContext.BuLevels on p.ParentLevelID equals s.ItemEntryID into se
                                                          from sd in se.DefaultIfEmpty()
                                                          orderby sd.ItemName, p.ItemName
                                                          where p.IsDeleted == false
                                                          select new ResourceLevelDto
                                                          {
                                                              ParentLevelID = p.ParentLevelID,
                                                              ParentLevel = sd.ItemName,
                                                              ItemName = p.ItemName,
                                                              ItemEntryID = p.ItemEntryID,
                                                          };

            if (pageSize <= 0 || pageNo < 1)
            {
                return ResourceLevels;
            }

            return ResourceLevels.Skip((pageNo - 1) * pageSize).Take(pageSize);

        }

        public IEnumerable<ResourceLevelDto> GetAllByLevel(int levelID)
        {
            IQueryable<ResourceLevelDto> ResourceLevels = from p in Entities
                                                          join s in DataContext.BuLevels on p.ParentLevelID equals s.ItemEntryID into se
                                                          from sd in se.DefaultIfEmpty()
                                                          orderby p.ItemName
                                                          where p.IsDeleted == false && p.ParentLevelID == levelID
                                                          select new ResourceLevelDto
                                                          {
                                                              ParentLevelID = p.ParentLevelID,
                                                              ParentLevel = sd.ItemName,
                                                              ItemName = p.ItemName,
                                                              ItemEntryID = p.ItemEntryID,
                                                          };

            return ResourceLevels;
        }

        public ResourceLevelDto GetByID(int id)
        {
            return (from p in Entities
                    where p.ItemEntryID == id && p.IsDeleted == false
                    select new ResourceLevelDto
                    {
                        ParentLevelID = p.ParentLevelID,
                        ItemName = p.ItemName,
                        ItemEntryID = p.ItemEntryID,
                    }).FirstOrDefault();
        }

        public void Update(ResourceLevelDto entity)
        {
            ResourceLevel buzEntity = Entities.FirstOrDefault(e => e.ItemEntryID == entity.ItemEntryID);
            MigrateEntity(entity, buzEntity);
            buzEntity.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public override bool CanBeDeleted(int id)
        {
            // are there any dependancy with employee records
            if (DataContext.Employees.Any(c => c.IsDeleted == false && c.Level1ID==id))
            {
                return false;
            }

            return true;
        }

        private ResourceLevel CreateBusinessEntity(ResourceLevelDto categoryDto, bool isNewEntity = false)
        {
            ResourceLevel ResourceLevel = new ResourceLevel
            {
                ParentLevelID = categoryDto.ParentLevelID,
                ItemName = categoryDto.ItemName,
                ItemEntryID = categoryDto.ItemEntryID,
            };

            ResourceLevel.UpdateTimeStamp(categoryDto.LoggedInUserName, true);

            return ResourceLevel;
        }

        private void MigrateEntity(ResourceLevelDto sourceEntity, ResourceLevel targetEntity)
        {
            targetEntity.ParentLevelID = sourceEntity.ParentLevelID;
            targetEntity.ItemEntryID = sourceEntity.ItemEntryID;
            targetEntity.ItemName = sourceEntity.ItemName;
            targetEntity.UpdateTimeStamp(sourceEntity.LoggedInUserName);
        }
    }

    public interface IResourceLevelRepository : IRepository<ResourceLevelDto>
    {
        bool Exists(string ResourceLevelName, int id);

        IEnumerable<ResourceLevelDto> GetAllByLevel(int buID);
    }
}

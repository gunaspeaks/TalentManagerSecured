using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.PostgresDbHelper;
using Agilisium.TalentManager.Repository.Abstract;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class EmployeeLoginMappingRepository : RepositoryBase<EmployeeLoginMapping>, IEmployeeLoginMappingRepository
    {
        private readonly PostgresSqlProcessor postgresSqlProcessor = null;

        public EmployeeLoginMappingRepository()
        {
            postgresSqlProcessor = new PostgresSqlProcessor();
        }

        public void Add(EmployeeLoginMappingDto entity)
        {
            EmployeeLoginMapping user = CreateBusinessEntity(entity, true);
            Entities.Add(user);
            DataContext.Entry(user).State = EntityState.Added;
            DataContext.SaveChanges();

            postgresSqlProcessor.CreateMappingEntryForUserAndRole(user.LoginUserID, user.RoleID);
        }

        public void Update(EmployeeLoginMappingDto entity)
        {
            EmployeeLoginMapping user = Entities.FirstOrDefault(e => e.MappingID == entity.MappingID);
            user.IsBlocked = entity.IsBlocked;
            user.RoleID = entity.RoleID;
            user.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(user);
            DataContext.Entry(user).State = EntityState.Modified;
            DataContext.SaveChanges();

            postgresSqlProcessor.UpdateMappingEntryForUserAndRole(user.LoginUserID, user.RoleID);
        }

        public void Delete(EmployeeLoginMappingDto entity)
        {
            EmployeeLoginMapping buzEntity = Entities.FirstOrDefault(e => e.MappingID == entity.MappingID);
            buzEntity.IsDeleted = true;
            buzEntity.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();

            postgresSqlProcessor.DeleteRoleMappingEntryForUser(entity.LoginUserID);
        }

        public bool Exists(string itemName)
        {
            return Entities.Any(c => c.LoginUserID.ToLower() == itemName.ToLower() && c.IsDeleted == false);
        }

        public bool Exists(int id)
        {
            return Entities.Any(c => c.MappingID == id && c.IsDeleted == false);
        }

        public bool Exists(string loginUserID, int employeeID)
        {
            return Entities.Any(c => c.LoginUserID.ToLower() == loginUserID.ToLower() && c.EmployeeID == employeeID);
        }

        public IEnumerable<EmployeeLoginMappingDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            IQueryable<EmployeeLoginMappingDto> users = from p in Entities
                                                        join e in DataContext.Employees on p.EmployeeID equals e.EmployeeEntryID into ee
                                                        from ed in ee.DefaultIfEmpty()
                                                        orderby ed.FirstName + " " + ed.LastName
                                                        where p.IsDeleted == false
                                                        select new EmployeeLoginMappingDto
                                                        {
                                                            EmployeeID = p.EmployeeID,
                                                            EmployeeName = ed.FirstName + " " + ed.LastName,
                                                            IsBlocked = p.IsBlocked,
                                                            MappingID = p.MappingID,
                                                            LoginUserEmail = ed.EmailID,
                                                            LoginUserID = p.LoginUserID,
                                                            RoleID = p.RoleID,
                                                        };
            if (pageSize <= 0 || pageNo < 1)
            {
                return users;
            }

            List<EmployeeLoginMappingDto> entries = users.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            List<EmployeeLoginDto> userRoleMapping = postgresSqlProcessor.GetUserAndRoleMapEntries().ToList();

            foreach(var entry in entries)
            {
                var mapping = userRoleMapping.FirstOrDefault(u => u.UserID == entry.LoginUserID);
                entry.LoginUserEmail = mapping.Email;
                entry.RoleName = mapping.RoleName;
            }

            return entries;
        }

        public EmployeeLoginMappingDto GetByID(int id)
        {
            return (from p in Entities
                    join e in DataContext.Employees on p.EmployeeID equals e.EmployeeEntryID into ee
                    from ed in ee.DefaultIfEmpty()
                    where p.MappingID == id
                    select new EmployeeLoginMappingDto
                    {
                        EmployeeID = p.EmployeeID,
                        EmployeeName = ed.FirstName + " " + ed.LastName,
                        IsBlocked = p.IsBlocked,
                        MappingID = p.MappingID,
                        LoginUserEmail = ed.EmailID,
                        LoginUserID = p.LoginUserID,
                        RoleID = p.RoleID,
                    }).FirstOrDefault();
        }

        #region Private Methods

        private EmployeeLoginMapping CreateBusinessEntity(EmployeeLoginMappingDto userDto, bool isNewEntity = false)
        {
            EmployeeLoginMapping user = new EmployeeLoginMapping
            {
                EmployeeID = userDto.EmployeeID,
                IsBlocked = userDto.IsBlocked,
                LoginUserID = userDto.LoginUserID,
                MappingID = userDto.MappingID,
                RoleID = userDto.RoleID,
            };

            user.UpdateTimeStamp(userDto.LoggedInUserName, true);

            return user;
        }

        private void MigrateEntity(EmployeeLoginMappingDto sourceEntity, EmployeeLoginMapping targetEntity)
        {
            targetEntity.EmployeeID = sourceEntity.EmployeeID;
            targetEntity.IsBlocked = sourceEntity.IsBlocked;
            targetEntity.LoginUserID = sourceEntity.LoginUserID;
            targetEntity.MappingID = sourceEntity.MappingID;
            targetEntity.RoleID = sourceEntity.RoleID;

            targetEntity.UpdateTimeStamp(sourceEntity.LoggedInUserName);
        }

        #endregion
    }

    public interface IEmployeeLoginMappingRepository : IRepository<EmployeeLoginMappingDto>
    {
        bool Exists(string loginUserID, int employeeID);
    }
}

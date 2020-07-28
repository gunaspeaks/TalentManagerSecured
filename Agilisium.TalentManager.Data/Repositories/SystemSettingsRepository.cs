using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class SystemSettingRepository : RepositoryBase<SystemSetting>, ISystemSettingRepository
    {
        public void Add(SystemSettingDto entity)
        {
        }

        public void Delete(SystemSettingDto entity)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(string itemName)
        {
            return Entities.Any(e => e.SettingName == itemName);
        }

        public bool Exists(int id)
        {
            return true;
        }

        public IEnumerable<SystemSettingDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            return (from e in Entities
                    where e.IsDeleted == false
                    select new SystemSettingDto
                    {
                        SettingEntryID = e.SettingEntryID,
                        SettingName = e.SettingName,
                        SettingValue = e.SettingValue
                    });
        }

        public SystemSettingDto GetByID(int id)
        {
            return (from e in Entities
                    where e.IsDeleted == false
                    select new SystemSettingDto
                    {
                        SettingEntryID = e.SettingEntryID,
                        SettingName = e.SettingName,
                        SettingValue = e.SettingValue
                    }).FirstOrDefault();
        }

        public string GetSystemSettingValue(string settingName)
        {
            return Entities.FirstOrDefault(e => e.SettingName == settingName)?.SettingValue;
        }

        public void Update(SystemSettingDto entity)
        {
        }

        public WindowsServiceSettingsDto GetServiceSettings(int serviceID)
        {
            return (from ws in DataContext.WindowsServiceSettingEntries
                    where ws.ServiceID == serviceID
                    select new WindowsServiceSettingsDto
                    {
                        ServiceID = ws.ServiceID,
                        ExecutedDate = ws.ExecutedDate,
                        ExecutionInterval = ws.ExecutionInterval,
                        ExecutedTime = ws.ExecutedTime,
                        ServiceName = ws.ServiceName,
                    }).FirstOrDefault();
        }

        public void UpdateWindowsServiceStatus(WindowsServiceSettingsDto serviceSettings)
        {
            WindowsServiceSettings winService = DataContext.WindowsServiceSettingEntries.FirstOrDefault(w => w.ServiceID == serviceSettings.ServiceID);
            winService.ExecutedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            winService.ExecutedTime = $"{DateTime.Today.Hour}:{DateTime.Today.Minute}";
            winService.ExecutionInterval = serviceSettings.ExecutionInterval;
            winService.ServiceName = serviceSettings.ServiceName;
            winService.UpdateTimeStamp(serviceSettings.LoggedInUserName);

            DataContext.WindowsServiceSettingEntries.Add(winService);
            DataContext.Entry(winService).State = EntityState.Modified;
            DataContext.SaveChanges();

        }
    }

    public interface ISystemSettingRepository : IRepository<SystemSettingDto>
    {
        string GetSystemSettingValue(string settingName);
    }
}

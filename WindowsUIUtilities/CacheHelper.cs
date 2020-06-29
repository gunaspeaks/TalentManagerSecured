using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.WindowsUIUtilities
{
    public static class CacheHelper
    {
        //#region Private Data Members

        //private static List<TechnologyEntity> supportedTechnologies = null;
        //private static List<TechnologyFrameworkEntity> supportedFrameworks = null;
        //private static List<FileExtensionEntity> fileExtensions = null;
        //private static List<ListItemEntity> listItemList = null;
        //private static List<BusinessUnitEntity> businessUnitList = null;
        //private static List<SubBusinessUnitEntity> accountsList = null;
        //private static List<SystemSettingsEntity> systemSettingsList = null;
        //private static CastAdminEntity loggedInUser = null;
        //private static List<ListItemCategoryEntity> listItemCategoryList = null;
        //private static List<ServerDetailEntity> castServersList = null;
        //private static TechSpecBusinessProcessor techBusProcesssor = null;
        //private static ApplicationDataBusinessProcessor appDataProcessor = null;
        //private static CastCoeBusinessProcessor appOwnerProcessor = null;

        //#endregion

        //#region Public Properties

        //public static string WindowsIdentityName
        //{
        //    get
        //    {
        //        if (System.Security.Principal.WindowsIdentity.GetCurrent() != null)
        //            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        //        else
        //            return null;
        //    }
        //}

        //public static List<TechnologyEntity> SupportedTechnologies { get { return supportedTechnologies; } }

        //public static List<TechnologyFrameworkEntity> SupportedFrameworks { get { return supportedFrameworks; } }
        //public static List<FileExtensionEntity> FileExtensions { get { return fileExtensions; } }
        //public static List<ListItemEntity> ListItemList { get { return listItemList; } }
        //public static List<BusinessUnitEntity> BusinessUnitList { get { return businessUnitList; } }
        //public static List<SubBusinessUnitEntity> SubBusinessUnitList { get { return accountsList; } }
        //public static List<SystemSettingsEntity> SystemSettingsList { get { return systemSettingsList; } }
        //public static CastAdminEntity LoggedInUser { get { return loggedInUser; } }
        //public static List<ListItemCategoryEntity> ListItemCategoryList { get { return listItemCategoryList; } }

        //public static List<ServerDetailEntity> CastAnalysisServersList
        //{
        //    get
        //    {
        //        return (from srv in castServersList where srv.ServerCategory == CastServerType.AIPServer orderby srv.ServerName select srv).ToList();
        //    }
        //}

        //public static List<ServerDetailEntity> CastStorageServersList
        //{
        //    get
        //    {
        //        return (from srv in castServersList where srv.ServerCategory == CastServerType.CssServer orderby srv.ServerName select srv).ToList();
        //    }
        //}

        //public static List<ServerDetailEntity> CastAadServersList
        //{
        //    get
        //    {
        //        return (from srv in castServersList where srv.ServerCategory == CastServerType.AadPortalServer orderby srv.ServerName select srv).ToList();
        //    }
        //}

        //public static List<ServerDetailEntity> CastAedServersList
        //{
        //    get
        //    {
        //        return (from srv in castServersList where srv.ServerCategory == CastServerType.AedPortalServer orderby srv.ServerName select srv).ToList();
        //    }
        //}

        //public static List<ServerDetailEntity> CastAicServersList
        //{
        //    get
        //    {
        //        return (from srv in castServersList where srv.ServerCategory == CastServerType.AicPortalServer orderby srv.ServerName select srv).ToList();
        //    }
        //}

        //public static List<ServerDetailEntity> CastCedServersList
        //{
        //    get
        //    {
        //        return (from srv in castServersList where srv.ServerCategory == CastServerType.CedPortalServer orderby srv.ServerName select srv).ToList();
        //    }
        //}

        //#endregion

        //#region Construction and Dispose

        //static CacheHelper()
        //{
        //    techBusProcesssor = new TechSpecBusinessProcessor();
        //    appDataProcessor = new ApplicationDataBusinessProcessor();
        //    appOwnerProcessor = new CastCoeBusinessProcessor();
        //}

        //#endregion

        //#region Public Methods

        //public static void LoadApplicationCache()
        //{
        //    try
        //    {
        //        //supportedTechnologies = techBusProcesssor.GetAllTechnologies();
        //        //supportedFrameworks = techBusProcesssor.GetAllFrameworks();
        //        //fileExtensions = techBusProcesssor.GetAllFileExtensions();
        //        //listItemList = appDataProcessor.GetAllListItems();
        //        //businessUnitList = appDataProcessor.GetAllBusinessUnits().OrderBy(item => item.BusinessUnitName).ToList();
        //        //accountsList = appDataProcessor.GetAllSubBusinessUnits().OrderBy(item => item.SubBusinessUnitName).ToList();
        //        //systemSettingsList = appDataProcessor.GetSystemSettings();
        //        //listItemCategoryList = appDataProcessor.GetAllListItemCategories();
        //        //castServersList = appDataProcessor.GetCastServersList();

        //        //loggedInUser = GetLoggedUser();
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("Error has occured while loading the application Cache. This might be because of database connectivity.");
        //    }
        //    if (LoggedInUser == null) throw new InvalidProgramException("This application is not enabled with your account information. Please contact your manager to configure the same");
        //}

        ////public static List<ListItemEntity> GetListItemsByCategory(ListItemCategory category)
        ////{
        ////    return (from item in ListItemList where item.ItemCategory == category orderby item.ItemName select item).ToList();
        ////}

        ////public static List<SubBusinessUnitEntity> GetSubBusinessUnitsByBu(int businessUnitId)
        ////{
        ////    return (from item in SubBusinessUnitList where item.BusinessUnitID == businessUnitId orderby item.SubBusinessUnitName select item).ToList();
        ////}

        //public static string GetSystemSettingByName(string settingName)
        //{
        //    return (from item in SystemSettingsList where item.SettingsName.Trim() == settingName select item.SettingsValue.Trim()).FirstOrDefault();
        //}

        ////public static List<TechnologyFrameworkEntity> GetFrameworksByTechnology(int technologyId)
        ////{
        ////    if (SupportedFrameworks == null) return null;

        ////    return (from item in SupportedFrameworks where item.TechnologyId == technologyId orderby item.FrameworkName select item).ToList();
        ////}

        ////#endregion

        ////#region Private Methods

        ////private static CastAdminEntity GetLoggedUser()
        ////{
        ////    string empID = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Replace(string.Format(@"{0}\", ConfigurationSettings.DomainName), "");
        ////    //_LoggedInUser = appOwnerProcessor.GetUserByEmployeeId(empID);
        ////    loggedInUser = new CastAdminEntity { CastAdminID = 100, DisplayName = empID, EmployeeID = empID };
        ////    return loggedInUser;
        ////}

        //#endregion

    }
}

namespace Agilisium.TalentManager.Dto
{
    public class EmployeeLoginMappingDto : DtoBase
    {
        public int MappingID { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string LoginUserID { get; set; }

        public string LoginUserEmail { get; set; }

        public  string RoleID { get; set; }

        public string RoleName { get; set; }

        public bool IsBlocked { get; set; }
    }
}

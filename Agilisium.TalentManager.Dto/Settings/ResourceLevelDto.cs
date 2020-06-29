namespace Agilisium.TalentManager.Dto
{
    public class ResourceLevelDto : DtoBase
    {
        public int ItemEntryID { get; set; }

        public int ParentLevelID { get; set; }

        public string ParentLevel { get; set; }

        public string ItemName { get; set; }
    }
}

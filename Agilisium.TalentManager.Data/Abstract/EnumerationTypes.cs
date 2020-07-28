namespace Agilisium.TalentManager.Repository.Abstract
{
    public enum AllocationType
    {
        Billable = 4,
        Bench = 5,
        NonCommittedBuffer = 6,
        CommittedBuffer = 7
    }

    public enum BusinessUnit
    {
        BusinessDevelopment = 1,
        BusinessOperations,
        Delivery,
        RMG=103,
    }

    public enum BenchCategory
    {
        Earmarked = 74,
        Available
    }

    public enum WindowsServices
    {
        WeeklyAllocationsMailer = 1,
        ManagementNotifications,
        DailyAllocationsUpdater
    }

    public enum ProjectType
    {
        Lab=10,

    }

    public enum SkillRating
    {
        Limited=78,
        Basic,
        Proficient,
        Advanced,
        Expert
    }
}

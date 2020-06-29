using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Helpers
{
    public enum CategoryType
    {
        BusinessUnit = 1,
        UtilizationCode = 2,
        ProjectType = 3,
        EmploymentType = 4,
        SpecializedPartner = 5,
        ContractPeriod = 6,
        ServiceRequestType = 7,
        Country = 8,
        DevelopmentRequestType = 9,
        DevelopmentPriority = 10,
        DevelopmentRequestStatus = 11,
        VisaCategory = 12,
        BenchCategory = 13,
        TechnologyArea = 14,
        WorkLocation = 15,
        TechnologyRating = 16,
        RequirementReasonType = 17,
        RequirementRequestStatus = 18,
        RequirementRequestPriority = 19,
        AgingBand = 20,
        WorkLocationType = 21,
        StrengthArea = 22,
        BillabilityType = 23,
        Level4_NonBillableItems = 24,
        Level5_NonBillableItems = 25,
        RMG_Growth_Bench_Level_2_Items = 26,
        Delivery_Billable_Level_5_Items = 27,
    }

    public enum BusinessUnitType
    {
        BusinessDelevopment = 1,
        Operations,
        Delivery,
        RMG = 103
    }

    public enum EmployementType
    {
        Permanent = 1,
        Internship,
        Contract
    }

    public enum BillabilityType
    {
        Billable = 105,
        NonBillable,
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Helpers
{
    public enum CategoryType
    {
        BusinessUnit = 1,
        UtilizationCode,
        ProjectType,
        EmploymentType,
        SpecializedPartner,
        ContractPeriod,
        ServiceRequestType,
        Country,
        DevelopmentRequestType,
        DevelopmentPriority,
        DevelopmentRequestStatus,
        VisaCategory,
        BenchCategory,
        TechnologyArea,
        WorkLocation,
        TechnologyRating,
        RequirementReasonType,
        RequirementRequestStatus,
        RequirementRequestPriority,
        AgingBand,
        WorkLocationType,
    }

    public enum PracticeType
    {
        BusinessDelevopment = 1,
        Operations,
        Delivery
    }

    public enum EmployementType
    {
        Permanent = 1,
        Internship,
        Contract
    }
}
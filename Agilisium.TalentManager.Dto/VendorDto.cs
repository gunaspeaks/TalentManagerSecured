﻿namespace Agilisium.TalentManager.Dto
{
    public class VendorDto : DtoBase
    {
        public int VendorID { get; set; }

        public bool IsSelected { get; set; }

        public string VendorName { get; set; }

        public string Location { get; set; }

        public int SpecializedPartnerID { get; set; }

        public string SpecializedPartner { get; set; }

        public string CEO { get; set; }

        public string PoC1 { get; set; }

        public string PoCPhone1 { get; set; }

        public string PoCEmail1 { get; set; }

        public string PoC2 { get; set; }

        public string PoCPhone2 { get; set; }

        public string PoCEmail2 { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public string Address { get; set; }

        public string RequestedSkill { get; set; }
    }
}

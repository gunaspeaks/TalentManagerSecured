using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class RecruitmentRequestRepository : RepositoryBase<RecruitmentRequest>, IRecruitmentRequestRepository
    {
        public void Add(RecruitmentRequestDto entity)
        {
            RecruitmentRequest subCategory = CreateBusinessEntity(entity, true);
            Entities.Add(subCategory);
            DataContext.Entry(subCategory).State = EntityState.Added;
            DataContext.SaveChanges();

            int? statusID = DataContext.DropDownSubCategories.FirstOrDefault(s => s.SubCategoryName == "Open" && s.CategoryID == 18 && s.IsDeleted == false)?.SubCategoryID;
            RecruitmentRequestStatus requestStatus = new RecruitmentRequestStatus
            {
                Comments = entity.LattestComment,
                RecruitmentRequestID = subCategory.RecruitmentRequestID,
                RequestStatusID = statusID ?? 1,
                RequestUpdatedOn = DateTime.Today,
                JoinedPositions = 0,
                OfferedPositions = 0,
            };
            requestStatus.UpdateTimeStamp(entity.LoggedInUserName, true);
            DataContext.RequestStatuseEntries.Add(requestStatus);
            DataContext.Entry(requestStatus).State = EntityState.Added;
            DataContext.SaveChanges();
        }

        public void AddRequestStatus(RecruitmentRequestStatusDto requestStatus)
        {
            RecruitmentRequestStatus status = new RecruitmentRequestStatus
            {
                Comments = requestStatus.Comments,
                RecruitmentRequestID = requestStatus.RecruitmentRequestID,
                RequestUpdatedOn = requestStatus.RequestUpdatedOn,
                RequestStatusEntryID = requestStatus.RequestStatusEntryID,
                RequestStatusID = requestStatus.RequestStatusID,
                JoinedPositions = requestStatus.JoinedPosition,
                OfferedPositions = requestStatus.OfferedPosition,
            };

            status.UpdateTimeStamp(requestStatus.LoggedInUserName, true);
            DataContext.RequestStatuseEntries.Add(status);
            DataContext.Entry(status).State = EntityState.Added;
            DataContext.SaveChanges();

            UpdateOverallStatus(requestStatus.RecruitmentRequestID, requestStatus.RequestStatusID, requestStatus.LoggedInUserName, requestStatus.JoinedPosition, requestStatus.OfferedPosition);
        }

        public void Delete(RecruitmentRequestDto entity)
        {
            RecruitmentRequest subCategory = Entities.FirstOrDefault(e => e.RecruitmentRequestID == entity.RecruitmentRequestID);
            subCategory.IsDeleted = true;
            subCategory.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(subCategory);
            DataContext.Entry(subCategory).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public bool Exists(string itemName, int id)
        {
            return Entities.Any(c => c.RequestNo.ToLower() == itemName.ToLower() &&
            c.RecruitmentRequestID != id && c.IsDeleted == false);
        }

        public bool Exists(string requestNo)
        {
            return Entities.Any(c => c.RequestNo.ToLower() == requestNo.ToLower() && c.IsDeleted == false);
        }

        public bool Exists(int id)
        {
            return Entities.Any(c => c.RecruitmentRequestID == id && c.IsDeleted == false);

        }

        public IEnumerable<RecruitmentRequestDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            IQueryable<RecruitmentRequestDto> requests = null;
            try
            {
                requests = from rr in Entities
                           join bu in DataContext.DropDownSubCategories on rr.BusinessUnitID equals bu.SubCategoryID into bue
                           from bud in bue.DefaultIfEmpty()
                           join ac in DataContext.ProjectAccounts on rr.AccountID equals ac.AccountID into ace
                           from acd in ace.DefaultIfEmpty()
                           join pr in DataContext.Projects on rr.ProjectID equals pr.ProjectID into pre
                           from prd in pre.DefaultIfEmpty()
                           join wl in DataContext.DropDownSubCategories on rr.WorkLocationTypeID equals wl.SubCategoryID into wle
                           from wld in wle.DefaultIfEmpty()
                           join rs in DataContext.DropDownSubCategories on rr.RequestReasonID equals rs.SubCategoryID into rse
                           from rsd in rse.DefaultIfEmpty()
                           join ee in DataContext.Employees on rr.ReplacementID equals ee.EmployeeEntryID into eee
                           from eed in eee.DefaultIfEmpty()
                           join ab in DataContext.DropDownSubCategories on rr.AgingBandID equals ab.SubCategoryID into abe
                           from abd in abe.DefaultIfEmpty()
                           join pt in DataContext.DropDownSubCategories on rr.PriorityID equals pt.SubCategoryID into pte
                           from ptd in pte.DefaultIfEmpty()
                           join st in DataContext.DropDownSubCategories on rr.OverallStatusID equals st.SubCategoryID into ste
                           from std in ste.DefaultIfEmpty()
                           where rr.IsDeleted == false
                           orderby rr.RequestedDate descending
                           select new RecruitmentRequestDto
                           {
                               Account = acd.AccountName,
                               AccountID = rr.AccountID,
                               AgingBand = abd.SubCategoryName,
                               AgingBandID = rr.AgingBandID,
                               BusinessUnit = bud.SubCategoryName,
                               BusinessUnitID = rr.BusinessUnitID,
                               IsBillable = rr.IsBillable,
                               JoinedCount = rr.JoinedPosition,
                               OfferedCount = rr.OfferedPosition,
                               OfferOrHoldDate = rr.OfferOrHoldDate,
                               Priority = ptd.SubCategoryName,
                               PriorityID = rr.PriorityID,
                               Project = prd != null ? prd.ProjectName : "",
                               ProjectID = rr.ProjectID,
                               ProjectStartDate = prd.StartDate,
                               Replacement = eed != null ? eed.FirstName + " " + eed.LastName : "",
                               ReplacementID = rr.ReplacementID,
                               RequestedDate = rr.RequestedDate,
                               RequestNo = rr.RequestNo,
                               RequestReason = rsd.SubCategoryName,
                               RequestReasonID = rr.RequestReasonID,
                               RequiredSkills = rr.RequiredSkills,
                               RecruitmentRequestID = rr.RecruitmentRequestID,
                               TotalPosition = rr.TotalPosition,
                               WorkLocationType = wld.SubCategoryName,
                               WorkLocationTypeID = rr.WorkLocationTypeID,
                               OverallStatus = std.SubCategoryName,
                               OverallStatusID = rr.OverallStatusID,
                           };

                if (pageSize <= 0 || pageNo < 1)
                {
                    return requests;
                }
            }
            catch (Exception)
            {

            }
            return requests?.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<RecruitmentRequestDto> GetAll(string filterType, int filterValueID, int pageSize = -1, int pageNo = -1)
        {
            if (string.IsNullOrWhiteSpace(filterType))
            {
                return GetAll(pageSize, pageNo);
            }

            IQueryable<RecruitmentRequest> recordsToRet = null;

            switch (filterType)
            {
                case "prj":
                    recordsToRet = Entities.Where(e => e.IsDeleted == false && e.ProjectID == filterValueID);
                    break;
                case "acc":
                    recordsToRet = Entities.Where(e => e.IsDeleted == false && e.AccountID == filterValueID);
                    break;
                case "wol":
                    recordsToRet = Entities.Where(e => e.IsDeleted == false && e.WorkLocationTypeID == filterValueID);
                    break;
                case "sts":
                    recordsToRet = Entities.Where(e => e.IsDeleted == false && e.OverallStatusID == filterValueID);
                    break;
                case "pri":
                    recordsToRet = Entities.Where(e => e.IsDeleted == false && e.PriorityID == filterValueID);
                    break;
                case "age":
                    recordsToRet = Entities.Where(e => e.IsDeleted == false && e.AgingBandID == filterValueID);
                    break;
            }

            IQueryable<RecruitmentRequest> records = recordsToRet?.OrderByDescending(s => s.RequestedDate).Skip((pageNo - 1) * pageSize).Take(pageSize);

            return (from rr in records
                    join bu in DataContext.DropDownSubCategories on rr.BusinessUnitID equals bu.SubCategoryID into bue
                    from bud in bue.DefaultIfEmpty()
                    join ac in DataContext.ProjectAccounts on rr.AccountID equals ac.AccountID into ace
                    from acd in ace.DefaultIfEmpty()
                    join pr in DataContext.Projects on rr.ProjectID equals pr.ProjectID into pre
                    from prd in pre.DefaultIfEmpty()
                    join wl in DataContext.DropDownSubCategories on rr.WorkLocationTypeID equals wl.SubCategoryID into wle
                    from wld in wle.DefaultIfEmpty()
                    join rs in DataContext.DropDownSubCategories on rr.RequestReasonID equals rs.SubCategoryID into rse
                    from rsd in rse.DefaultIfEmpty()
                    join ee in DataContext.Employees on rr.ReplacementID equals ee.EmployeeEntryID into eee
                    from eed in eee.DefaultIfEmpty()
                    join ab in DataContext.DropDownSubCategories on rr.AgingBandID equals ab.SubCategoryID into abe
                    from abd in abe.DefaultIfEmpty()
                    join pt in DataContext.DropDownSubCategories on rr.PriorityID equals pt.SubCategoryID into pte
                    from ptd in pte.DefaultIfEmpty()
                    join st in DataContext.DropDownSubCategories on rr.OverallStatusID equals st.SubCategoryID into ste
                    from std in ste.DefaultIfEmpty()
                    orderby rr.RequestedDate descending
                    select new RecruitmentRequestDto
                    {
                        Account = acd.AccountName,
                        AccountID = rr.AccountID,
                        AgingBand = abd.SubCategoryName,
                        AgingBandID = rr.AgingBandID,
                        BusinessUnit = bud.SubCategoryName,
                        BusinessUnitID = rr.BusinessUnitID,
                        IsBillable = rr.IsBillable,
                        JoinedCount = rr.JoinedPosition,
                        OfferedCount = rr.OfferedPosition,
                        OfferOrHoldDate = rr.OfferOrHoldDate,
                        Priority = ptd.SubCategoryName,
                        PriorityID = rr.PriorityID,
                        Project = prd != null ? prd.ProjectName : "",
                        ProjectID = rr.ProjectID,
                        ProjectStartDate = prd.StartDate,
                        Replacement = eed != null ? eed.FirstName + " " + eed.LastName : "",
                        ReplacementID = rr.ReplacementID,
                        RequestedDate = rr.RequestedDate,
                        RequestNo = rr.RequestNo,
                        RequestReason = rsd.SubCategoryName,
                        RequestReasonID = rr.RequestReasonID,
                        RequiredSkills = rr.RequiredSkills,
                        RecruitmentRequestID = rr.RecruitmentRequestID,
                        TotalPosition = rr.TotalPosition,
                        WorkLocationType = wld.SubCategoryName,
                        WorkLocationTypeID = rr.WorkLocationTypeID,
                        OverallStatus = std.SubCategoryName,
                        OverallStatusID = rr.OverallStatusID,
                    });
        }

        public RecruitmentRequestDto GetByID(int id)
        {
            return (from rr in Entities
                    where rr.RecruitmentRequestID == id && rr.IsDeleted == false
                    select new RecruitmentRequestDto
                    {
                        AccountID = rr.AccountID,
                        AgingBandID = rr.AgingBandID,
                        BusinessUnitID = rr.BusinessUnitID,
                        IsBillable = rr.IsBillable,
                        JoinedCount = rr.JoinedPosition,
                        OfferedCount = rr.OfferedPosition,
                        OfferOrHoldDate = rr.OfferOrHoldDate,
                        PriorityID = rr.PriorityID,
                        ProjectID = rr.ProjectID,
                        ReplacementID = rr.ReplacementID,
                        RequestedDate = rr.RequestedDate,
                        RequestNo = rr.RequestNo,
                        RequestReasonID = rr.RequestReasonID,
                        RequiredSkills = rr.RequiredSkills,
                        RecruitmentRequestID = rr.RecruitmentRequestID,
                        TotalPosition = rr.TotalPosition,
                        WorkLocationTypeID = rr.WorkLocationTypeID,
                        OverallStatusID = rr.OverallStatusID,
                    }).FirstOrDefault();
        }

        public void Update(RecruitmentRequestDto entity)
        {
            RecruitmentRequest buzEntity = Entities.FirstOrDefault(e => e.RecruitmentRequestID == entity.RecruitmentRequestID);
            MigrateEntity(entity, buzEntity);
            buzEntity.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public int TotalRecordsCount(string filterType, int filterValueID)
        {
            if (string.IsNullOrWhiteSpace(filterType))
            {
                return TotalRecordsCount();
            }

            int count = 0;
            switch (filterType)
            {
                case "prj":
                    count = Entities.Count(e => e.IsDeleted == false && e.ProjectID == filterValueID);
                    break;
                case "acc":
                    count = Entities.Count(e => e.IsDeleted == false && e.AccountID == filterValueID);
                    break;
                case "wol":
                    count = Entities.Count(e => e.IsDeleted == false && e.WorkLocationTypeID == filterValueID);
                    break;
                case "sts":
                    count = Entities.Count(e => e.IsDeleted == false && e.OverallStatusID == filterValueID);
                    break;
                case "pri":
                    count = Entities.Count(e => e.IsDeleted == false && e.PriorityID == filterValueID);
                    break;
                case "age":
                    count = Entities.Count(e => e.IsDeleted == false && e.AgingBandID == filterValueID);
                    break;
            }

            return count;
        }

        public IEnumerable<RecruitmentRequestStatusDto> GetStatusEntriesForRequest(int requestID)
        {
            List<RecruitmentRequestStatusDto> res = (from re in DataContext.RequestStatuseEntries
                                                     join r in Entities on re.RecruitmentRequestID equals r.RecruitmentRequestID
                                                     join s in DataContext.DropDownSubCategories on re.RequestStatusID equals s.SubCategoryID into se
                                                     from sd in se.DefaultIfEmpty()
                                                     where re.IsDeleted == false && re.RecruitmentRequestID == requestID
                                                     orderby re.RequestUpdatedOn descending
                                                     select new RecruitmentRequestStatusDto
                                                     {
                                                         Comments = re.Comments,
                                                         RecruitmentRequestID = re.RecruitmentRequestID,
                                                         RequestStatus = sd.SubCategoryName,
                                                         RequestStatusEntryID = re.RequestStatusEntryID,
                                                         RequestStatusID = re.RequestStatusID,
                                                         RequestUpdatedOn = re.RequestUpdatedOn,
                                                         RequestNo = r.RequestNo,
                                                         OfferedPosition = re.OfferedPositions,
                                                         JoinedPosition = re.JoinedPositions,
                                                         TotalPosition = r.TotalPosition,
                                                         OpenPosition = r.TotalPosition - re.JoinedPositions - re.OfferedPositions,
                                                     }).ToList();
            return res;
        }

        private RecruitmentRequest CreateBusinessEntity(RecruitmentRequestDto reqDto, bool isNewEntity = false)
        {
            RecruitmentRequest request = new RecruitmentRequest
            {
                AccountID = reqDto.AccountID,
                AgingBandID = reqDto.AgingBandID,
                BusinessUnitID = reqDto.BusinessUnitID,
                IsBillable = reqDto.IsBillable,
                JoinedPosition = reqDto.JoinedCount,
                OfferedPosition = reqDto.OfferedCount,
                OfferOrHoldDate = reqDto.OfferOrHoldDate,
                PriorityID = reqDto.PriorityID,
                ProjectID = reqDto.ProjectID,
                ReplacementID = reqDto.ReplacementID,
                RequestedDate = reqDto.RequestedDate,
                RequestNo = reqDto.RequestNo?.ToUpper(),
                RequestReasonID = reqDto.RequestReasonID,
                RequiredSkills = reqDto.RequiredSkills,
                RecruitmentRequestID = reqDto.RecruitmentRequestID,
                TotalPosition = reqDto.TotalPosition,
                WorkLocationTypeID = reqDto.WorkLocationTypeID,
                OverallStatusID = reqDto.OverallStatusID,
            };

            request.UpdateTimeStamp(reqDto.LoggedInUserName, true);
            return request;
        }

        private void MigrateEntity(RecruitmentRequestDto sourceEntity, RecruitmentRequest targetEntity)
        {
            targetEntity.AccountID = sourceEntity.AccountID;
            targetEntity.AgingBandID = sourceEntity.AgingBandID;
            targetEntity.BusinessUnitID = sourceEntity.BusinessUnitID;
            targetEntity.IsBillable = sourceEntity.IsBillable;
            targetEntity.JoinedPosition = sourceEntity.JoinedCount;
            targetEntity.OfferedPosition = sourceEntity.OfferedCount;
            targetEntity.OfferOrHoldDate = sourceEntity.OfferOrHoldDate;
            targetEntity.PriorityID = sourceEntity.PriorityID;
            targetEntity.ProjectID = sourceEntity.ProjectID;
            targetEntity.ReplacementID = sourceEntity.ReplacementID;
            targetEntity.RequestedDate = sourceEntity.RequestedDate;
            targetEntity.RequestNo = sourceEntity.RequestNo;
            targetEntity.RequestReasonID = sourceEntity.RequestReasonID;
            targetEntity.RequiredSkills = sourceEntity.RequiredSkills;
            targetEntity.RecruitmentRequestID = sourceEntity.RecruitmentRequestID;
            targetEntity.TotalPosition = sourceEntity.TotalPosition;
            targetEntity.WorkLocationTypeID = sourceEntity.WorkLocationTypeID;
            targetEntity.OverallStatusID = sourceEntity.OverallStatusID;
        }

        private void UpdateOverallStatus(int requestID, int statusID, string userName, int joined, int offered)
        {
            RecruitmentRequest buzEntity = Entities.FirstOrDefault(e => e.RecruitmentRequestID == requestID);
            buzEntity.OverallStatusID = statusID;
            buzEntity.UpdateTimeStamp(userName);
            buzEntity.OfferedPosition = offered;
            buzEntity.JoinedPosition = joined;
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }
    }

    public interface IRecruitmentRequestRepository : IRepository<RecruitmentRequestDto>
    {
        bool Exists(string itemName, int id);

        int TotalRecordsCount(string filterType, int filterValueID);

        IEnumerable<RecruitmentRequestDto> GetAll(string filterType, int filterValueID, int pageSize = -1, int pageNo = -1);

        IEnumerable<RecruitmentRequestStatusDto> GetStatusEntriesForRequest(int requestID);

        void AddRequestStatus(RecruitmentRequestStatusDto requestStatus);
    }
}

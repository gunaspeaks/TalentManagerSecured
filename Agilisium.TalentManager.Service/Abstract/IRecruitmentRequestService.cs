using Agilisium.TalentManager.Dto;
using System.Collections.Generic;

namespace Agilisium.TalentManager.Service.Abstract
{
    public interface IRecruitmentRequestService
    {
        bool Exists(string reqNo);

        bool Exists(int id);

        bool Exists(string reqNo, int id);

        List<RecruitmentRequestDto> GetAll(int pageSize = -1, int pageNo = -1);

        List<RecruitmentRequestDto> GetAll(string filterType, int filterValueID, int pageSize = -1, int pageNo = -1);

        RecruitmentRequestDto GetByID(int id);

        void Create(RecruitmentRequestDto request);

        void Update(RecruitmentRequestDto request);

        void Delete(RecruitmentRequestDto request);

        int TotalRecordsCount();

        int TotalRecordsCount(string filterType, int filterValueID);

        List<RecruitmentRequestStatusDto> GetStatusEntriesForRequest(int requestID);

        void AddRequestStatus(RecruitmentRequestStatusDto requestStatus);
    }
}

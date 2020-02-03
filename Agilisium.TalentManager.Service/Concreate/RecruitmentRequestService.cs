using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Repository.Repositories;
using Agilisium.TalentManager.Service.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Agilisium.TalentManager.Service.Concreate
{
    public class RecruitmentRequestService : IRecruitmentRequestService
    {
        private readonly IRecruitmentRequestRepository repository;

        public RecruitmentRequestService(IRecruitmentRequestRepository repository)
        {
            this.repository = repository;
        }

        public void Create(RecruitmentRequestDto request)
        {
            repository.Add(request);
        }

        public void Delete(RecruitmentRequestDto request)
        {
            repository.Delete(request);
        }

        public bool Exists(string request)
        {
            return repository.Exists(request);
        }

        public bool Exists(int id)
        {
            return repository.Exists(id);
        }

        public bool Exists(string reqNo, int id)
        {
            return repository.Exists(reqNo, id);
        }

        public List<RecruitmentRequestDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            return repository.GetAll(pageSize, pageNo).ToList();
        }

        public RecruitmentRequestDto GetByID(int id)
        {
            return repository.GetByID(id);
        }

        public void Update(RecruitmentRequestDto request)
        {
            repository.Update(request);
        }

        public int TotalRecordsCount()
        {
            return repository.TotalRecordsCount();
        }

        public int TotalRecordsCount(string filterType, int filterValueID)
        {
            return repository.TotalRecordsCount(filterType, filterValueID);
        }

        public List<RecruitmentRequestDto> GetAll(string filterType, int filterValueID, int pageSize = -1, int pageNo = -1)
        {
            return repository.GetAll(filterType, filterValueID, pageSize, pageNo).ToList();
        }

        public List<RecruitmentRequestStatusDto> GetStatusEntriesForRequest(int requestID)
        {
            return repository.GetStatusEntriesForRequest(requestID).ToList();
        }

        public void AddRequestStatus(RecruitmentRequestStatusDto requestStatus)
        {
            repository.AddRequestStatus(requestStatus);
        }
    }
}

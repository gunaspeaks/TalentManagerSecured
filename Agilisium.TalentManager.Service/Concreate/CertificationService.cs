using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Repository.Repositories;
using Agilisium.TalentManager.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Service.Concreate
{
    public class CertificationService : ICertificationService
    {
        private readonly ICertificationRepository repo;

        public CertificationService(ICertificationRepository repo)
        {
            this.repo = repo;
        }

        public void AddCertificate(CertificationDto entity)
        {
            repo.Add(entity);
        }

        public bool CanBeDeleted(int id)
        {
            return repo.CanBeDeleted(id);
        }

        public void Delete(CertificationDto entity)
        {
            repo.Delete(entity);
        }

        public bool Exists(string certificateName, int id)
        {
            return repo.Exists(certificateName, id);
        }

        public bool Exists(string certificateName)
        {
            return repo.Exists(certificateName);
        }

        public bool Exists(int id)
        {
            return repo.Exists(id);
        }

        public List<CertificationDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            return repo.GetAll(pageSize, pageNo).ToList();
        }

        public List<CertificationDto> GetAllByTechnologyArea(int areaID,int pageSize = -1, int pageNo = -1)
        {
            return repo.GetAllByTechnologyArea(areaID, pageSize, pageNo).ToList();
        }

        public CertificationDto GetByID(int id)
        {
            return repo.GetByID(id);
        }

        public CertificationDto GetByName(string name, int certificateID)
        {
            return repo.GetByName(name, certificateID);
        }

        public int TotalRecordsCount()
        {
            return repo.TotalRecordsCount();
        }

        public int TotalRecordsCountByTechAreaID(int areaID)
        {
            return repo.TotalRecordsCountByTechAreaID(areaID);
        }

        public void Update(CertificationDto entity)
        {
            repo.Update(entity);
        }
    }
}

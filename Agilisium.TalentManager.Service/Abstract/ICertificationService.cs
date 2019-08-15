using Agilisium.TalentManager.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Service.Abstract
{
    public interface ICertificationService
    {
        void AddCertificate(CertificationDto entity);

        void Delete(CertificationDto entity);

        bool Exists(string certificateName, int id);

        bool Exists(string certificateName);

        bool Exists(int id);

        List<CertificationDto> GetAll(int pageSize = -1, int pageNo = -1);

        CertificationDto GetByID(int id);

        CertificationDto GetByName(string name, int certificateID);

        List<CertificationDto> GetAllByTechnologyArea(int areaID, int pageSize = -1, int pageNo = -1);

        void Update(CertificationDto entity);

        bool CanBeDeleted(int id);

        int TotalRecordsCount();

        int TotalRecordsCountByTechAreaID(int areaID);

    }
}

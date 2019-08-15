using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Model.Entities;
using Agilisium.TalentManager.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Agilisium.TalentManager.Repository.Repositories
{
    public class CertificationRepository : RepositoryBase<Certification>, ICertificationRepository
    {
        public void Add(CertificationDto entity)
        {
            Certification cert = CreateBusinessEntity(entity, true);
            Entities.Add(cert);
            DataContext.Entry(cert).State = EntityState.Added;
            DataContext.SaveChanges();
        }

        public void Delete(CertificationDto entity)
        {
            Certification cert = Entities.FirstOrDefault(e => e.CertificationID == entity.CertificationID);
            cert.IsDeleted = true;
            cert.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(cert);
            DataContext.Entry(cert).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public bool Exists(string itemName, int id)
        {
            return Entities.Any(c => c.Name.ToLower() == itemName.ToLower() &&
            c.CertificationID != id && c.IsDeleted == false);
        }

        public bool Exists(string certificateName)
        {
            return Entities.Any(c => c.Name.ToLower() == certificateName.ToLower() && c.IsDeleted == false);
        }

        public bool Exists(int id)
        {
            return Entities.Any(c => c.CertificationID == id && c.IsDeleted == false);

        }

        public IEnumerable<CertificationDto> GetAll(int pageSize = -1, int pageNo = -1)
        {
            IQueryable<CertificationDto> certs = null;
            try
            {
                certs = GetAllCertificates();

                if (pageSize <= 0 || pageNo < 1)
                {
                    return certs;
                }
            }
            catch (Exception)
            {

            }
            return certs?.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public CertificationDto GetByID(int id)
        {
            return GetAllCertificates().FirstOrDefault(s => s.CertificationID == id);
        }

        public CertificationDto GetByName(string name, int certificateID)
        {
            return GetAllCertificates().FirstOrDefault(s => s.CertificationID != certificateID && s.Name.ToLower() == name.ToLower());
        }

        public IEnumerable<CertificationDto> GetAllByTechnologyArea(int areaID, int pageSize = -1, int pageNo = -1)
        {
            IQueryable<CertificationDto> entries = GetAllCertificates().Where(s => s.TechnologyAreaID == areaID);
            if (pageSize <= 0 || pageNo < 1)
            {
                return entries;
            }

            return entries?.Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public int TotalRecordsCountByTechAreaID(int areaID)
        {
            return GetAllCertificates().Count(s => s.TechnologyAreaID == areaID);
        }

        public void Update(CertificationDto entity)
        {
            Certification buzEntity = Entities.FirstOrDefault(e => e.CertificationID == entity.CertificationID);
            MigrateEntity(entity, buzEntity);
            buzEntity.UpdateTimeStamp(entity.LoggedInUserName);
            Entities.Add(buzEntity);
            DataContext.Entry(buzEntity).State = EntityState.Modified;
            DataContext.SaveChanges();
        }

        public override bool CanBeDeleted(int id)
        {
            if (DataContext.EmpCertifications.Any(c => c.IsDeleted == false && c.CertificationID == id))
            {
                return false;
            }

            return true;
        }

        private IQueryable<CertificationDto> GetAllCertificates()
        {
            return (from s in Entities
                    join c in DataContext.DropDownSubCategories on s.TechnologyAreaID equals c.SubCategoryID into ce
                    from cd in ce.DefaultIfEmpty()
                    orderby s.Name
                    where s.IsDeleted == false
                    select new CertificationDto
                    {
                        CertificationID = s.CertificationID,
                        Name = s.Name,
                        TechnologyArea = cd.SubCategoryName,
                        TechnologyAreaID = s.TechnologyAreaID,
                        ShortName = s.ShortName,
                    });
        }

        private Certification CreateBusinessEntity(CertificationDto certDto, bool isNewEntity = false)
        {
            Certification category = new Certification
            {
                CertificationID = certDto.CertificationID,
                Name = certDto.Name,
                TechnologyAreaID = certDto.TechnologyAreaID,
                ShortName = certDto.ShortName,
            };

            category.UpdateTimeStamp(certDto.LoggedInUserName, true);
            return category;
        }

        private void MigrateEntity(CertificationDto sourceEntity, Certification targetEntity)
        {
            targetEntity.Name = sourceEntity.Name;
            targetEntity.ShortName = sourceEntity.ShortName;
            targetEntity.TechnologyAreaID = sourceEntity.TechnologyAreaID;
            targetEntity.CertificationID = sourceEntity.CertificationID;

            targetEntity.UpdateTimeStamp(sourceEntity.LoggedInUserName);
        }
    }

    public interface ICertificationRepository : IRepository<CertificationDto>
    {
        IEnumerable<CertificationDto> GetAllByTechnologyArea(int areaID, int pageSize = -1, int pageNo = -1);

        bool Exists(string certificateName, int id);

        CertificationDto GetByName(string name, int categoryID);

        int TotalRecordsCountByTechAreaID(int areaID);
    }
}

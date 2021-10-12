using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Cloudinary;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicalImageService
    {
        //Task ProcessMedicalImageFile(long patientId, IList<MedicalImageFile> fileImgBase64);
        Task<List<CloudinaryFile>> GetMedicalImageFiles(long patientId);
        Task<List<CloudinaryFile>> SaveChanges(long patientId, IList<CloudinaryFieldDto> cloudinaryFields);
        //Task<PatientMedicalImageFile> EditMedicalImageFile(long id, string name, string description);
        Task Delete(long id);
    }
}

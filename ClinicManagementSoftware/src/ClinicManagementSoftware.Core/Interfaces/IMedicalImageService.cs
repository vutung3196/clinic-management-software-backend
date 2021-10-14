using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Cloudinary;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicalImageService
    {
        Task<IEnumerable<CloudinaryFile>> GetMedicalImageFiles(long labTestId);
        Task<IEnumerable<CloudinaryFile>> GetMedicalImageFilesByVisitingFormId(long visitingFormId);
        Task<List<CloudinaryFile>> SaveChanges(long patientId, IList<CloudinaryFieldDto> cloudinaryFields);
        Task Delete(long id);
    }
}

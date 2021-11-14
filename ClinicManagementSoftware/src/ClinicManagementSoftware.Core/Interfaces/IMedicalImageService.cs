using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Cloudinary;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicalImageService
    {
        Task<IEnumerable<CloudinaryFile>> GetMedicalImageFilesOfALabTest(long labTestId);
        Task<IEnumerable<CloudinaryFile>> GetMedicalImageFilesByVisitingFormId(long visitingFormId);

        Task<List<CloudinaryFile>> CreateFileImagesForLabTest(long patientId,
            IList<CloudinaryFieldDto> cloudinaryFields);

        Task<CloudinaryFile> CreateImageLogoForClinic(CloudinaryFieldDto cloudinaryField);
        Task Delete(long id);
        Task DeleteCloudinaryFile(string cloudinaryFileId);
    }
}
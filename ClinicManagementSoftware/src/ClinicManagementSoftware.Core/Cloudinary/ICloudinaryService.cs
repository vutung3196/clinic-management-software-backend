using System.Collections.Generic;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace ClinicManagementSoftware.Core.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<List<ImageUploadResult>> UploadImages(List<string> filePaths, string accessType, string folder);
        Task<ImageUploadResult> UploadImage(string filePath, string accessType, string folder);
        Task<DeletionResult> DeleteImage(string publicId);
    }
}
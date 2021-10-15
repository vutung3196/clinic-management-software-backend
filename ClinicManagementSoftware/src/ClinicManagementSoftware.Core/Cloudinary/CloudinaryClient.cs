using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace ClinicManagementSoftware.Core.Cloudinary
{
    public class CloudinaryClient
    {
        public CloudinaryDotNet.Cloudinary Instance { get; set; }

        public async Task<ImageUploadResult> UploadAsync(ImageUploadParams uploadParams)
        {
            return await Instance.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult> DeleteAsync(string publicId)
        {
             return await Instance.DestroyAsync(new DeletionParams(publicId));
        }
    }
}
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace ClinicManagementSoftware.Core.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryClient _cloudinaryClient;

        public CloudinaryService(CloudinaryClient cloudinaryClient)
        {
            _cloudinaryClient = cloudinaryClient;
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            return await _cloudinaryClient.DeleteAsync(publicId);
        }
    }
}
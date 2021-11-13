using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace ClinicManagementSoftware.Core.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<DeletionResult> DeleteImage(string publicId);
    }
}
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetImageAndCloudinaryFileSpec : Specification<MedicalImageFile>, ISingleResultSpecification
    {
        public GetImageAndCloudinaryFileSpec(long id)
        {
            Query.Where(@group => @group.Id == id)
                .Include(x => x.CloudinaryFile);
        }
    }
}
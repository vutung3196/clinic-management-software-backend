using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetClinicAndLogoImageBySpec : Specification<Clinic>, ISingleResultSpecification
    {
        public GetClinicAndLogoImageBySpec(long clinicId)
        {
            Query.Include(clinic => clinic.CloudinaryFile)
                .Where(clinic => clinic.Id == clinicId);
        }
    }
}
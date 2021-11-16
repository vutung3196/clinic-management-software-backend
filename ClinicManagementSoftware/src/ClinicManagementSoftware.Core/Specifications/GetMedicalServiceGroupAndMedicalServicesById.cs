using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetMedicalServiceGroupAndMedicalServicesById : Specification<MedicalServiceGroup>,
        ISingleResultSpecification
    {
        public GetMedicalServiceGroupAndMedicalServicesById(long medicalServiceGroupId)
        {
            Query.Where(medicalServiceGroup => medicalServiceGroup.Id == medicalServiceGroupId)
                .Include(x => x.MedicalServices);
        }
    }
}
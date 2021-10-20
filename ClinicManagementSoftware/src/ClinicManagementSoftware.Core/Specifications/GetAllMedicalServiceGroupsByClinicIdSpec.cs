using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllMedicalServiceGroupsByClinicIdSpec : Specification<MedicalServiceGroup>
    {
        public GetAllMedicalServiceGroupsByClinicIdSpec(long clinicId)
        {
            Query.Where(x => x.ClinicId == clinicId)
                .Where(x => x.IsDeleted == false);
        }
    }
}
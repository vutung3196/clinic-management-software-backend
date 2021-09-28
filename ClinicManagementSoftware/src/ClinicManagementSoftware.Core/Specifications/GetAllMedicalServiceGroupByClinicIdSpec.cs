using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllMedicalServiceGroupByClinicIdSpec : Specification<MedicalServiceGroup>
    {
        public GetAllMedicalServiceGroupByClinicIdSpec(long clinicId)
        {
            Query.Where(clinicServiceGroup => clinicServiceGroup.ClinicId == clinicId)
                .Include(x => x.MedicalServices);
        }
    }
}
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientsOfClinicSpec : Specification<Patient>
    {
        public GetPatientsOfClinicSpec(long clinicId)
        {
            Query.Where(patient => patient.ClinicId == clinicId && patient.IsDeleted == 0)
                .OrderByDescending(x => x.CreatedAt)
                .Take(50);
        }
    }
}
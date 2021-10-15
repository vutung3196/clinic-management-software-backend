using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientsByNameOfClinicSpec : Specification<Patient>
    {
        public GetPatientsByNameOfClinicSpec(long clinicId, string searchName)
        {
            Query.Where(patient => patient.ClinicId == clinicId && patient.IsDeleted == 0)
                .Where(x => x.FullName.Contains(searchName));
        }
    }
}
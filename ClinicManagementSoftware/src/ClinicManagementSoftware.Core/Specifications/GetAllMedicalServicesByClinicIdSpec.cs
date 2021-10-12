using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllMedicalServicesByClinicIdSpec : Specification<MedicalService>
    {
        public GetAllMedicalServicesByClinicIdSpec(long clinicId)
        {
            Query.Where(x => x.ClinicId == clinicId)
                .Include(x => x.MedicalServiceGroup);
        }
    }
}
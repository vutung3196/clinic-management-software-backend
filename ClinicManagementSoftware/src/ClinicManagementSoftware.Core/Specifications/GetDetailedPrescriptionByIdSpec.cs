using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDetailedPrescriptionByIdSpec : Specification<Prescription>, ISingleResultSpecification
    {
        public GetDetailedPrescriptionByIdSpec(long id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.PatientHospitalizedProfile.Patient)
                .Include(x => x.Doctor)
                .ThenInclude(x => x.Clinic);
        }
    }
}
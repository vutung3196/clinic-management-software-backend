using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientHospitalizedProfilesByClinicIdSpec : Specification<PatientHospitalizedProfile>
    {
        public GetPatientHospitalizedProfilesByClinicIdSpec(long clinicId)
        {
            Query.Include(profile => profile.Patient)
                .ThenInclude(patient => patient.Clinic)
                .Where(profile => profile.Patient.ClinicId == clinicId);
        }
    }
}
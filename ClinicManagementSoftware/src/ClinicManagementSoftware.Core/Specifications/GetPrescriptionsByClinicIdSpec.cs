using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPrescriptionsByClinicIdSpec : Specification<PatientHospitalizedProfile>
    {
        public GetPrescriptionsByClinicIdSpec(long clinicId)
        {
            Query.Include(patient => patient.Patient)
                .Where(patient => patient.Patient.ClinicId == clinicId)
                .Include(profile => profile.Prescriptions)
                .ThenInclude(prescription => prescription.Doctor);
        }
    }
}
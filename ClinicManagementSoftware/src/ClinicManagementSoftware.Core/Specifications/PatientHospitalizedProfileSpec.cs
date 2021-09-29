using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class PatientHospitalizedProfileSpec : Specification<PatientHospitalizedProfile>
    {
        public PatientHospitalizedProfileSpec(long patientId)
        {
            Query.Where(patientHospitalizedProfile => patientHospitalizedProfile.PatientId == patientId)
                .Include(p => p.Prescriptions)
                .ThenInclude(x => x.Doctor);
        }
    }
}
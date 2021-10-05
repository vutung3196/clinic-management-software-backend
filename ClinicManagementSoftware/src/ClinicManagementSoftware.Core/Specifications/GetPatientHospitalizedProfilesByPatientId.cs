using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientHospitalizedProfilesByPatientId : Specification<PatientHospitalizedProfile>
    {
        public GetPatientHospitalizedProfilesByPatientId(long patientId)
        {
            Query.Where(x => x.PatientId == patientId).Include(x => x.Patient);
        }
    }
}
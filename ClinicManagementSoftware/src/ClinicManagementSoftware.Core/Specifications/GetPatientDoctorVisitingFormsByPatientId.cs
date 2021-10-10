using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientDoctorVisitingFormsByPatientId : Specification<PatientDoctorVisitForm>
    {
        public GetPatientDoctorVisitingFormsByPatientId(long patientId)
        {
            Query.Where(x => x.PatientId == patientId);
        }
    }
}
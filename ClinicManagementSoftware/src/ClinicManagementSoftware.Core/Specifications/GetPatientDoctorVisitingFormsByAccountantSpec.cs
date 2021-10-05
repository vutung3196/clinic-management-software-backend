using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientDoctorVisitingFormsByAccountantSpec : Specification<PatientDoctorVisitForm>
    {
        public GetPatientDoctorVisitingFormsByAccountantSpec(long clinicId)
        {
            Query.Include(x => x.Patient)
                .Where(x => x.Patient.ClinicId == clinicId)
                .Where(x => x.VisitingStatus == (byte) EnumDoctorVisitingFormStatus.WaitingForDoctor);
        }
    }
}
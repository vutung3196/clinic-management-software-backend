using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class
        GetPatientDoctorVisitingFormsForReceptionistFromClinicIdSpec : Specification<PatientDoctorVisitForm>
    {
        public GetPatientDoctorVisitingFormsForReceptionistFromClinicIdSpec(long clinicId)
        {
            Query.Include(x => x.Patient)
                .Include(x => x.Doctor)
                .Where(x => x.Patient.ClinicId == clinicId)
                .Where(x => x.VisitingStatus == (byte) EnumDoctorVisitingFormStatus.WaitingForDoctor)
                .Where(x => x.IsDeleted == false);
        }
    }
}
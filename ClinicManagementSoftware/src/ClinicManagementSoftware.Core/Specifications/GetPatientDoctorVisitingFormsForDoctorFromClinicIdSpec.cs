using System.Linq;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetPatientDoctorVisitingFormsForDoctorFromClinicIdSpec : Specification<PatientDoctorVisitForm>
    {
        public GetPatientDoctorVisitingFormsForDoctorFromClinicIdSpec(long doctorId, long[] ids)
        {
            Query.Include(x => x.Patient)
                .Include(x => x.Doctor)
                .Where(x => x.DoctorId == doctorId)
                .Where(x => ids.Contains(x.Id))
                .Where(x => x.IsDeleted == false)
                .Where(x => x.VisitingStatus != (byte) EnumDoctorVisitingFormStatus.Done);
        }
    }
}
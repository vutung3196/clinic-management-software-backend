using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabOrderFormsForReceptionistSpec : Specification<LabOrderForm>
    {
        public GetLabOrderFormsForReceptionistSpec(long clinicId)
        {
            Query.Include(x => x.PatientHospitalizedProfile.Patient)
                .Include(x => x.Doctor)
                .Include(x => x.LabTests)
                .ThenInclude(x => x.MedicalService)
                .Where(x => x.PatientHospitalizedProfile.Patient.ClinicId == clinicId)
                .Where(x => x.Status != (byte) EnumLabOrderFormStatus.Done)
                .Where(x => x.IsDeleted == false);
        }
    }
}
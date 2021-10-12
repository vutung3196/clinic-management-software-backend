using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabOrderFormsForDoctorSpec : Specification<LabOrderForm>
    {
        public GetLabOrderFormsForDoctorSpec(long clinicId)
        {
            Query.Include(x => x.PatientHospitalizedProfile.Patient)
                .Include(x => x.Doctor)
                .Include(x => x.LabTests)
                .ThenInclude(x => x.MedicalService)
                .Where(x => x.PatientHospitalizedProfile.Patient.ClinicId == clinicId)
                .Where(x => x.IsDeleted == false);
        }
    }
}
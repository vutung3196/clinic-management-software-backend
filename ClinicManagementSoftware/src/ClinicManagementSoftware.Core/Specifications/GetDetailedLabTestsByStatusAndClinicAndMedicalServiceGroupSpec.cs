using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDetailedLabTestsByStatusAndClinicAndMedicalServiceGroupSpec : Specification<LabTest>
    {
        public GetDetailedLabTestsByStatusAndClinicAndMedicalServiceGroupSpec(byte status, long clinicId,
            long medicalServiceGroupForTestSpecialistId)
        {
            Query.Where(x => x.Status == status)
                .Where(x => x.LabOrderForm.PatientHospitalizedProfile.Patient.ClinicId == clinicId)
                .Where(x => x.IsDeleted == false)
                .Where(x => x.MedicalService.MedicalServiceGroupId == medicalServiceGroupForTestSpecialistId)
                .Include(x => x.MedicalService)
                .Include(x => x.MedicalImageFiles)
                .ThenInclude(x => x.CloudinaryFile)
                .Include(x => x.LabOrderForm)
                .Include(x => x.LabOrderForm.Doctor)
                .Include(x => x.LabOrderForm.PatientHospitalizedProfile)
                .ThenInclude(x => x.Patient);
        }
    }
}
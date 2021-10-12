using System.Linq;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDetailedLabTestsByStatusAndClinicSpec : Specification<LabTest>
    {
        public GetDetailedLabTestsByStatusAndClinicSpec(byte status, long clinicId)
        {
            Query.Where(x => x.Status == status)
                .Where(x => x.LabOrderForm.PatientHospitalizedProfile.Patient.ClinicId == clinicId)
                .Where(x => x.IsDeleted == false)
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
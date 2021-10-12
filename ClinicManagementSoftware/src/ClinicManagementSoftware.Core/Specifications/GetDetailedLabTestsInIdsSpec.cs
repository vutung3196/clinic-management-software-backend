using System.Linq;
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDetailedLabTestsInIdsSpec : Specification<LabTest>
    {
        public GetDetailedLabTestsInIdsSpec(long[] ids)
        {
            Query.Where(x => ids.Contains(x.Id))
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
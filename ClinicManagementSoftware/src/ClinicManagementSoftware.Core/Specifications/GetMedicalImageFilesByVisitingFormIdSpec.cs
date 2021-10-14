using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetMedicalImageFilesByVisitingFormIdSpec : Specification<PatientDoctorVisitForm>,
        ISingleResultSpecification
    {
        public GetMedicalImageFilesByVisitingFormIdSpec(long id)
        {
            Query.Where(x => x.Id == id)
                .Where(x => x.IsDeleted == false)
                .Include(x => x.LabOrderForms)
                .ThenInclude(x => x.LabTests)
                .ThenInclude(x => x.MedicalImageFiles)
                .ThenInclude(x => x.CloudinaryFile);
        }
    }
}
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDetailedLabTestByIdSpec : Specification<LabTest>, ISingleResultSpecification
    {
        public GetDetailedLabTestByIdSpec(long id)
        {
            Query.Where(x => x.Id == id)
                .Where(x => x.IsDeleted == false)
                .Include(x => x.MedicalService)
                .Include(x => x.MedicalImageFiles)
                .ThenInclude(x => x.CloudinaryFile)
                .Include(x => x.LabOrderForm);
        }
    }
}
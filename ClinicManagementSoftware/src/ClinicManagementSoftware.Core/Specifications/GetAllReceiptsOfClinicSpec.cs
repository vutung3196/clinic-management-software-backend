using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllReceiptOfClinicSpec : Specification<Receipt>
    {
        public GetAllReceiptOfClinicSpec(long clinicId)
        {
            Query.Include(x => x.Patient)
                .Where(x => x.Patient.ClinicId == clinicId);
        }
    }
}
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabOrderFormAndLabTestsSpec : Specification<LabOrderForm>, ISingleResultSpecification
    {
        public GetLabOrderFormAndLabTestsSpec(long id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.LabTests);
        }
    }
}
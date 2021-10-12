using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetLabTestQueueByClinicIdSpec : Specification<LabTestQueue>, ISingleResultSpecification
    {
        public GetLabTestQueueByClinicIdSpec(long clinicId)
        {
            Query.Where(labTestQueue => labTestQueue.ClinicId == clinicId);
        }
    }
}
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetDoctorQueueByDoctorIdSpec : Specification<VisitingDoctorQueue>, ISingleResultSpecification
    {
        public GetDoctorQueueByDoctorIdSpec(long doctorId)
        {
            Query.Where(doctorQueue => doctorQueue.DoctorId == doctorId);
        }
    }
}
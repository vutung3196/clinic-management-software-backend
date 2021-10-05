using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetAllDoctorQueuesByClinicIdSpec : Specification<VisitingDoctorQueue>
    {
        public GetAllDoctorQueuesByClinicIdSpec(long clinicId)
        {
            Query.Include(doctorQueue => doctorQueue.Doctor)
                .Where(doctorQueue => doctorQueue.Doctor.ClinicId == clinicId);
        }
    }
}
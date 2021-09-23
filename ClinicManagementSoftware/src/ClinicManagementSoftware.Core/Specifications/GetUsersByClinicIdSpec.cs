using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetUsersByClinicIdSpec : Specification<User>
    {
        public GetUsersByClinicIdSpec(long clinicId)
        {
            Query.Where(x => x.ClinicId == clinicId)
                .Include(x => x.Role);
        }
    }
}
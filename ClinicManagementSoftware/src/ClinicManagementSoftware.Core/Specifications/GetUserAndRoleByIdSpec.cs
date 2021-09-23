using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetUserAndRoleByIdSpec : Specification<User>, ISingleResultSpecification
    {
        public GetUserAndRoleByIdSpec(long id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Role);
        }
    }
}
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetRoleByRoleNameSpec : Specification<Role>, ISingleResultSpecification
    {
        public GetRoleByRoleNameSpec(string roleName)
        {
            Query.Where(x => x.RoleName == roleName);
        }
    }
}
using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetUserRoleAndClinicByUsernameSpec : Specification<User>, ISingleResultSpecification
    {
        public GetUserRoleAndClinicByUsernameSpec(string userName)
        {
            Query.Where(x => x.Username == userName)
                .Include(x => x.Role)
                .Include(x => x.Clinic);
        }
    }
}
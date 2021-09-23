using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetClinicsAndAdminSpec : Specification<Clinic>
    {
        public GetClinicsAndAdminSpec()
        {
            Query.Include(x => x.Users)
                .ThenInclude(x => x.Role);
        }
    }
}
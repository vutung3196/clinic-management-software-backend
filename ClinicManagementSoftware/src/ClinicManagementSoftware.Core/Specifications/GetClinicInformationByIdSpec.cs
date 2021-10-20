using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetClinicInformationByIdSpec : Specification<Clinic>, ISingleResultSpecification
    {
        public GetClinicInformationByIdSpec(long id)
        {
            Query.Where(x => x.Id == id);
        }
    }
}
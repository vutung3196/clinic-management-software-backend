using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetMailTemplateByNameSpec : Specification<MailTemplate>, ISingleResultSpecification
    {
        public GetMailTemplateByNameSpec(string type)
        {
            Query.Where(x => x.Name.Equals(type));
        }
    }
}
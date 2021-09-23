using Ardalis.Specification;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Specifications
{
    public sealed class GetRefreshTokenByTokenStringSpec : Specification<RefreshToken>, ISingleResultSpecification
    {
        public GetRefreshTokenByTokenStringSpec(string tokenString)
        {
            Query.Where(x => x.TokenString == tokenString);
        }
    }
}
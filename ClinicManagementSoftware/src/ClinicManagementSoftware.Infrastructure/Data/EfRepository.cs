using Ardalis.Specification.EntityFrameworkCore;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Infrastructure.Data
{
    // inherit from Ardalis.Specification type
    public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(ClinicManagementSoftwareDbContext dbContext) : base(dbContext)
        {
        }
    }
}

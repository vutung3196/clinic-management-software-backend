using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.EFCore.Extensions;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClinicManagementSoftware.Infrastructure.Data
{
    public class ClinicManagementSoftwareDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private static readonly MySqlServerVersion MySqlServerVersion = new(new Version(8, 0, 21));

        public ClinicManagementSoftwareDbContext(DbContextOptions<ClinicManagementSoftwareDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Clinic> Clinics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString(ConfigurationConstant.ClinicManagementSoftwareDatabase);
            optionsBuilder.UseMySql(connectionString, MySqlServerVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
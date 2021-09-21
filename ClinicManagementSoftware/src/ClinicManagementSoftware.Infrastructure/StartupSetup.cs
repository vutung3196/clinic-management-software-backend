using System;
using ClinicManagementSoftware.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagementSoftware.Infrastructure
{
    public static class StartupSetup
    {
        private static readonly MySqlServerVersion SqlServerVersion = new(new Version(8, 0, 21));

        public static void AddAffordableClinicDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<ClinicManagementSoftwareDbContext>(options => options.UseMySql(connectionString, SqlServerVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
    }
}

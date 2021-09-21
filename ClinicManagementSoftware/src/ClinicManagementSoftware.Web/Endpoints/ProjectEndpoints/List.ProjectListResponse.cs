using ClinicManagementSoftware.Core.ProjectAggregate;
using System.Collections.Generic;

namespace ClinicManagementSoftware.Web.Endpoints.ProjectEndpoints
{
    public class ProjectListResponse
    {
        public List<ProjectRecord> Projects { get; set; } = new();
    }
}

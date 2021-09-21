using System.Collections.Generic;

namespace ClinicManagementSoftware.Web.Endpoints.ProjectEndpoints
{
    public class GetProjectByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<ToDoItemRecord> Items { get; set; } = new();
    }
}
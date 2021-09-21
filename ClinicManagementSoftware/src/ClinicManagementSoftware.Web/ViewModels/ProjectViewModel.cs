using System.Collections.Generic;

namespace ClinicManagementSoftware.Web.ViewModels
{
    public class ProjectViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<ToDoItemViewModel> Items = new();
    }
}

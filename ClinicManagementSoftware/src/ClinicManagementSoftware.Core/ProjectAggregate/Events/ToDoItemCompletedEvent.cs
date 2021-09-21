using ClinicManagementSoftware.Core.ProjectAggregate;
using ClinicManagementSoftware.SharedKernel;

namespace ClinicManagementSoftware.Core.ProjectAggregate.Events
{
    public class ToDoItemCompletedEvent : BaseDomainEvent
    {
        public ToDoItem CompletedItem { get; set; }

        public ToDoItemCompletedEvent(ToDoItem completedItem)
        {
            CompletedItem = completedItem;
        }
    }
}
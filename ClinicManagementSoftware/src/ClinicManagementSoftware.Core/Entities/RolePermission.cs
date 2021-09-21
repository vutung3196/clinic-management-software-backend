using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("role_permission")]
    public class RolePermission : BaseEntity, IAggregateRoot
    {
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
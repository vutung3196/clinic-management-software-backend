using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("role")]
    public class Role : BaseEntity, IAggregateRoot
    {
        [Column("role_name")]
        public string RoleName { get; set; }

        public IList<User> Users { get; set; }
        public IList<RolePermission> RolePermissions { get; set; }
    }
}
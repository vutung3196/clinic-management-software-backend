using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("permission")]
    public class Permission : BaseEntity, IAggregateRoot
    {
        [Column("permission_screen_name")] public string PermissionScreenName { get; set; }
        public IList<RolePermission> RolePermissions { get; set; }
    }
}
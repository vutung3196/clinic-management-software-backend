using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("clinic")]
    public class Clinic : BaseEntity, IAggregateRoot
    {
        [Column("name")] public string Name { get; set; }

        [Column("address")] public string Address { get; set; }

        [Column("description")] public string Description { get; set; }

        [Column("is_enabled")] public byte IsEnabled { get; set; }

        //public IList<User> Users { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("lab_test_queue")]
    public class LabTestQueue : BaseEntity, IAggregateRoot
    {
        // foreign keys
        [Column("clinic_id")] public long ClinicId { get; set; }
        public Clinic Clinic { get; set; }

        [Column("queue")] public string Queue { get; set; }
    }
}
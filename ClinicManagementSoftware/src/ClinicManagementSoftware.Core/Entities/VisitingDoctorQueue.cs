using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("visiting_doctor_queue")]
    public class VisitingDoctorQueue : BaseEntity, IAggregateRoot
    {
        // foreign keys
        [Column("doctor_id")] public long DoctorId { get; set; }
        public User Doctor { get; set; }
        
        [Column("queue")]
        public string Queue { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [Column("is_deleted")]
        public byte? IsDeleted { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("patient_doctor_visit_form")]
    public class PatientDoctorVisitForm : BaseEntity, IAggregateRoot
    {
        [Column("patient_id")] public long PatientId { get; set; }

        [Column("doctor_id")] public long DoctorId { get; set; }

        [Column("description")] public string Description { get; set; }
        [Column("visiting_status")] public byte VisitingStatus { get; set; }
        [Column("code")] public string Code { get; set; }
        [Column("is_deleted")] public bool IsDeleted { get; set; }

        [Column("deleted_at")] public DateTime? DeletedAt { get; set; }

        public Patient Patient { get; set; }
        public Receipt Receipt { get; set; }
        public User Doctor { get; set; }
    }
}
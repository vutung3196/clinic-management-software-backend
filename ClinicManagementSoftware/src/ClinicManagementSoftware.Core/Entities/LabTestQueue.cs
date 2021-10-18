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

        [Column("medical_service_group_for_test_specialist_id")]
        public long? MedicalServiceGroupForTestSpecialistId { get; set; }

        public MedicalServiceGroup MedicalServiceGroupForTestSpecialist { get; set; }

        [Column("queue")] public string Queue { get; set; }
    }
}
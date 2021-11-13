using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("patient_hospitalized_profile")]
    public class PatientHospitalizedProfile : BaseEntity, IAggregateRoot
    {
        [Column("patient_id")] public long PatientId { get; set; }
        public Patient Patient { get; set; }

        [Column("disease_name")] public string DiseaseName { get; set; }
        [Column("description")] public string Description { get; set; }

        [Column("revisit_date")] public DateTime? RevisitDate { get; set; }

        [Column("deleted_at")] public DateTime? DeletedAt { get; set; }
        [Column("code")] public string Code { get; set; }

        [Column("is_deleted")] public bool IsDeleted { get; set; }

        // 1 to many
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<LabOrderForm> LabOrderForms { get; set; }
    }
}
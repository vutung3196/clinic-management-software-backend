﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("medical_service_group")]
    public class MedicalServiceGroup : BaseEntity, IAggregateRoot
    {
        // foreign key
        [Column("clinic_id")] public long ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("description")] public string Description { get; set; }
        public ICollection<MedicalService> MedicalServices { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
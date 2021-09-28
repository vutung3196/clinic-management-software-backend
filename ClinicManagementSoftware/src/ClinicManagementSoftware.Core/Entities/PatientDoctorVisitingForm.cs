﻿using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("patient_doctor_visiting_form")]
    public class PatientDoctorVisitingForm : BaseEntity, IAggregateRoot
    {
        [Column("patient_id")] public long PatientId { get; set; }

        [Column("doctor_id")] public long? DoctorId { get; set; }

        [Column("description")] public string Description { get; set; }
        [Column("visiting_status")] public byte VisitingStatus { get; set; }
        [Column("phone_number")] public string PhoneNumber { get; set; }

        public Patient Patient { get; set; }
        public User Doctor { get; set; }
    }
}
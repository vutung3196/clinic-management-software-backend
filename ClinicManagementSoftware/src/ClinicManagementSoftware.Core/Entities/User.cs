using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("user")]
    public class User : BaseEntity, IAggregateRoot
    {
        [Column("user_name")] public string Username { get; set; }
        [Column("password_hash")] public string Password { get; set; }
        [Column("full_name")] public string FullName { get; set; }
        [Column("is_enabled")] public byte Enabled { get; set; }
        [Column("email_address")] public string EmailAddress { get; set; }

        [Column("role_id")] public long RoleId { get; set; }
        [Column("clinic_id")] public long ClinicId { get; set; }
        [Column("phone_number")] public string PhoneNumber { get; set; }

        [Column("medical_service_group_for_test_specialist_id")]
        public long? MedicalServiceGroupForTestSpecialistId { get; set; }

        public MedicalServiceGroup MedicalServiceGroupForTestSpecialist { get; set; }

        public Clinic Clinic { get; set; }

        public Role Role { get; set; }

        // FOR DOCTOR ROLE
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<VisitingDoctorQueue> VisitingDoctorQueues { get; set; }
    }
}
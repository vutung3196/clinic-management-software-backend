using System.Collections.Generic;
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
        [Column("is_enabled")] public byte IsEnabled { get; set; }
        [Column("phone_number")] public string PhoneNumber { get; set; }
        [Column("first_time_registration")] public bool? FirstTimeRegistration { get; set; }
        public IList<User> Users { get; set; }
        public IList<Patient> Patients { get; set; }
        public IList<MedicalService> MedicalServices { get; set; }
        public IList<LabTestQueue> LabTestQueues { get; set; }
        public IList<MedicalServiceGroup> MedicalServiceGroups { get; set; }
        [Column("email_address")] public string EmailAddress { get; set; }
    }
}
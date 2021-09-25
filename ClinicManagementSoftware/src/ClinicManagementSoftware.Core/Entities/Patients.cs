using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("patient")]
    public class Patient : BaseEntity, IAggregateRoot
    {
        [Column("clinic_id")] public long ClinicId { get; set; }

        [Column("full_name")] public string FullName { get; set; }

        [Column("email_address")] public string EmailAddress { get; set; }

        [Column("phone_number")] public string PhoneNumber { get; set; }

        [Column("occupation")] public string Occupation { get; set; }

        [Column("address")] public string Address { get; set; }

        [Column("gender")] public byte? Gender { get; set; }
        [Column("year_of_birth")] public int? YearOfBirth { get; set; }

        [Column("is_deleted")] public byte IsDeleted { get; set; }

        [Column("deleted_at")] public DateTime? DeletedAt { get; set; }

        public Clinic Clinic { get; set; }
    }
}
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
        [Column("address_detail")] public string AddressDetail { get; set; }

        [Column("gender")] public byte? Gender { get; set; }
        [Column("date_of_birth")] public DateTime? DateOfBirth { get; set; }

        [Column("is_deleted")] public byte IsDeleted { get; set; }

        [Column("deleted_at")] public DateTime? DeletedAt { get; set; }

        [Column("medical_insurance_code")] public string MedicalInsuranceCode { get; set; }
        [Column("address_street")] public string AddressStreet { get; set; }
        [Column("address_district")] public string AddressDistrict { get; set; }
        [Column("address_city")] public string AddressCity { get; set; }


        public Clinic Clinic { get; set; }
        public ICollection<PatientHospitalizedProfile> PatientHospitalizedProfiles { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}
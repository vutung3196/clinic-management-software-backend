using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.Patient
{
    public class CreatePatientDto
    {
        public CreatePatientDto()
        {
        }

        public long ClinicId { get; set; }

        [Required]
        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public byte IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        public DateTime? ActiveDate { get; set; }
        public string AddressDetail { get; set; }
        public string AddressCity { get; set; }
        public string AddressStreet { get; set; }
        public string AddressDistrict { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MedicalInsuranceCode { get; set; }
    }
}

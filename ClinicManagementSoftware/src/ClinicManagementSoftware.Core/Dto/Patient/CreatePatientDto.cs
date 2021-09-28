using System;

namespace ClinicManagementSoftware.Core.Dto.Patient
{
    public class CreatePatientDto
    {
        public CreatePatientDto()
        {
        }

        public long ClinicId { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Occupation { get; set; }

        public string Gender { get; set; }

        public byte IsDeleted { get; set; }

        public string PatientCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}

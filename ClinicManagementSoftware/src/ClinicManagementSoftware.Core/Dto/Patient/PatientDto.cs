using System;

namespace ClinicManagementSoftware.Core.Dto.Patient
{
    public class PatientDto
    {
        public long Id { get; set; }
        public long ClinicId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public string Gender { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        //public int? Age => DateOfBirth.HasValue ? DateTime.Now.Year - DateOfBirth : null;
    }
}
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class CreateUserDto
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Enabled { get; set; }
        public string Role { get; set; }
        public long? MedicalServiceGroupForTestSpecialistId { get; set; }
    }
}
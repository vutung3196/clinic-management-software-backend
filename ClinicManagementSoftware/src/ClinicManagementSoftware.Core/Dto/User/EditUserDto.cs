using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class EditUserDto
    {
        public string FirstName { get; set; }
        [Required] public string Password { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool Enabled { get; set; }
        public string Role { get; set; }
    }
}
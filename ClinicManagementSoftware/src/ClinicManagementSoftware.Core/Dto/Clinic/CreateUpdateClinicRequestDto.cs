using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.Clinic
{
    public class CreateUpdateClinicRequestDto
    {
        [Required] public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required] [MaxLength(75)] public string UserName { get; set; }
        [Required] [MaxLength(75)] public string Password { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
    }
}
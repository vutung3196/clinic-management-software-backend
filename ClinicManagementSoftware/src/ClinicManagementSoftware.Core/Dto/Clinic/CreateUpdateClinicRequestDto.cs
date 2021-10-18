using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.Clinic
{
    public class CreateUpdateClinicRequestDto
    {
        [Required] public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required] public string EmailAddress { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Tên tài khoản phải có tối thiểu 6 ký tự")]
        [MaxLength(75, ErrorMessage = "Tên tài khoản không vượt quá 75 ký tự")]
        public string UserName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có tối thiểu 6 ký tự")]
        [MaxLength(75, ErrorMessage = "Mật khẩu không vượt quá 75 ký tự")]
        public string Password { get; set; }

        public string Description { get; set; }
        public bool Enabled { get; set; }
    }
}
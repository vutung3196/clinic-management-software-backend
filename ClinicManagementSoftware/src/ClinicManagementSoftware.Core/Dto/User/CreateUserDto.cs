using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class CreateUserDto
    {
        [Required]
        [MinLength(6, ErrorMessage = "Tên tài khoản phải có tối thiểu 6 ký tự")]
        [MaxLength(75, ErrorMessage = "Tên tài khoản không vượt quá 75 ký tự")]
        public string UserName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có tối thiểu 6 ký tự")]
        [MaxLength(75, ErrorMessage = "Mật khẩu không vượt quá 75 ký tự")]
        public string Password { get; set; }

        [MaxLength(70, ErrorMessage = "Tên bệnh nhân không vượt quá 70 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [MaxLength(62, ErrorMessage = "Email không vượt quá 62 ký tự")]
        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
        public bool Enabled { get; set; }
        public string Role { get; set; }
        public long? MedicalServiceGroupForTestSpecialistId { get; set; }
    }
}
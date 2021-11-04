using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.Clinic
{
    public class CreateUpdateClinicRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Tên phòng khám không vượt quá {1} ký tự")]
        public string Name { get; set; }

        [MaxLength(12, ErrorMessage = "Số điện thoại không vượt quá 11 ký tự")]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(62, ErrorMessage = "Email không vượt quá 62 ký tự")]
        public string EmailAddress { get; set; }

        //[Required]
        [MinLength(6, ErrorMessage = "Tên tài khoản phải có tối thiểu 6 ký tự")]
        [MaxLength(75, ErrorMessage = "Tên tài khoản không vượt quá 75 ký tự")]
        public string UserName { get; set; }

        //[Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có tối thiểu 6 ký tự")]
        [MaxLength(75, ErrorMessage = "Mật khẩu không vượt quá 75 ký tự")]
        public string Password { get; set; }

        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool FirstTimeRegistration { get; set; }

        [MaxLength(30, ErrorMessage = "Tên phố không vượt quá 30 ký tự")]
        public string AddressStreet { get; set; }

        public string AddressDistrict { get; set; }
        public string AddressCity { get; set; }

        [MaxLength(20, ErrorMessage = "Thông tin địa chỉ chi tiết không vượt quá 15 ký tự")]
        public string AddressDetail { get; set; }
    }
}
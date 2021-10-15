using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Web.ApiModels.Patient
{
    public class CreateOrUpdatePatientModel
    {
        [Required(ErrorMessage = "Tên phải bắt buộc")]
        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(12, ErrorMessage = "Số điện thoại không vượt quá 11 ký tự")]
        public string PhoneNumber { get; set; }

        public string AddressCity { get; set; }

        [MaxLength(30, ErrorMessage = "Tên phố không vượt quá 30 ký tự")]
        public string AddressStreet { get; set; }

        public string AddressDistrict { get; set; }

        [MaxLength(15, ErrorMessage = "Mã số thẻ bảo hiểm y tế không được vượt quá 15 ký tự")]
        public string MedicalInsuranceCode { get; set; }

        public string Gender { get; set; }

        [MaxLength(20, ErrorMessage = "Số nhà không vượt quá 15 ký tự")]
        public string AddressDetail { get; set; }
    }
}
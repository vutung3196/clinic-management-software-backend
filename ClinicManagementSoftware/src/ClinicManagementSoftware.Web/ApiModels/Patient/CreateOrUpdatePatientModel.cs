using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Web.ApiModels.Patient
{
    public class CreateOrUpdatePatientModel
    {
        public CreateOrUpdatePatientModel()
        {
        }

        [Required(ErrorMessage = "Tên phải bắt buộc")] public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Vui lòng nhập email đúng định dạng")] public string EmailAddress { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Occupation { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }
    }
}
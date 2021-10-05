using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Web.ApiModels.Patient
{
    public class CreateOrUpdatePatientModel
    {
        public CreateOrUpdatePatientModel()
        {
        }

        [Required(ErrorMessage = "Tên phải bắt buộc")]
        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }
        public string AddressCity { get; set; }
        public string AddressStreet { get; set; }
        public string AddressDistrict { get; set; }
        public string MedicalInsuranceCode { get; set; }
        public string Gender { get; set; }
        public string AddressDetail { get; set; }
    }
}
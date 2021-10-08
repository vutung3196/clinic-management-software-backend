using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm
{
    public class CreateOrUpdatePatientDoctorVisitingFormDto
    {
        public string VisitingFormCode { get; set; }

        [Required(ErrorMessage = "Lý do khám là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Miêu tả không được vượt quá 100 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cần phải chọn bác sĩ")]
        public long DoctorId { get; set; }

        [MaxLength(100, ErrorMessage = "Ghi chú thanh toán không được vượt quá 100 ký tự")]
        public string PaymentDescription { get; set; }
        public long PatientId { get; set; }
        public string PaymentCode { get; set; }
        public bool ChangeStatusFromWaitingForDoctorToVisitingDoctor { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm
{
    public class CreateOrUpdatePatientDoctorVisitingFormDto
    {
        public string VisitingFormCode { get; set; }
        [Required] public string Description { get; set; }
        [Required] public long DoctorId { get; set; }
        public string PaymentDescription { get; set; }
        public long PatientId { get; set; }
        public string PaymentCode { get; set; }
    }
}
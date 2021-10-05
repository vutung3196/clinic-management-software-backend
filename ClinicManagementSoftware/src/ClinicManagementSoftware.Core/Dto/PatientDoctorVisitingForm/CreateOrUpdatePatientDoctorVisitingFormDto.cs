namespace ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm
{
    public class CreateOrUpdatePatientDoctorVisitingFormDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public long DoctorId { get; set; }
        public string PaymentDescription { get; set; }
        public long PatientId { get; set; }
        public string PaymentCode { get; set; }
    }
}
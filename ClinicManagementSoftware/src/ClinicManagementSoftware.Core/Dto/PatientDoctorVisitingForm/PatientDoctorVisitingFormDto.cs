namespace ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm
{
    public class PatientDoctorVisitingFormDto
    {
        public long Id { get; set; }
        public string Code => "PK" + Id;
        public string Description { get; set; }
        public byte VisitingStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string PatientInformation { get; set; }
        public string Doctor { get; set; }
    }
}
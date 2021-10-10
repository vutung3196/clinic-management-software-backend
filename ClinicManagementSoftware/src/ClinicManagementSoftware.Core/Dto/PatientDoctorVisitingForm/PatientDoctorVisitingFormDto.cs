using System;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;

namespace ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm
{
    public class PatientDoctorVisitingFormDto
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public byte VisitingStatus { get; set; }
        public string VisitingStatusDisplayed { get; set; }
        public string PatientDetailedInformation => PatientInformation.FullName + "-" + PatientInformation.Gender +
                                                    "-" + PatientInformation.Age + " tuổi";
        public string UpdatedAt { get; set; }
        public PatientDto PatientInformation { get; set; }
        public string DoctorName { get; set; }
        public long? DoctorId { get; set; }
        public ClinicInformationResponse ClinicInformation { get; set; }
    }
}
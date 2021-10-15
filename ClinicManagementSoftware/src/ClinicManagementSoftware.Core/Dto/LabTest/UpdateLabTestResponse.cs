using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Files;
using ClinicManagementSoftware.Core.Dto.Patient;

namespace ClinicManagementSoftware.Core.Dto.LabTest
{
    public class LabTestDto
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }
        public long LabOrderFormId { get; set; }
        public string LabOrderFormCode { get; set; }
        public string Description { get; set; }
        public string MedicalServiceName { get; set; }
        public string DoctorVisitingFormCode { get; set; }
        public string StatusDisplayed { get; set; }
        public byte Status { get; set; }
        public long DoctorVisitingFormId { get; set; }

        public string PatientDetailedInformation => PatientInformation.FullName + "-" + PatientInformation.Gender +
                                                    "-" + PatientInformation.Age + " tuổi";
        public string Result { get; set; }
        public IEnumerable<ImageFileResponse> ImageFiles { get; set; }
        public PatientDto PatientInformation { get; set; }
        public ClinicInformationResponse ClinicInformation { get; set; }
        public string DoctorName { get; set; }
        public int Index { get; set; }
    }
}
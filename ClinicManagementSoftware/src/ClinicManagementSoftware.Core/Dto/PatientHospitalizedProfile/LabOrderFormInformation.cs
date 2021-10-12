namespace ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile
{
    public class LabOrderFormInformation
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DoctorVisitingFormCode { get; set; }
        public long DoctorVisitingFormId { get; set; }
    }
}
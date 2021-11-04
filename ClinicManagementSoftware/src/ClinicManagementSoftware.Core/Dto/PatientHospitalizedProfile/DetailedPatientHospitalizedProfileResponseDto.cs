using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Prescription;

namespace ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile
{
    public class DetailedPatientHospitalizedProfileResponseDto
    {
        // prescription history
        public IEnumerable<PrescriptionInformation> Prescriptions { get; set; }

        // lab order form history
        public IEnumerable<LabOrderFormInformation> LabOrderForms { get; set; }

        // lab test 
        public IEnumerable<LabTestInformation> LabTests { get; set; }

        public string DiseaseName { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
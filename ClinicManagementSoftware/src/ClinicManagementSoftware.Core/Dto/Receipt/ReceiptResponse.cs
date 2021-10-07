using System;
using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;

namespace ClinicManagementSoftware.Core.Dto.Receipt
{
    public class ReceiptResponse
    {
        public long Id { get; set; }
        public PatientDto PatientInformation { get; set; }
        public ClinicInformationResponse ClinicInformation { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public string TotalDisplayed => $"{Total:n0}";
        public string TotalInText { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public ICollection<ReceiptMedicalServiceDto> MedicalServices { get; set; }
    }
}
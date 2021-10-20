using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.Patient;
using ClinicManagementSoftware.Core.Helpers;

namespace ClinicManagementSoftware.Core.Dto.Receipt
{
    public class ReceiptReportResponse
    {
        public bool ContainingPatientName { get; set; }
        public bool ContainingPatientAge { get; set; }
        public bool ContainingPatientEmail { get; set; }
        public bool ContainingPatientAddress { get; set; }
        public bool ContainingPatientPhoneNumber { get; set; }
        public ClinicInformationResponse ClinicInformation { get; set; }
        public double Total { get; set; }
        public string TotalDisplayed => $"{Total:n0}";
        public string TotalInText => Total.ConvertToText();
        public List<ReceiptReportMedicalServiceDto> MedicalServices { get; set; }
    }

    public class ReceiptReportMedicalServiceDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double BasePrice { get; set; }
        public double Total => BasePrice * Quantity;
        public string TotalDisplayed => $"{Total:n0}";
        public string Description { get; set; }
        public long Id { get; set; }
        public string ReceiptCode { get; set; }
        public string CreatedAt { get; set; }
        public PatientDto PatientInformation { get; set; }


    }
}
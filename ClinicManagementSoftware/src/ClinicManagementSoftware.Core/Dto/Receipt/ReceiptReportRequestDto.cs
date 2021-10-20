using System;

namespace ClinicManagementSoftware.Core.Dto.Receipt
{
    public class ReceiptReportRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ContainingPatientName { get; set; }
        public bool ContainingPatientAge { get; set; }
        public bool ContainingPatientEmail { get; set; }
        public bool ContainingPatientAddress { get; set; }
        public bool ContainingPatientPhoneNumber { get; set; }
    }
}
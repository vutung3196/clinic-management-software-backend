using System;
using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.MedicalService;

namespace ClinicManagementSoftware.Core.Dto.Receipt
{
    public class ReceiptResponse
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }   
        public string Type { get; set; }
        public string PayerName { get; set; }
        public double Total { get; set; }
        public IEnumerable<MedicalServiceDto> MedicalServices { get; set; }
        public string ReceiptDate { get; set; }
        public DateTime? ReceiptDateTime { get; set; }
        public string Code { get; set; }
    }
}
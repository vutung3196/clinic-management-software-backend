using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClinicManagementSoftware.Core.Dto.Receipt;

namespace ClinicManagementSoftware.Core.Dto.LabOrderForm
{
    public class CreatePaymentForLabOrderFormDto
    {
        [MaxLength(100, ErrorMessage = "Ghi chú thanh toán không được vượt quá 100 ký tự")]
        public string PaymentDescription { get; set; }
        public string PaymentCode { get; set; }
        public IList<ReceiptMedicalServiceDto> LabTests { get; set; }
        public double Total { get; set; }
    }
}
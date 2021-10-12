using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Receipt;

namespace ClinicManagementSoftware.Core.Dto.LabOrderForm
{
    public class CreatePaymentForLabOrderFormDto
    {
        public string PaymentDescription { get; set; }
        public string PaymentCode { get; set; }
        public IList<ReceiptMedicalServiceDto> LabTests { get; set; }
        public double Total { get; set; }
    }
}
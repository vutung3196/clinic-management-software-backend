using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.Receipt
{
    public class CreateReceiptRequestDto
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string PayerName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập tổng tiền lớn hơn {1}")]
        public double Total { get; set; }

        public string Description { get; set; }
        public long ClinicId { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}
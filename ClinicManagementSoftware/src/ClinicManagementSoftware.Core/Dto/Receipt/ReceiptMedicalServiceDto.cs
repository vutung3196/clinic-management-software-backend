namespace ClinicManagementSoftware.Core.Dto.Receipt
{
    public class ReceiptMedicalServiceDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double BasePrice { get; set; }
        public double Total => BasePrice * Quantity;
    }
}
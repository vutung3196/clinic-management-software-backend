namespace ClinicManagementSoftware.Core.Dto.FinancialReport
{
    public class ReceiptByDayInformation
    {
        public string Date { get; set; }
        public double TotalReceiptAmount { get; set; }
        public string MedicalServiceName { get; set; }
        public double TotalReceiptInDay { get; set; }
        public string TotalReceiptAmountDisplayed => $"{TotalReceiptAmount:n0}";
    }
}
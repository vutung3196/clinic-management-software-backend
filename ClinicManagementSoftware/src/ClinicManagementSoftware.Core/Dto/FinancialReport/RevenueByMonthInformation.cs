namespace ClinicManagementSoftware.Core.Dto.FinancialReport
{
    public class RevenueByMonthInformation
    {
        public string Date { get; set; }
        public double TotalReceiptAmount { get; set; }
        public double TotalSpendingAmount { get; set; }
        public double TotalBalanceAmount => TotalReceiptAmount - TotalSpendingAmount;
        public string TotalReceiptAmountDisplayed => $"{TotalReceiptAmount:n0}";
        public string TotalSpendingAmountDisplayed => $"{TotalSpendingAmount:n0}";
        public string TotalBalanceAmountDisplayed => $"{TotalBalanceAmount:n0}";
        public int NumberOfPrescriptions { get; set; }
        public double TotalPrescriptionAmount { get; set; }
        public string TotalPrescriptionAmountDisplayed => $"{TotalPrescriptionAmount:n0}";
    }
}
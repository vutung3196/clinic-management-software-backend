namespace ClinicManagementSoftware.Core.Dto.FinancialReport
{
    public class MedicalServiceReport
    {
        public long Id { get; set; }
        public string MedicalServiceName { get; set; }
        public int Number { get; set; }
        public double BasePrice { get; set; }
        public double TotalRevenue { get; set; }
    }
}
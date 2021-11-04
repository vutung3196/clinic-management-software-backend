using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.FinancialReport
{
    public class DashboardReportResponse
    {
        public int TotalPatients { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalBills { get; set; }
        public int TotalReceipts { get; set; }
        public IEnumerable<int> NumberOfPatientsByMonthInformation { get; set; }
        public IEnumerable<RevenueByMonthInformation> RevenueByMonthInformation { get; set; }
    }
}
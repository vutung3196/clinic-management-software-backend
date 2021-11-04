using System;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.FinancialReport;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IClinicReportService
    {
        Task<FinancialReportResponse> Get(DateTime startDate, DateTime endDate);
        //Task<DashboardReportResponse> GetDashBoardReport(long clinicId);
    }
}

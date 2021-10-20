using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Receipt;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IReceiptService
    {
        Task<ReceiptResponse> GetReceiptById(long id);
        Task<long> CreateReceipt(CreateReceiptDto createReceiptDto);
        Task Delete(long id);
        Task<IEnumerable<ReceiptResponse>> GetAll();
        Task<ReceiptReportResponse> GetReceiptReport(ReceiptReportRequestDto request);
    }
}
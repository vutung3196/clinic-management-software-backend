using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.LabOrderForm;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface ILabOrderFormService
    {
        Task EditLabOrderForm(long id, CreateOrEditLabOrderFormDto request);
        Task<ClinicInformationResponse> GetLabOrderForm(long id);
        Task<ClinicInformationResponse> GetAll();
        Task CreateLabOrderForm(CreateOrEditLabOrderFormDto request);
        Task DeleteLabOrderForm(long id);
        Task PayLabOrderForm(long id);
    }
}
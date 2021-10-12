using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.LabOrderForm;
using ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface ILabOrderFormService
    {
        Task EditLabOrderForm(long id, CreateOrEditLabOrderFormDto request);
        Task<long> PayLabOrderForm(long id, CreatePaymentForLabOrderFormDto request);
        Task<LabOrderFormDto> GetLabOrderFormById(long id);
        Task<ClinicInformationResponse> GetAll();
        Task CreateLabOrderForm(CreateOrEditLabOrderFormDto request);
        Task DeleteLabOrderForm(long id);
        Task<IEnumerable<LabOrderFormDto>> GetAllByRole();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Dto.Receipt;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IMedicalServiceService
    {
        Task<ReceiptMedicalServiceDto> GetDoctorVisitingFormMedicalService();
        Task<IEnumerable<MedicalServiceDto>> GetAllMedicalServices();
        Task<MedicalServiceDto> CreateMedicalService(MedicalServiceDto request);
        Task<MedicalServiceDto> EditMedicalService(long id, MedicalServiceDto request);
        Task DeleteMedicalService(long id);
    }
}
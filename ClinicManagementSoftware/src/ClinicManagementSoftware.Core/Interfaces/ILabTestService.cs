using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.LabTest;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface ILabTestService
    {
        Task<LabTestDto> GetLabTestById(long id);
        Task<IEnumerable<LabTestDto>> GetAllByRole();
        Task<UpdateLabTestResponse> Edit(long id, EditLabTestDto request);
        Task<IEnumerable<LabTestDto>> GetLabTestsByStatus(byte status);
        Task MoveALabTestToTheEndOfAQueue(long labTestId);
        Task MoveALabTestToTheBeginningOfAQueue(long labTestId);
    }
}
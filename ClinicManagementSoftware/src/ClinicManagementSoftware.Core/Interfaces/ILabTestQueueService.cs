using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface ILabTestQueueService
    {
        Task EnqueueNewLabTestForMedicalServiceGroup(long[] labTestIds, long medicalServiceGroupId, long clinicId);
        Task<Queue<long>> GetCurrentLabTestQueue(long clinicId, long medicalServiceGroupId);
        Task DeleteALabTestInQueue(long labTestId, long clinicId, long medicalServiceGroupId);
        Task CreateNewLabTestQueue(long clinicId, long medicalServiceGroupId);
        Task MoveALabTestToTheEndOfTheQueue(long labTestId, long clinicId);
        Task MoveALabTestToTheBeginningOfTheQueue(long labTestId, long clinicId);
    }
}
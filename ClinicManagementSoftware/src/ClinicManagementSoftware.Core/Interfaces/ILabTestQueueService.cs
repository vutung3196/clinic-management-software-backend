using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface ILabTestQueueService
    {
        Task EnqueueNewLabTests(long[] labTestIds, long clinicId);
        Task<long> MoveAFirstPatientToTheEndOfTheQueue(long clinicId);
        Task<Queue<long>> GetCurrentLabTestQueue(long clinicId);
        Task DeleteALabTestInQueue(long labTestId, long clinicId);
        Task CreateNewLabTestQueue(long clinicId);
        Task MoveALabTestToTheEndOfTheQueue(long labTestId, long clinicId);
        Task MoveALabTestToTheBeginningOfTheQueue(long labTestId, long clinicId);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IDoctorQueueService
    {
        Task EnqueueNewPatient(long visitingFormId, long doctorId);
        Task<Queue<long>> GetCurrentDoctorQueue(long doctorId);
        Task<IEnumerable<VisitingDoctorQueue>> GetAllDoctorQueues(long clinicId);
    }
}
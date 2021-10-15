using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Newtonsoft.Json;

namespace ClinicManagementSoftware.Core.Services
{
    public class LabTestQueueService : ILabTestQueueService
    {
        // lab test queue chỉ chứa các lab tests có trạng thái đang chờ đến lượt xét nghiệm
        private readonly IRepository<LabTestQueue> _labTestQueueRepository;

        public LabTestQueueService(IRepository<LabTestQueue> labTestQueueRepository)
        {
            _labTestQueueRepository = labTestQueueRepository;
        }

        public async Task EnqueueNewLabTests(long[] labTestIds, long clinicId)
        {
            var @spec = new GetLabTestQueueByClinicIdSpec(clinicId);
            var currentDoctorQueue = await _labTestQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with clinic id: {clinicId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            foreach (var labTestId in labTestIds)
            {
                currentQueue.Data.Enqueue(labTestId);
            }

            currentDoctorQueue.UpdatedAt = DateTime.UtcNow;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _labTestQueueRepository.UpdateAsync(currentDoctorQueue);
        }

        public async Task<long> MoveAFirstPatientToTheEndOfTheQueue(long clinicId)
        {
            var @spec = new GetLabTestQueueByClinicIdSpec(clinicId);
            var currentDoctorQueue = await _labTestQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {clinicId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            var currentVisitingFormId = currentQueue.Data.Dequeue();
            currentQueue.Data.Enqueue(currentVisitingFormId);
            currentDoctorQueue.UpdatedAt = DateTime.UtcNow;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _labTestQueueRepository.UpdateAsync(currentDoctorQueue);
            return currentVisitingFormId;
        }

        public async Task<Queue<long>> GetCurrentLabTestQueue(long clinicId)
        {
            var @spec = new GetLabTestQueueByClinicIdSpec(clinicId);
            var currentDoctorQueue = await _labTestQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with clinic id: {clinicId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            return currentQueue.Data;
        }

        public async Task DeleteALabTestInQueue(long labTestId, long clinicId)
        {
            var @spec = new GetLabTestQueueByClinicIdSpec(clinicId);
            var testQueue = await _labTestQueueRepository.GetBySpecAsync(@spec);
            if (testQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {clinicId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(testQueue.Queue);
            var newQueue = new Queue<long>();
            foreach (var id in currentQueue.Data.Where(id => labTestId != id))
            {
                newQueue.Enqueue(id);
            }

            currentQueue.Data = newQueue;
            testQueue.UpdatedAt = DateTime.UtcNow;
            testQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _labTestQueueRepository.UpdateAsync(testQueue);
        }

        public async Task CreateNewLabTestQueue(long clinicId)
        {
            var visitingDoctorQueueData = new QueueData
            {
                Data = new Queue<long>(),
            };
            var labTestQueue = new LabTestQueue
            {
                ClinicId = clinicId,
                CreatedAt = DateTime.UtcNow,
                Queue = JsonConvert.SerializeObject(visitingDoctorQueueData)
            };

            await _labTestQueueRepository.AddAsync(labTestQueue);
        }

        public async Task MoveALabTestToTheEndOfTheQueue(long labTestId, long clinicId)
        {
            var @spec = new GetLabTestQueueByClinicIdSpec(clinicId);
            var currentDoctorQueue = await _labTestQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {clinicId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            var newQueue = new Queue<long>();
            foreach (var element in currentQueue.Data.Where(element => element != labTestId))
            {
                newQueue.Enqueue(element);
            }

            newQueue.Enqueue(labTestId);
            currentQueue.Data = newQueue;
            currentDoctorQueue.UpdatedAt = DateTime.UtcNow;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _labTestQueueRepository.UpdateAsync(currentDoctorQueue);
        }

        public async Task MoveALabTestToTheBeginningOfTheQueue(long labTestId, long clinicId)
        {
            var @spec = new GetLabTestQueueByClinicIdSpec(clinicId);
            var currentDoctorQueue = await _labTestQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {clinicId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            var newQueue = new Queue<long>();
            newQueue.Enqueue(labTestId);
            foreach (var element in currentQueue.Data.Where(element => element != labTestId))
            {
                newQueue.Enqueue(element);
            }

            currentQueue.Data = newQueue;
            currentDoctorQueue.UpdatedAt = DateTime.UtcNow;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _labTestQueueRepository.UpdateAsync(currentDoctorQueue);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.PatientDoctorVisitingForm;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Newtonsoft.Json;

namespace ClinicManagementSoftware.Core.Services
{
    public class DoctorQueueService : IDoctorQueueService
    {
        private readonly IRepository<VisitingDoctorQueue> _visitingDoctorQueueRepository;

        public DoctorQueueService(IRepository<VisitingDoctorQueue> visitingDoctorQueueRepository)
        {
            _visitingDoctorQueueRepository = visitingDoctorQueueRepository;
        }

        public async Task EnqueueNewPatient(long visitingFormId, long doctorId)
        {
            var @spec = new GetDoctorQueueByDoctorIdSpec(doctorId);
            var currentDoctorQueue = await _visitingDoctorQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {doctorId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            currentQueue.Data.Enqueue(visitingFormId);
            currentDoctorQueue.UpdatedAt = DateTime.Now;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _visitingDoctorQueueRepository.UpdateAsync(currentDoctorQueue);
        }

        public async Task<long> MoveAFirstPatientToTheEndOfTheQueue(long doctorId)
        {
            var @spec = new GetDoctorQueueByDoctorIdSpec(doctorId);
            var currentDoctorQueue = await _visitingDoctorQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {doctorId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            var currentVisitingFormId = currentQueue.Data.Dequeue();
            currentQueue.Data.Enqueue(currentVisitingFormId);
            currentDoctorQueue.UpdatedAt = DateTime.Now;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _visitingDoctorQueueRepository.UpdateAsync(currentDoctorQueue);
            return currentVisitingFormId;
        }

        public async Task MoveAVisitingFormToTheEndOfTheQueue(long visitingFormId, long doctorId)
        {
            var @spec = new GetDoctorQueueByDoctorIdSpec(doctorId);
            var currentDoctorQueue = await _visitingDoctorQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {doctorId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            var newQueue = new Queue<long>();
            foreach (var element in currentQueue.Data.Where(element => element != visitingFormId))
            {
                newQueue.Enqueue(element);
            }

            newQueue.Enqueue(visitingFormId);
            currentQueue.Data = newQueue;
            currentDoctorQueue.UpdatedAt = DateTime.Now;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _visitingDoctorQueueRepository.UpdateAsync(currentDoctorQueue);
        }

        public async Task<Queue<long>> GetCurrentDoctorQueue(long doctorId)
        {
            var @spec = new GetDoctorQueueByDoctorIdSpec(doctorId);
            var currentDoctorQueue = await _visitingDoctorQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {doctorId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            return currentQueue.Data;
        }

        public async Task<IEnumerable<VisitingDoctorQueue>> GetAllDoctorQueues(long clinicId)
        {
            var spec = new GetAllDoctorQueuesByClinicIdSpec(clinicId);
            var result = await _visitingDoctorQueueRepository.ListAsync(spec);
            return result;
        }

        public async Task DeleteAVisitingFormInDoctorQueue(long visitingFormId, long doctorId)
        {
            var @spec = new GetDoctorQueueByDoctorIdSpec(doctorId);
            var currentDoctorQueue = await _visitingDoctorQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {doctorId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            if (!currentQueue.Data.Contains(visitingFormId))
            {
                return;
            }

            var newQueue = new Queue<long>();
            foreach (var id in currentQueue.Data.Where(id => visitingFormId != id))
            {
                newQueue.Enqueue(id);
            }

            currentQueue.Data = newQueue;
            currentDoctorQueue.UpdatedAt = DateTime.Now;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _visitingDoctorQueueRepository.UpdateAsync(currentDoctorQueue);
        }

        public async Task CreateNewDoctorQueue(long userId)
        {
            var visitingDoctorQueueData = new QueueData()
            {
                Data = new Queue<long>(),
            };
            var doctorQueue = new VisitingDoctorQueue()
            {
                DoctorId = userId,
                CreatedAt = DateTime.Now,
                Queue = JsonConvert.SerializeObject(visitingDoctorQueueData),
                IsDeleted = (byte) EnumIsDeleted.No
            };

            await _visitingDoctorQueueRepository.AddAsync(doctorQueue);
        }

        public async Task MoveAVisitingFormToTheBeginningOfTheQueue(long visitingFormId, long doctorId)
        {
            var @spec = new GetDoctorQueueByDoctorIdSpec(doctorId);
            var currentDoctorQueue = await _visitingDoctorQueueRepository.GetBySpecAsync(@spec);
            if (currentDoctorQueue == null)
            {
                throw new ArgumentException($"Cannot find current queue with {doctorId}");
            }

            var currentQueue = JsonConvert.DeserializeObject<QueueData>(currentDoctorQueue.Queue);
            var newQueue = new Queue<long>();
            newQueue.Enqueue(visitingFormId);
            foreach (var element in currentQueue.Data.Where(element => element != visitingFormId))
            {
                newQueue.Enqueue(element);
            }

            currentQueue.Data = newQueue;
            currentDoctorQueue.UpdatedAt = DateTime.Now;
            currentDoctorQueue.Queue = JsonConvert.SerializeObject(currentQueue);
            await _visitingDoctorQueueRepository.UpdateAsync(currentDoctorQueue);
        }
    }
}
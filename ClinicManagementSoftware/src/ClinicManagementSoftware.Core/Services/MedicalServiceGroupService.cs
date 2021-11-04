using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicalServiceGroupService : IMedicalServiceGroupService
    {
        private readonly IUserContext _userContext;
        private readonly IRepository<MedicalServiceGroup> _medicalServiceGroupRepository;
        private readonly ILabTestQueueService _labTestQueueService;

        public MedicalServiceGroupService(IUserContext userContext,
            IRepository<MedicalServiceGroup> medicalServiceGroupRepository,
            ILabTestQueueService labTestQueueService)
        {
            _userContext = userContext;
            _medicalServiceGroupRepository = medicalServiceGroupRepository;
            _labTestQueueService = labTestQueueService;
        }

        public async Task<IEnumerable<MedicalServiceGroupResponseDto>> GetAllMedicalServiceGroups()
        {
            var currentUser = await _userContext.GetCurrentContext();
            var @spec = new GetAllMedicalServiceGroupsByClinicIdSpec(currentUser.ClinicId);
            var medicalGroups = await _medicalServiceGroupRepository.ListAsync(@spec);
            return medicalGroups.OrderByDescending(x => x.CreatedAt).Select(x =>
                new MedicalServiceGroupResponseDto(x.Id, x.Name, x.Description, x.CreatedAt.Format()));
        }

        public async Task<MedicalServiceGroupResponseDto> CreateMedicalServiceGroup(
            MedicalServiceGroupResponseDto request)
        {
            var currentUser = await _userContext.GetCurrentContext();

            var medicalServiceGroup = new MedicalServiceGroup
            {
                CreatedAt = DateTime.Now,
                Description = request.Description,
                Name = request.Name,
                ClinicId = currentUser.ClinicId,
                IsDeleted = false
            };

            medicalServiceGroup = await _medicalServiceGroupRepository.AddAsync(medicalServiceGroup);

            // create a new queue here please
            await _labTestQueueService.CreateNewLabTestQueue(currentUser.ClinicId, medicalServiceGroup.Id);

            return new MedicalServiceGroupResponseDto(medicalServiceGroup.Id, medicalServiceGroup.Name,
                medicalServiceGroup.Description, medicalServiceGroup.CreatedAt.Format());
        }

        public async Task<MedicalServiceGroupResponseDto> EditMedicalServiceGroup(long id,
            MedicalServiceGroupResponseDto request)
        {
            var medicalServiceGroup = await _medicalServiceGroupRepository.GetByIdAsync(id);
            if (medicalServiceGroup == null)
            {
                throw new ArgumentException($"Cannot find service group with id: {id}");
            }

            medicalServiceGroup.UpdatedAt = DateTime.Now;
            medicalServiceGroup.Description = request.Description;
            medicalServiceGroup.Name = request.Name;

            await _medicalServiceGroupRepository.UpdateAsync(medicalServiceGroup);
            return new MedicalServiceGroupResponseDto(medicalServiceGroup.Id, medicalServiceGroup.Name,
                medicalServiceGroup.Description,
                medicalServiceGroup.CreatedAt.Format());
        }

        public async Task DeleteMedicalServiceGroup(long id)
        {
            var medicalServiceGroup = await _medicalServiceGroupRepository.GetByIdAsync(id);

            if (medicalServiceGroup == null)
            {
                throw new ArgumentException($"Cannot find medication service group with id: {id}");
            }

            medicalServiceGroup.IsDeleted = true;

            await _medicalServiceGroupRepository.UpdateAsync(medicalServiceGroup);
        }
    }
}
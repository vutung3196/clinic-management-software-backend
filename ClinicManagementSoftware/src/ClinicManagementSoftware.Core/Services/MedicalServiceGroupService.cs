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
        private readonly IRepository<MedicalService> _medicalServiceRepository;
        private readonly IRepository<LabTest> _labTestRepository;
        private readonly ILabTestQueueService _labTestQueueService;


        public MedicalServiceGroupService(IUserContext userContext,
            IRepository<MedicalServiceGroup> medicalServiceGroupRepository,
            ILabTestQueueService labTestQueueService, IRepository<LabTest> labTestRepository,
            IRepository<MedicalService> medicalServiceRepository)
        {
            _userContext = userContext;
            _medicalServiceGroupRepository = medicalServiceGroupRepository;
            _labTestQueueService = labTestQueueService;
            _labTestRepository = labTestRepository;
            _medicalServiceRepository = medicalServiceRepository;
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
            var medicalServiceGroup =
                await _medicalServiceGroupRepository.GetBySpecAsync(
                    new GetMedicalServiceGroupAndMedicalServicesById(id));

            var labTest = await _labTestRepository.GetBySpecAsync(
                new GetLabTestHavingDeletingMedicalServiceGroupSpec(id));
            if (labTest != null)
            {
                throw new ArgumentException("Không thể xóa nhóm chỉ định này, do đang có xét nghiệm chưa thanh toán");
            }

            if (medicalServiceGroup == null)
            {
                throw new ArgumentException($"Cannot find medication service group with id: {id}");
            }

            if (medicalServiceGroup.MedicalServices.Any(x => x.IsVisitingDoctorService))
            {
                throw new ArgumentException("Không thể xóa nhóm dịch vụ thu tiền khám bệnh ban đầu");
            }

            foreach (var medicalService in medicalServiceGroup.MedicalServices)
            {
                medicalService.IsDeleted = true;
                await _medicalServiceRepository.UpdateAsync(medicalService);
            }

            medicalServiceGroup.IsDeleted = true;

            await _medicalServiceGroupRepository.UpdateAsync(medicalServiceGroup);
        }
    }
}
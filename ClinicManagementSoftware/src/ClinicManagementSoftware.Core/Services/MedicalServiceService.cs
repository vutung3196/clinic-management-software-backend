using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicationService;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicalServiceService : IMedicalServiceService
    {
        private readonly IUserContext _userContext;
        private readonly IRepository<MedicalServiceGroup> _medicalServiceGroupSpecification;
        private readonly IRepository<MedicalService> _medicalServiceSpecification;


        public MedicalServiceService(IUserContext userContext,
            IRepository<MedicalServiceGroup> medicalServiceGroupSpecification,
            IRepository<MedicalService> medicalServiceSpecification)
        {
            _userContext = userContext;
            _medicalServiceGroupSpecification = medicalServiceGroupSpecification;
            _medicalServiceSpecification = medicalServiceSpecification;
        }


        public async Task<IEnumerable<MedicalServiceDto>> GetAllMedicalServices()
        {
            var currentUser = await _userContext.GetCurrentContext();
            var @spec = new GetAllMedicalServiceGroupByClinicIdSpec(currentUser.ClinicId);
            var medicalGroups = await _medicalServiceGroupSpecification.ListAsync(@spec);
            var result = medicalGroups.SelectMany(x => x.MedicalServices).OrderByDescending(x => x.CreatedAt);
            return result.Select(x =>
                new MedicalServiceDto(x.Id, x.Name, x.Description, x.Price, x.MedicalServiceGroup.Name,
                    x.MedicalServiceGroupId, x.CreatedAt));
        }

        public async Task<MedicalServiceDto> CreateMedicalService(MedicalServiceDto request)
        {
            var medicalServiceGroup = await _medicalServiceGroupSpecification.GetByIdAsync(request.GroupId);
            if (medicalServiceGroup == null)
            {
                throw new ArgumentException($"Cannot find service group with id: {request.GroupId}");
            }

            var medicalService = new MedicalService
            {
                CreatedAt = DateTime.UtcNow,
                Description = request.Description,
                MedicalServiceGroupId = request.GroupId,
                Name = request.Name,
                Price = request.Price,
            };

            medicalService = await _medicalServiceSpecification.AddAsync(medicalService);
            return new MedicalServiceDto(medicalService.Id, medicalService.Name,
                medicalService.Description, medicalService.Price, medicalServiceGroup.Name, medicalServiceGroup.Id,
                medicalService.CreatedAt);
        }

        public async Task<MedicalServiceDto> EditMedicalService(long id, MedicalServiceDto request)
        {
            var medicalServiceGroup = await _medicalServiceGroupSpecification.GetByIdAsync(request.GroupId);
            if (medicalServiceGroup == null)
            {
                throw new ArgumentException($"Cannot find service group with id: {request.GroupId}");
            }

            var medicalService = await _medicalServiceSpecification.GetByIdAsync(id);

            if (medicalService == null)
            {
                throw new ArgumentException($"Cannot find medication service with id: {id}");
            }

            medicalService.UpdatedAt = DateTime.UtcNow;
            medicalService.Description = request.Description;
            medicalService.MedicalServiceGroupId = request.GroupId;
            medicalService.Name = request.Name;
            medicalService.Price = request.Price;

            await _medicalServiceSpecification.UpdateAsync(medicalService);
            return new MedicalServiceDto(medicalService.Id, medicalService.Name,
                medicalService.Description, medicalService.Price, medicalServiceGroup.Name, medicalServiceGroup.Id,
                medicalService.CreatedAt);
        }

        public async Task DeleteMedicalService(long id)
        {
            var medicalService = await _medicalServiceSpecification.GetByIdAsync(id);

            if (medicalService == null)
            {
                throw new ArgumentException($"Cannot find medication service with id: {id}");
            }

            await _medicalServiceSpecification.DeleteAsync(medicalService);
        }
    }
}
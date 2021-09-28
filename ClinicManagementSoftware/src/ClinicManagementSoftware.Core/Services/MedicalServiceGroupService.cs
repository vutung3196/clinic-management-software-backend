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
        private readonly IRepository<MedicalServiceGroup> _medicalServiceGroupSpecification;
        private readonly IRepository<MedicalService> _medicalServiceSpecification;


        public MedicalServiceGroupService(IUserContext userContext,
            IRepository<MedicalServiceGroup> medicalServiceGroupSpecification,
            IRepository<MedicalService> medicalServiceSpecification)
        {
            _userContext = userContext;
            _medicalServiceGroupSpecification = medicalServiceGroupSpecification;
            _medicalServiceSpecification = medicalServiceSpecification;
        }


        public async Task<IEnumerable<MedicalServiceGroupDto>> GetAllMedicalServiceGroups()
        {
            var currentUser = await _userContext.GetCurrentContext();
            var @spec = new GetAllMedicalServiceGroupByClinicIdSpec(currentUser.ClinicId);
            var medicalGroups = await _medicalServiceGroupSpecification.ListAsync(@spec);
            var result = medicalGroups.SelectMany(x => x.MedicalServices).OrderByDescending(x => x.CreatedAt);
            return result.Select(x =>
                new MedicalServiceGroupDto(x.Id, x.Name, x.Description, x.CreatedAt.Format()));
        }

        public async Task<MedicalServiceGroupDto> CreateMedicalServiceGroup(MedicalServiceGroupDto request)
        {
            var medicalServiceGroup = new MedicalServiceGroup
            {
                CreatedAt = DateTime.UtcNow,
                Description = request.Description,
                Name = request.Name,
            };

            medicalServiceGroup = await _medicalServiceGroupSpecification.AddAsync(medicalServiceGroup);
            return new MedicalServiceGroupDto(medicalServiceGroup.Id, medicalServiceGroup.Name,
                medicalServiceGroup.Description, medicalServiceGroup.CreatedAt.Format());
        }

        public async Task<MedicalServiceGroupDto> EditMedicalServiceGroup(long id, MedicalServiceGroupDto request)
        {
            var medicalServiceGroup = await _medicalServiceGroupSpecification.GetByIdAsync(id);
            if (medicalServiceGroup == null)
            {
                throw new ArgumentException($"Cannot find service group with id: {id}");
            }

            medicalServiceGroup.UpdatedAt = DateTime.UtcNow;
            medicalServiceGroup.Description = request.Description;
            medicalServiceGroup.Name = request.Name;

            await _medicalServiceGroupSpecification.UpdateAsync(medicalServiceGroup);
            return new MedicalServiceGroupDto(medicalServiceGroup.Id, medicalServiceGroup.Name,
                medicalServiceGroup.Description,
                medicalServiceGroup.CreatedAt.Format());
        }

        public async Task DeleteMedicalServiceGroup(long id)
        {
            var medicalService = await _medicalServiceGroupSpecification.GetByIdAsync(id);

            if (medicalService == null)
            {
                throw new ArgumentException($"Cannot find medication service group with id: {id}");
            }

            await _medicalServiceGroupSpecification.DeleteAsync(medicalService);
        }
    }
}
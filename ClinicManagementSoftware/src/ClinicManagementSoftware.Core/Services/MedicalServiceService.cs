using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.MedicalService;
using ClinicManagementSoftware.Core.Dto.Receipt;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Exceptions.MedicalService;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class MedicalServiceService : IMedicalServiceService
    {
        private readonly IUserContext _userContext;
        private readonly IRepository<MedicalServiceGroup> _medicalServiceGroupSpecification;
        private readonly IRepository<MedicalService> _medicalServiceRepository;


        public MedicalServiceService(IUserContext userContext,
            IRepository<MedicalServiceGroup> medicalServiceGroupSpecification,
            IRepository<MedicalService> medicalServiceRepository)
        {
            _userContext = userContext;
            _medicalServiceGroupSpecification = medicalServiceGroupSpecification;
            _medicalServiceRepository = medicalServiceRepository;
        }


        public async Task<ReceiptMedicalServiceDto> GetDoctorVisitingFormMedicalService()
        {
            var currentContext = await _userContext.GetCurrentContext();
            var @spec = new GetPatientDoctorVisitingMedicalServiceSpec(currentContext.ClinicId);
            var medicalService = await _medicalServiceRepository.GetBySpecAsync(@spec);
            if (medicalService == null)
            {
                throw new VisitingDoctorMedicalServiceNotFoundException(
                    "Visiting doctor medical service is not created");
            }

            return new ReceiptMedicalServiceDto
            {
                BasePrice = medicalService.Price,
                Name = medicalService.Name,
                Quantity = 1,
            };
        }

        public async Task<IEnumerable<MedicalServiceDto>> GetAllMedicalServices()
        {
            var currentUser = await _userContext.GetCurrentContext();
            var @spec = new GetAllMedicalServicesByClinicIdSpec(currentUser.ClinicId);
            var medicalGroups = await _medicalServiceRepository.ListAsync(@spec);
            var result = medicalGroups.OrderByDescending(x => x.CreatedAt);
            return result.Select(x =>
                new MedicalServiceDto(x.Id, x.Name, x.Description, x.Price, x.MedicalServiceGroup.Name,
                    x.MedicalServiceGroupId, x.CreatedAt));
        }

        public async Task<IEnumerable<MedicalServiceGroupDto>> GetAllMedicalServiceByGroup()
        {
            var currentUser = await _userContext.GetCurrentContext();
            var @spec = new GetAllMedicalServicesByClinicIdSpec(currentUser.ClinicId);
            var medicalGroups = await _medicalServiceRepository.ListAsync(@spec);
            var result = medicalGroups.GroupBy(x => x.MedicalServiceGroup)
                .Select(x => new MedicalServiceGroupDto()
                {
                    GroupName = x.Key.Name,
                    MedicalServices = x.Select(medicalService => new MedicalServiceForLabTest
                    {
                        Description = "",
                        Name = medicalService.Name,
                        Id = medicalService.Id,
                        Quantity = 1
                    }),
                });
            return result;
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

            medicalService = await _medicalServiceRepository.AddAsync(medicalService);
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

            var medicalService = await _medicalServiceRepository.GetByIdAsync(id);

            if (medicalService == null)
            {
                throw new ArgumentException($"Cannot find medication service with id: {id}");
            }

            medicalService.UpdatedAt = DateTime.UtcNow;
            medicalService.Description = request.Description;
            medicalService.MedicalServiceGroupId = request.GroupId;
            medicalService.Name = request.Name;
            medicalService.Price = request.Price;

            await _medicalServiceRepository.UpdateAsync(medicalService);
            return new MedicalServiceDto(medicalService.Id, medicalService.Name,
                medicalService.Description, medicalService.Price, medicalServiceGroup.Name, medicalServiceGroup.Id,
                medicalService.CreatedAt);
        }

        public async Task DeleteMedicalService(long id)
        {
            var medicalService = await _medicalServiceRepository.GetByIdAsync(id);

            if (medicalService == null)
            {
                throw new ArgumentException($"Cannot find medication service with id: {id}");
            }

            await _medicalServiceRepository.DeleteAsync(medicalService);
        }
    }
}
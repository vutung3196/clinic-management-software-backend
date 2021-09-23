using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class ClinicManagementService : IClinicManagementService
    {
        private readonly IRepository<Clinic> _clinicSpecificationRepository;
        private readonly IUserService _userService;

        public ClinicManagementService(IRepository<Clinic> clinicSpecificationRepository,
            IUserService userService)
        {
            _clinicSpecificationRepository = clinicSpecificationRepository;
            _userService = userService;
        }

        public async Task UpdateClinicInformation(long id, CreateUpdateClinicRequestDto request)
        {
            var clinic = await _clinicSpecificationRepository.GetByIdAsync(id);
            if (clinic == null)
            {
                throw new ArgumentException($"Cannot find clinic with id {id}");
            }

            clinic.Address = request.Address;
            //clinic.EmailAddress = request.EmailAddress;
            clinic.PhoneNumber = request.PhoneNumber;
            clinic.Name = request.Name;
            clinic.IsEnabled = request.Enabled ? (byte) EnumEnabled.Active : (byte) EnumEnabled.InActive;
            await _clinicSpecificationRepository.UpdateAsync(clinic);
        }

        public async Task<ClinicInformationResponse> GetClinicInformation(long id)
        {
            var clinic = await _clinicSpecificationRepository.GetByIdAsync(id);
            if (clinic == null)
            {
                throw new ArgumentException($"Cannot find clinic with id {id}");
            }

            return new ClinicInformationResponse
            {
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber,
                Id = clinic.Id,
                Name = clinic.Name
            };
        }

        public async Task<ClinicInformationForAdminResponse> CreateClinic(CreateUpdateClinicRequestDto request)
        {
            var clinic = new Clinic
            {
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                IsEnabled = request.Enabled ? (byte) EnumEnabled.Active : (byte) EnumEnabled.InActive,
            };

            var isDuplicatedUser = await _userService.IsDuplicatedUser(request.UserName);
            if (isDuplicatedUser)
            {
                throw new ArgumentException("Username bị trùng");
            }

            clinic = await _clinicSpecificationRepository.AddAsync(clinic);

            // create new user too and send email
            var createUserDto = new CreateUserDto
            {
                Role = "Admin",
                FullName = request.Name,
                Password = request.Password,
                Enabled = true,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
            };

            await _userService.CreateUserWithClinic(createUserDto, clinic.Id);
            return new ClinicInformationForAdminResponse
            {
                Name = request.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber,
                Id = clinic.Id,
                Password = "",
                Username = request.UserName,
                CreatedAt = clinic.CreatedAt.Format(),
                Enabled = clinic.IsEnabled == (byte) EnumEnabled.Active,
            };
        }

        public async Task<IEnumerable<ClinicInformationForAdminResponse>> GetClinics()
        {
            // get clinic and admin user
            var spec = new GetClinicsAndAdminSpec();
            var clinics = await _clinicSpecificationRepository.ListAsync(spec);

            return clinics.Select(clinic => new ClinicInformationForAdminResponse
            {
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber,
                Id = clinic.Id,
                Name = clinic.Name,
                Username = clinic.Users
                    .FirstOrDefault(x => x.Role.RoleName == "Admin")
                    ?.Username ?? string.Empty,
                CreatedAt = clinic.CreatedAt.Format(),
                Enabled = clinic.IsEnabled == (byte) EnumEnabled.Active,
            });
        }
    }
}
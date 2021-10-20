using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Constants;
using ClinicManagementSoftware.Core.Dto.Clinic;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Helpers;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using SendGrid;

namespace ClinicManagementSoftware.Core.Services
{
    public class ClinicManagementService : IClinicManagementService
    {
        private readonly IRepository<Clinic> _clinicSpecificationRepository;
        private readonly ILabTestQueueService _labTestQueueService;
        private readonly IUserService _userService;
        private readonly ISendGridService _sendGridService;

        public ClinicManagementService(IRepository<Clinic> clinicSpecificationRepository,
            IUserService userService, ILabTestQueueService labTestQueueService, ISendGridService sendGridService)
        {
            _clinicSpecificationRepository = clinicSpecificationRepository;
            _userService = userService;
            _labTestQueueService = labTestQueueService;
            _sendGridService = sendGridService;
        }

        public async Task UpdateClinicInformation(long id, CreateUpdateClinicRequestDto request)
        {
            var clinic = await _clinicSpecificationRepository.GetByIdAsync(id);
            if (clinic == null)
            {
                throw new ArgumentException($"Cannot find clinic with id {id}");
            }

            if (clinic.FirstTimeRegistration.HasValue && clinic.FirstTimeRegistration.Value && request.Enabled)
            {
                // sending email back to the customer
                clinic.FirstTimeRegistration = false;
                var content =
                    "Tài khoản của bạn đã được phê duyệt, vui lòng đăng nhập tại localhost:3000";

                await _sendGridService.Send(content, "Phê duyệt tài khoản", MimeType.Text,
                    clinic.EmailAddress, "Clinic management software");
            }

            clinic.EmailAddress = request.EmailAddress;
            clinic.PhoneNumber = request.PhoneNumber;
            clinic.Name = request.Name;
            clinic.AddressDetail = request.AddressDetail;
            clinic.AddressCity = request.AddressCity;
            clinic.AddressDistrict = request.AddressDistrict;
            clinic.AddressStreet = request.AddressStreet;
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
                PhoneNumber = clinic.PhoneNumber,
                EmailAddress = clinic.EmailAddress,
                Id = clinic.Id,
                Name = clinic.Name,
                AddressCity = clinic.AddressCity,
                AddressDistrict = clinic.AddressDistrict,
                AddressStreet = clinic.AddressStreet,
                AddressDetail = clinic.AddressDetail,
            };
        }

        public async Task<ClinicInformationForAdminResponse> CreateClinic(CreateUpdateClinicRequestDto request)
        {
            var clinic = new Clinic
            {
                AddressDetail = request.AddressDetail,
                AddressCity = request.AddressCity,
                AddressDistrict = request.AddressDistrict,
                AddressStreet = request.AddressStreet,
                PhoneNumber = request.PhoneNumber,
                EmailAddress = request.EmailAddress,
                Name = request.Name,
                CreatedAt = DateTime.Now,
                FirstTimeRegistration = request.FirstTimeRegistration,
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

            // create new lab test queue
            await _labTestQueueService.CreateNewLabTestQueue(clinic.Id);

            // send email to system administrator to approve new clinic
            if (request.FirstTimeRegistration)
            {
                var content =
                    $"Phòng khám với tên {request.Name} vừa đăng ký, bạn vui lòng phê duyệt tài khoản tại trang quản lý";
                await _sendGridService.Send(content, "Phê duyệt tài khoản", MimeType.Text,
                    ConfigurationConstant.SystemAdminEmail, "Clinic management software");
            }

            return new ClinicInformationForAdminResponse
            {
                Name = request.Name,
                PhoneNumber = clinic.PhoneNumber,
                Id = clinic.Id,
                Password = "",
                Username = request.UserName,
                CreatedAt = clinic.CreatedAt.Format(),
                Enabled = clinic.IsEnabled == (byte) EnumEnabled.Active,
                AddressDetail = clinic.AddressDetail,
                AddressCity = clinic.AddressCity,
                AddressDistrict = clinic.AddressDistrict,
                AddressStreet = clinic.AddressStreet,
            };
        }

        public async Task<IEnumerable<ClinicInformationForAdminResponse>> GetClinics()
        {
            // get clinic and admin user
            var spec = new GetClinicsAndAdminSpec();
            var clinics = await _clinicSpecificationRepository.ListAsync(spec);

            return clinics.Select(clinic => new ClinicInformationForAdminResponse
            {
                PhoneNumber = clinic.PhoneNumber,
                Id = clinic.Id,
                Name = clinic.Name,
                Username = clinic.Users
                    .FirstOrDefault(x => x.Role.RoleName == "Admin")
                    ?.Username ?? string.Empty,
                CreatedAt = clinic.CreatedAt.Format(),
                Enabled = clinic.IsEnabled == (byte) EnumEnabled.Active,
                EmailAddress = clinic.EmailAddress,
                AddressDetail = clinic.AddressDetail,
                AddressCity = clinic.AddressCity,
                AddressDistrict = clinic.AddressDistrict,
                AddressStreet = clinic.AddressStreet,
            });
        }
    }
}
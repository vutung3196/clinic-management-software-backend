using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Exceptions.Authentication;
using ClinicManagementSoftware.Core.Exceptions.Clinic;
using ClinicManagementSoftware.Core.Exceptions.User;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using SendGrid;

namespace ClinicManagementSoftware.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserContext _userContext;
        private readonly IDoctorQueueService _doctorQueueService;
        private readonly IMapper _mapper;
        private readonly ISendGridService _sendGridService;

        public UserService(IMapper mapper,
            IRepository<User> userRepository, IUserContext userContext,
            IRepository<Role> roleRepository, IDoctorQueueService doctorQueueService, ISendGridService sendGridService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userContext = userContext;
            _roleRepository = roleRepository;
            _doctorQueueService = doctorQueueService;
            _sendGridService = sendGridService;
        }

        public async Task<UserDto> CreateAsync(UserDto input)
        {
            var user = _mapper.Map<User>(input);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);
            user.Password = passwordHash;
            await _userRepository.AddAsync(user);
            return input;
        }

        public async Task<UserDto> LoginAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(userName);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(password);

            var @spec = new GetUserRoleAndClinicByUsernameSpec(userName);
            var user = await _userRepository.GetBySpecAsync(@spec);
            if (user == null)
                throw new UserNotFoundException("Invalid username or password");
            if (user.Enabled == (byte) EnumEnabled.InActive)
            {
                throw new UserInActiveException("This user is not active");
            }

            if (user.Clinic.IsEnabled == (byte) EnumEnabled.InActive)
            {
                throw new ClinicInActiveException("The clinic is not active");
            }

            if (!VerifyPasswordHash(password, user.Password))
                throw new InCorrectPasswordException("Invalid username or password");
            var userResult = _mapper.Map<UserDto>(user);
            return userResult;
        }

        public async Task<IEnumerable<UserResultResponse>> GetAllUsers()
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var currentClinicId = currentUserContext.ClinicId;
            var @spec = new GetUsersByClinicIdSpec(currentClinicId);
            var currentUsers = (await _userRepository.ListAsync(@spec)).OrderByDescending(x => x.CreatedAt);
            return currentUsers.Select(user => _mapper.Map<UserResultResponse>(user));
        }

        public async Task<UserResultResponse> CreateUser(CreateUserDto request)
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var @spec = new GetUserRoleAndClinicByUsernameSpec(request.UserName.Trim());
            var duplicatedUser = await _userRepository.GetBySpecAsync(@spec);
            if (duplicatedUser != null)
            {
                throw new ArgumentException("Username đã có người dùng");
            }

            var @roleSpec = new GetRoleByRoleNameSpec(request.Role.Trim());
            var role = await _roleRepository.GetBySpecAsync(@roleSpec);
            if (role == null)
            {
                throw new ArgumentException($"Cannot find a role having name: {request.Role}");
            }


            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                CreatedAt = DateTime.Now,
                Username = request.UserName.Trim(),
                ClinicId = currentUserContext.ClinicId,
                Enabled = request.Enabled ? (byte) EnumEnabled.Active : (byte) EnumEnabled.InActive,
                Password = passwordHash,
                EmailAddress = request.EmailAddress,
                FullName = request.FullName,
                RoleId = role.Id,
                MedicalServiceGroupForTestSpecialistId = request.MedicalServiceGroupForTestSpecialistId
            };
            user = await _userRepository.AddAsync(user);
            if (role.RoleName == "Doctor")
            {
                await _doctorQueueService.CreateNewDoctorQueue(user.Id);
            }

            var content =
                $"Tài khoản của bạn đã được tạo, với tên người dùng là {request.UserName} và mật khẩu là {request.Password}";

            await _sendGridService.Send(content, "Tạo mới tài khoản", MimeType.Text,
                request.EmailAddress, "Clinic management software");

            return _mapper.Map<UserResultResponse>(user);
        }

        public async Task<UserResultResponse> CreateUserWithClinic(CreateUserDto request, long clinicId)
        {
            var @spec = new GetUserRoleAndClinicByUsernameSpec(request.UserName.Trim());
            var duplicatedUser = await _userRepository.GetBySpecAsync(@spec);
            if (duplicatedUser != null)
            {
                throw new ArgumentException("Username should be unique");
            }

            var @roleSpec = new GetRoleByRoleNameSpec(request.Role.Trim());
            var role = await _roleRepository.GetBySpecAsync(@roleSpec);
            if (role == null)
            {
                throw new ArgumentException($"Cannot find a role having name: {request.Role}");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                CreatedAt = DateTime.Now,
                Username = request.UserName.Trim(),
                ClinicId = clinicId,
                Enabled = request.Enabled ? (byte) EnumEnabled.Active : (byte) EnumEnabled.InActive,
                Password = passwordHash,
                PhoneNumber = request.PhoneNumber,
                FullName = request.FullName,
                RoleId = role.Id,
            };
            user = await _userRepository.AddAsync(user);
            return _mapper.Map<UserResultResponse>(user);
        }

        public async Task<bool> IsDuplicatedUser(string username)
        {
            var @spec = new GetUserRoleAndClinicByUsernameSpec(username);
            var duplicatedUser = await _userRepository.GetBySpecAsync(@spec);
            return duplicatedUser != null;
        }

        public async Task<UserResultResponse> EditUser(long id, EditUserDto request)
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var @spec = new GetUserAndRoleByIdSpec(id);
            var user = await _userRepository.GetBySpecAsync(@spec);
            if (user == null)
            {
                throw new ArgumentException($"Cannot find user with id {id}");
            }

            if (currentUserContext.ClinicId != user.ClinicId)
            {
                throw new UnauthorizedAccessException(
                    $"This admin cannot have access to this user having id: {user.Id}");
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.Password = passwordHash;
            }

            user.FullName = request.FullName;
            //user.PhoneNumber = request.PhoneNumber;
            user.Enabled = request.Enabled ? (byte) EnumEnabled.Active : (byte) EnumEnabled.InActive;
            var @roleSpec = new GetRoleByRoleNameSpec(request.Role.Trim());
            var newRole = await _roleRepository.GetBySpecAsync(@roleSpec);
            if (newRole == null)
            {
                throw new ArgumentException($"Cannot find a role having name: {request.Role}");
            }

            // for doctor
            if (newRole.RoleName.Equals("Doctor") && !user.Role.RoleName.Equals("Doctor"))
            {
                await _doctorQueueService.CreateNewDoctorQueue(user.Id);
            }

            user.RoleId = newRole.Id;
            user.MedicalServiceGroupForTestSpecialistId = request.MedicalServiceGroupForTestSpecialistId;

            var content =
                $"Tài khoản của bạn đã được cập nhật, với tên người dùng là {user.Username} và mật khẩu là {request.Password}";

            await _sendGridService.Send(content, "Tạo mới tài khoản", MimeType.Text,
                request.EmailAddress, "Clinic management software");

            await _userRepository.UpdateAsync(user);


            return _mapper.Map<UserResultResponse>(user);
        }

        public async Task DeactivateUser(long id)
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var @spec = new GetUserAndRoleByIdSpec(id);
            var user = await _userRepository.GetBySpecAsync(@spec);
            if (user == null)
            {
                throw new ArgumentException($"Cannot find user with id {id}");
            }

            if (currentUserContext.ClinicId != user.ClinicId)
            {
                throw new UnauthorizedAccessException(
                    $"This admin cannot have access to this user having id: {user.Id}");
            }

            user.Enabled = (byte) EnumEnabled.InActive;
            await _userRepository.UpdateAsync(user);
        }

        private static bool VerifyPasswordHash(string loginPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(loginPassword, hashedPassword);
        }
    }
}
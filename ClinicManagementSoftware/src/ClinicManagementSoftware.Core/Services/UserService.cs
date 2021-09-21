using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Enum;
using ClinicManagementSoftware.Core.Exceptions.Authentication;
using ClinicManagementSoftware.Core.Exceptions.User;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userSpecificationRepository;
        private readonly IRepository<Role> _roleSpecificationRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper,
            IRepository<User> userSpecificationRepository, IUserContext userContext,
            IRepository<Role> roleSpecificationRepository)
        {
            _mapper = mapper;
            _userSpecificationRepository = userSpecificationRepository;
            _userContext = userContext;
            _roleSpecificationRepository = roleSpecificationRepository;
        }

        public async Task<UserDto> CreateAsync(UserDto input)
        {
            var user = _mapper.Map<User>(input);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);
            user.Password = passwordHash;
            await _userSpecificationRepository.AddAsync(user);
            return input;
        }

        public async Task<UserDto> LoginAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(userName);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(password);

            var @spec = new GetUserRoleAndClinicByUsernameSpec(userName);
            var user = await _userSpecificationRepository.GetBySpecAsync(@spec);
            if (user == null)
                throw new UserNotFoundException("Invalid username or password");
            if (user.IsEnabled == (byte) EnumEnabledUser.InActive)
            {
                throw new UserInActiveException("This user is not active");
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
            var currentUsers = await _userSpecificationRepository.ListAsync(@spec);
            return currentUsers.Select(user => _mapper.Map<UserResultResponse>(user));
        }

        public async Task<UserResultResponse> CreateUser(CreateUserDto input)
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var @spec = new GetUserRoleAndClinicByUsernameSpec(input.UserName.Trim());
            var duplicatedUser = await _userSpecificationRepository.GetBySpecAsync(@spec);
            if (duplicatedUser != null)
            {
                throw new ArgumentException("Username should be unique");
            }

            var @roleSpec = new GetRoleByRoleNameSpec(input.Role.Trim());
            var role = await _roleSpecificationRepository.GetBySpecAsync(@roleSpec);
            if (role == null)
            {
                throw new ArgumentException($"Cannot find a role having name: {input.Role}");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);
            var user = new User
            {
                CreatedAt = DateTime.UtcNow,
                Username = input.UserName.Trim(),
                ClinicId = currentUserContext.ClinicId,
                IsEnabled = input.Enabled ? (byte) EnumEnabledUser.Active : (byte) EnumEnabledUser.InActive,
                Password = passwordHash,
                PhoneNumber = input.PhoneNumber,
                FullName = input.FullName,
                RoleId = role.Id
            };
            user = await _userSpecificationRepository.AddAsync(user);
            return _mapper.Map<UserResultResponse>(user);
        }

        public async Task<UserResultResponse> EditUser(long id, EditUserDto input)
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var @spec = new GetUserAndRoleByIdSpec(id);
            var user = await _userSpecificationRepository.GetBySpecAsync(@spec);
            if (user == null)
            {
                throw new ArgumentException($"Cannot find user with id {id}");
            }

            if (currentUserContext.ClinicId != user.ClinicId)
            {
                throw new UnauthorizedAccessException(
                    $"This admin cannot have access to this user having id: {user.Id}");
            }

            if (!string.IsNullOrWhiteSpace(input.Password))
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);
                user.Password = passwordHash;
            }

            user.FullName = input.FullName;
            user.PhoneNumber = input.PhoneNumber;
            user.IsEnabled = input.Enabled ? (byte) EnumEnabledUser.Active : (byte) EnumEnabledUser.InActive;
            var @roleSpec = new GetRoleByRoleNameSpec(input.Role.Trim());
            var newRole = await _roleSpecificationRepository.GetBySpecAsync(@roleSpec);
            if (newRole == null)
            {
                throw new ArgumentException($"Cannot find a role having name: {input.Role}");
            }

            user.RoleId = newRole.Id;

            await _userSpecificationRepository.UpdateAsync(user);


            return _mapper.Map<UserResultResponse>(user);
        }

        public async Task DeleteUser(long id)
        {
            var currentUserContext = await _userContext.GetCurrentContext();
            var @spec = new GetUserAndRoleByIdSpec(id);
            var user = await _userSpecificationRepository.GetBySpecAsync(@spec);
            if (user == null)
            {
                throw new ArgumentException($"Cannot find user with id {id}");
            }

            if (currentUserContext.ClinicId != user.ClinicId)
            {
                throw new UnauthorizedAccessException(
                    $"This admin cannot have access to this user having id: {user.Id}");
            }


            await _userSpecificationRepository.DeleteAsync(user);
        }

        private static bool VerifyPasswordHash(string loginPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(loginPassword, hashedPassword);
        }
    }
}
using System;
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

namespace ClinicManagementSoftware.Core.Services
{
    public class TokenAuthenticationService : ITokenAuthenticationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public TokenAuthenticationService(IMapper mapper,
            IRepository<User> userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
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
            userResult.ClinicId = user.ClinicId;
            return userResult;
        }

        private static bool VerifyPasswordHash(string loginPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(loginPassword, hashedPassword);
        }
    }
}
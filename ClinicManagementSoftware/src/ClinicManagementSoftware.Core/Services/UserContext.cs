using System;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.User;
using ClinicManagementSoftware.Core.Entities;
using ClinicManagementSoftware.Core.Exceptions.User;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Specifications;
using ClinicManagementSoftware.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClinicManagementSoftware.Core.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRepository<User> _userRepository;

        public UserContext(IHttpContextAccessor contextAccessor, IRepository<User> userRepository)
        {
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
        }

        public async Task<CurrentUserContext> GetCurrentContext()
        {
            if (_contextAccessor.HttpContext.User.Identity == null)
                throw new UnauthorizedAccessException("Cannot get current user identity");
            var userName = _contextAccessor.HttpContext.User.Identity.Name;
            var @spec = new GetUserRoleAndClinicByUsernameSpec(userName);
            var currentUser = await _userRepository.GetBySpecAsync(@spec);
            if (currentUser == null)
                throw new UserNotFoundException($"Cannot find current user with username is: {userName}");
            var currentUserContext = new CurrentUserContext(currentUser.Id, currentUser.ClinicId,
                currentUser.Username, currentUser.Role);
            return currentUserContext;
        }

        public async Task<CurrentUserContext> GetUserContextByUserName(string username)
        {
            var @spec = new GetUserRoleAndClinicByUsernameSpec(username);
            var currentUser = await _userRepository.GetBySpecAsync(@spec);
            if (currentUser == null)
                throw new UserNotFoundException($"Cannot find current user with username is: {username}");
            var currentUserContext = new CurrentUserContext(currentUser.Id, currentUser.ClinicId,
                currentUser.Username, currentUser.Role);
            return currentUserContext;
        }
    }
}
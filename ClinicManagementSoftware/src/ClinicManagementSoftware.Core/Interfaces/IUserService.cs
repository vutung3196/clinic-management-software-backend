using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.User;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(UserDto input);
        Task<UserDto> LoginAsync(string userName, string password);
        Task<IEnumerable<UserResultResponse>> GetAllUsers();
        Task<UserResultResponse> CreateUser(CreateUserDto request);
        Task<UserResultResponse> EditUser(long id, EditUserDto request);
        Task DeactivateUser(long id);
        Task<UserResultResponse> CreateUserWithClinic(CreateUserDto request, long clinicId);
        Task<bool> IsDuplicatedUser(string username);
    }
}
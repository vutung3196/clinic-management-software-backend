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
        Task<UserResultResponse> CreateUser(CreateUserDto input);
        Task<UserResultResponse> EditUser(long id, EditUserDto input);
        Task DeactivateUser(long id);
        Task<UserResultResponse> CreateUserWithClinic(CreateUserDto input, long clinicId);
        Task<bool> IsDuplicatedUser(string username);
    }
}
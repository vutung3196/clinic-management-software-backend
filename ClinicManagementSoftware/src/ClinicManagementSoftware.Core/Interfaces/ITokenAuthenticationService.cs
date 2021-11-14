using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.User;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface ITokenAuthenticationService
    {
        Task<UserDto> LoginAsync(string userName, string password);
    }
}
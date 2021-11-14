using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Dto.User;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface IUserContext
    {
        Task<CurrentUserContext> GetCurrentContext();
    }
}
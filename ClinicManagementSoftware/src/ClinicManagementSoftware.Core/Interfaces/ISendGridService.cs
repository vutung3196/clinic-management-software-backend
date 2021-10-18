using System.Threading.Tasks;
using SendGrid;

namespace ClinicManagementSoftware.Core.Interfaces
{
    public interface ISendGridService
    {
        public Task<Response> Send(string content, string subject, string type, string to, string name);
    }
}

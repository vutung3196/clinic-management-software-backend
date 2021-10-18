using System.Threading.Tasks;
using ClinicManagementSoftware.Core.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ClinicManagementSoftware.Core.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly ISendGridClient _sendGridClient;

        public SendGridService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task<Response> Send(string content, string subject, string type, string to, string name)
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress("tungvusoftware@gmail.com", name),
                Subject = subject
            };
            msg.AddContent(type, content);
            msg.AddTo(new EmailAddress(to, name));
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            return response;
        }
    }
}
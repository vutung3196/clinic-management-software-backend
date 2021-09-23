using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Web.Authentication.Model
{
    public class AuthenticateModel
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}
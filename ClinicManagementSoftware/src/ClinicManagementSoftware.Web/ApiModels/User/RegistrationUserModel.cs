using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Web.ApiModels.User
{
    public class RegistrationUserModel
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }

        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Enabled { get; set; }
        public int ClinicId { get; set; }
        public string Role { get; set; }
    }
}
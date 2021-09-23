using System;
using ClinicManagementSoftware.Core.Entities;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public byte IsEnabled { get; set; }
        public long ClinicId { get; set; }
        public Role Role { get; set; }
    }
}
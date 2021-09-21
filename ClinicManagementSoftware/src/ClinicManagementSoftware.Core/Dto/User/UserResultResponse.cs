using System;
using System.Collections.Generic;
using System.Linq;
using ClinicManagementSoftware.Core.Enum;

namespace ClinicManagementSoftware.Core.Dto.User
{
    public class UserResultResponse
    {
        public string PhoneNumber { get; set; }
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public byte Enabled { get; set; }
        public bool IsEnabled => Enabled == (byte) EnumEnabledUser.Active;
        public long ClinicId { get; set; }
        public string Role { get; set; }
    }
}
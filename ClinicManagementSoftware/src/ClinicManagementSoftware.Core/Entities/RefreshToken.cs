using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("refresh_token")]
    public class RefreshToken : BaseEntity, IAggregateRoot
    {
        [Column("token_string")]
        public string TokenString { get; set; }

        [Column(name:"user_name")]
        public string UserName { get; set; }

        [Column(name: "expired_at")]
        public DateTime ExpiredAt { get; set; }
    }
}

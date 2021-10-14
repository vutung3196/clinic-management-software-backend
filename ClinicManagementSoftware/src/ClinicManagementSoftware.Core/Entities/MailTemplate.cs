using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("mail_template")]
    public class MailTemplate : BaseEntity, IAggregateRoot
    {
        [Column("name")] public string Name { get; set; }

        [Column("description")] public string Description { get; set; }

        [Column("template")] public string Template { get; set; }
    }
}
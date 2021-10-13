using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("disease")]
    public class Disease : BaseEntity, IAggregateRoot
    {
        [Column("disease_group_id")] public long DiseaseGroupId { get; set; }
        [Column("disease_group")] public string DiseaseGroup { get; set; }

        [Column("description")] public string Description { get; set; }
    }
}
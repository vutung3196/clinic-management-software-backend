using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("medication")]
    public class Medication : BaseEntity, IAggregateRoot
    {
        [Column("medication_group_id")] public long MedicationGroupId { get; set; }
        public MedicationGroup MedicationGroup { get; set; }
        [Column("medication_name")] public string Name { get; set; }
        [Column("description")] public string Description { get; set; }
        [Column("usage")] public string Usage { get; set; }
    }
}
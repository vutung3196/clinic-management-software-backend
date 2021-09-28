using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("medication_group")]
    public class MedicationGroup : BaseEntity, IAggregateRoot
    {
        [Column("group_name")] public string Name { get; set; }

        [Column("description")] public string Description { get; set; }

        public ICollection<Medication> Medications { get; set; }
    }
}
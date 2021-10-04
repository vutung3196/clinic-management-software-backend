using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("lab_test")]
    public class LabTest : BaseEntity, IAggregateRoot
    {
        // foreign key
        [Column("lab_order_form_id")] public long LabOrderFormId { get; set; }
        public LabOrderForm LabOrderForm { get; set; }

        [Column("medical_service_id")] public long MedicalServiceId { get; set; }
        public MedicalService MedicalService { get; set; }

        [Column("status")] public byte Status { get; set; }
        [Column("result", TypeName = "json")] public string Result { get; set; }
        [Column("description")] public string Description { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("receipt")]
    public class Receipt : BaseEntity, IAggregateRoot
    {
        [Column("patient_id")] public long PatientId { get; set; }

        [Column("total")] public double Total { get; set; }

        [Column("services", TypeName = "json")]
        public string Services { get; set; }

        [Column("description")] public string Description { get; set; }


        [Column("code")] public string Code { get; set; }

        [Column("patient_doctor_visit_form_id")]
        public long? PatientDoctorVisitFormId { get; set; }

        public PatientDoctorVisitForm PatientDoctorVisitForm { get; set; }

        [Column("lab_order_form_id")] public long? LabOrderFormId { get; set; }

        public LabOrderForm LabOrderForm { get; set; }
        public Patient Patient { get; set; }
    }
}
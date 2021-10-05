using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("lab_order_form")]
    public class LabOrderForm : BaseEntity, IAggregateRoot
    {
        // foreign keys
        [Column("patient_hospitalized_profile_id")]
        public long PatientHospitalizedProfileId { get; set; }

        public PatientHospitalizedProfile PatientHospitalizedProfile { get; set; }

        [Column("patient_doctor_visit_form_id")]
        public long PatientDoctorVisitingFormId { get; set; }

        public PatientDoctorVisitForm PatientDoctorVisitForm { get; set; }
        [Column("doctor_id")] public long DoctorId { get; set; }
        public User Doctor { get; set; }
        [Column("code")] public string Code { get; set; }
        [Column("description")] public string Description { get; set; }

        [Column("status")] public byte Status { get; set; }

        public ICollection<LabTest> LabTests;
    }
}
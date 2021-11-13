using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("prescription")]
    public class Prescription : BaseEntity, IAggregateRoot
    {
        // foreign keys
        [Column("patient_hospitalized_profile_id")]
        public long PatientHospitalizedProfileId { get; set; }

        public PatientHospitalizedProfile PatientHospitalizedProfile { get; set; }

        [Column("patient_doctor_visit_form_id")] public long PatientDoctorVisitFormId { get; set; }

        public PatientDoctorVisitForm PatientDoctorVisitForm { get; set; }
        [Column("doctor_id")] public long DoctorId { get; set; }
        public User Doctor { get; set; }


        [Column("diagnosed_description")] public string DiagnosedDescription { get; set; }

        // Json
        [Column("medication_information", TypeName = "json")]
        public string MedicationInformation { get; set; }

        [Column("doctor_suggestion")] public string DoctorSuggestion { get; set; }
        [Column("revisit_date")] public DateTime? RevisitDate { get; set; }
        [Column("medical_insurance_code")] public string MedicalInsuranceCode { get; set; }
        [Column("code")] public string Code { get; set; }
        [Column("disease_note")] public string DiseaseNote { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("medical_service")]
    public class MedicalService : BaseEntity, IAggregateRoot
    {
        [Column("medical_service_group_id")] public long MedicalServiceGroupId { get; set; }
        public MedicalServiceGroup MedicalServiceGroup { get; set; }
        [Column("clinic_id")] public long ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("price")] public double Price { get; set; }
        [Column("description")] public string Description { get; set; }
        [Column("is_visiting_doctor_service")] public bool IsVisitingDoctorService { get; set; }
        public ICollection<LabTest> LabTests { get; set; }
    }
}
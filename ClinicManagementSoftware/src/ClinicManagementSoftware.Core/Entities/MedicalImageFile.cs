using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("medical_image_file")]
    public class MedicalImageFile : BaseEntity, IAggregateRoot
    {
        [Column("file_name")] public string FileName { get; set; }

        [Column("file_description")] public string Description { get; set; }

        [Column("file_path")] public string FilePath { get; set; }

        [Column("lab_test_id")] public long LabTestId { get; set; }
        public LabTest LabTest { get; set; }

        [Column("cloudinary_file_id")] public long? CloudinaryFileId { get; set; }
        public CloudinaryFile CloudinaryFile { get; set; }

        [Column("deleted_at")] public DateTime? DeletedAt { get; set; }
    }
}
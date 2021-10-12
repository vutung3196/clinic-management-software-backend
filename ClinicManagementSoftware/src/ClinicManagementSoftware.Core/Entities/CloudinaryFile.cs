using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementSoftware.SharedKernel;
using ClinicManagementSoftware.SharedKernel.Interfaces;

namespace ClinicManagementSoftware.Core.Entities
{
    [Table("cloudinary_file")]
    public class CloudinaryFile : BaseEntity, IAggregateRoot
    {
        [Column("public_id")] public string PublicId { get; set; }
        [Column("url")] public string Url { get; set; }
        [Column("secure_url")] public string SecureUrl { get; set; }
        [Column("bytes")] public long Bytes { get; set; }
        [Column("width")] public int Width { get; set; }
        [Column("height")] public int Height { get; set; }
        [Column("file_name")] public string FileName { get; set; }
        [Column("created_at")] public DateTime CreatedAt { get; set; }
        [Column("updated_at")] public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")] public DateTime? DeletedAt { get; set; }

        public MedicalImageFile MedicalImageFile { get; set; }
    }
}
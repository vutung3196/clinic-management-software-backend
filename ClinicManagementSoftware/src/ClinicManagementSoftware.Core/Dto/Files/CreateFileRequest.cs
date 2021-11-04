using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClinicManagementSoftware.Core.Dto.Cloudinary;

namespace ClinicManagementSoftware.Core.Dto.Files
{
    public class CreateFileRequest
    {
        [Required] public long LabTestId { get; set; }
        public IList<CloudinaryFieldDto> CloudinaryFiles { get; set; }
    }
}
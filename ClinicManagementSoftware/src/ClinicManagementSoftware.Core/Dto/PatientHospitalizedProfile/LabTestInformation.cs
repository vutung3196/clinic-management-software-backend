using System.Collections.Generic;
using ClinicManagementSoftware.Core.Dto.Files;

namespace ClinicManagementSoftware.Core.Dto.PatientHospitalizedProfile
{
    public class LabTestInformation
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public IEnumerable<ImageFileResponse> ImageFiles { get; set; }
        public double Price { get; set; }
    }
}
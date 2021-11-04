using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSoftware.Core.Dto.LabTest
{
    public class EditLabTestDto
    {
        [MaxLength(500, ErrorMessage = "Kết quả của phiếu xét nghiệm không được vượt quá 500 ký tự")]
        public string Result { get; set; }

        public byte Status { get; set; }
        public byte CurrentPageStatus { get; set; }
    }
}
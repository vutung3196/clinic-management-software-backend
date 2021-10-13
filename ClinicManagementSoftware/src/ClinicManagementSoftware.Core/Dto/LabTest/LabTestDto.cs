using System.Collections.Generic;

namespace ClinicManagementSoftware.Core.Dto.LabTest
{
    public class UpdateLabTestResponse
    {
        public bool IsLabOrderFormDone { get; set; }
        public bool IsMovingFromWaitingForTestingStatusToWaitingForResultStatus { get; set; }
        public bool IsMovingFromWaitingForResultStatusToDoneStatus { get; set; }
        public bool IsMovingFromWaitingForTestingStatusToDoneStatus { get; set; }
        public IEnumerable<LabTestDto> LabTests { get; set; }
    }
}
namespace ClinicManagementSoftware.Core.Enum
{
    public enum EnumLabTestStatus
    {
        // Chưa thanh toán
        NotPaid = 0,

        // đã thanh toán
        // đang chờ xét nghiệm
        WaitingForTesting = 1,


        // đang đi xét nghiệm
        WaitingForResult = 2,

        // xong
        Done = 3,
    }
}
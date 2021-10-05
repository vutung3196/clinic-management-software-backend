namespace ClinicManagementSoftware.Core.Enum
{
    public enum EnumLabOrderFormStatus
    {
        // Chưa thanh toán
        NotPaid = 0,

        // đã thanh toán
        Paid = 1,

        // đang chờ xét nghiệm
        WaitingForTesting = 2,


        // đang đi xét nghiệm
        HavingTesting = 3,

        // xong
        Done = 4,
    }
}
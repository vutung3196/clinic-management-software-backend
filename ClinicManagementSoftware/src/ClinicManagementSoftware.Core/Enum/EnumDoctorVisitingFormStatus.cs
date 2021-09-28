namespace ClinicManagementSoftware.Core.Enum
{
    public enum EnumDoctorVisitingFormStatus
    {
        // Chưa thanh toán
        NotPaid = 0,

        // đã thanh toán
        Paid = 1,

        // đang chờ khám
        WaitingForDoctor = 2,

        // đang khám
        VisitingDoctor = 3,
        
        // đang đi xét nghiệm
        HavingTesting = 4,

        // khám xong
        Done = 5,
    }
}
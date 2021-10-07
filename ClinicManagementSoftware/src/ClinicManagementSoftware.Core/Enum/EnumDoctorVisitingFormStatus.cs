namespace ClinicManagementSoftware.Core.Enum
{
    public enum EnumDoctorVisitingFormStatus
    {
        // đang chờ khám
        WaitingForDoctor = 1,

        // đang khám
        VisitingDoctor = 2,

        // đang đi xét nghiệm
        HavingTesting = 3,

        // khám xong
        Done = 4,
    }
}
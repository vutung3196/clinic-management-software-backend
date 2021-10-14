namespace ClinicManagementSoftware.Core.Dto.MedicalService
{
    public class MedicationForPrescriptionDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Usage { get; set; }
    }
}
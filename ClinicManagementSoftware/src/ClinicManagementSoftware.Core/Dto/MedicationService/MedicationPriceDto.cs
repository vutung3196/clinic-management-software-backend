namespace ClinicManagementSoftware.Core.Dto.MedicationService
{
    public class MedicationPriceDto
    {
        public MedicationPriceDto(long id, long clinicId, string name, string description, double? price, int? quantity)
        {
            Id = id;
            ClinicId = clinicId;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public long Id { get; set; }
        public long ClinicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public bool IsMedication => true;
        public double? TotalPrice => Quantity * Price;
    }
}
namespace ClinicManagementSoftware.Core.Dto.Medication
{
    public class MedicationDto
    {
        public MedicationDto(long id, string name, string description, string usage)
        {
            Id = id;
            Name = name;
            Description = description;
            Usage = usage;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
    }
}
namespace ClinicManagementSoftware.Core.Dto.Files
{
    public class ImageFileResponse
    {
        public long? Id { get; set; }
        public string PublicId { get; set; }

        // using for production
        public string FilePath { get; set; }
        public string Base64Image { get; set; }
        public string Url { get; set; }
        public string SecureUrl { get; set; }
        public string CreatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
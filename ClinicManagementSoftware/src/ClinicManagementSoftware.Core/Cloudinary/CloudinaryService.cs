using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ClinicManagementSoftware.Core.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryClient _cloudinaryClient;

        public CloudinaryService(CloudinaryClient cloudinaryClient)
        {
            _cloudinaryClient = cloudinaryClient;
        }

        public async Task<List<ImageUploadResult>> UploadImages(List<string> filePaths, string accessType,
            string folder)
        {
            var imageUploadResults = new List<ImageUploadResult>();
            var bytes = Convert.FromBase64String("aa");
            var contents = new StreamContent(new MemoryStream(bytes));
            var ahihi = await contents.ReadAsStreamAsync();
            foreach (var path in filePaths)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("aa", ahihi),
                    Type = accessType,
                    Folder = folder
                };
                var currentResult = await _cloudinaryClient.UploadAsync(uploadParams);
                imageUploadResults.Add(currentResult);
            }

            return imageUploadResults;
        }

        public async Task<ImageUploadResult> UploadImage(string filePath, string accessType, string folder)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(filePath),
                Type = accessType,
                Folder = folder,
                Transformation = new Transformation().Width(800).Height(800)
            };
            var result = await _cloudinaryClient.UploadAsync(uploadParams);
            return result;
        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            return await _cloudinaryClient.DeleteAsync(publicId);
        }
    }
}
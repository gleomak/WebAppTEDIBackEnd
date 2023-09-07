using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class ImageService
    {
        private readonly Cloudinary cloudinary;
        public ImageService(IConfiguration configuration)
        {
            //var account = new Account
            //    (

            //        configuration.GetSection("Cloudinary").GetSection("CloudName").Value,
            //        configuration.GetSection("Cloudinary").GetSection("ApiKey").Value,
            //        configuration.GetSection("Cloudinary").GetSection("ApiSecretKey").Value

            Account account = new Account(
                    "dcxqcdltr",
                    "289846393238217",
                    "ty3BxFPQMQx-kD_4v-EpnHvCzbc");

            cloudinary = new Cloudinary (account);
        }

        public async Task<ImageUploadResult> AddImageAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}

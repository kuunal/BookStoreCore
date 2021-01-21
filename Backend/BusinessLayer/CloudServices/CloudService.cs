using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CloudServices
{
    public class CloudService : ICloudService
    {
        private readonly Cloudinary _cloudinary;

        public CloudService(CloudConfiguration config)
        {
            Account account = new Account(config.Name
                                          , config.ApiKey
                                          , config.Secret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UpdloadToCloud(IFormFile image, string email)
        {
            var uploadResult = new ImageUploadResult();
            if (image != null && image.Length > 0)
            {
                using (var stream = image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(email + DateTime.Now.ToString(), stream)
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            return uploadResult.Url.ToString();
        }
    }
}

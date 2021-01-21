using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.CloudServices
{
    public interface ICloudService
    {
        Task<string> UpdloadToCloud(IFormFile image, string email);
    }
}

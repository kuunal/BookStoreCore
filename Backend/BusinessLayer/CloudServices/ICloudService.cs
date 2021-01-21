using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CloudServices
{
    public interface ICloudService
    {
        Task<string> UpdloadToCloud(IFormFile image, string email);
    }
}

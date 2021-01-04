using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Fundoo.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            string message = exception.Error.Message;
            int statusCode = exception.Error.GetType().Name switch
            {
                "BookstoreException" => Convert.ToInt32(exception.Error.ToString()),
                _ => (int)HttpStatusCode.InternalServerError
            };
            return new JsonResult(new { detail = message, statusCode = statusCode });
        }
    }
}
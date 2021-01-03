using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class UserController : ControllerBase
    {
        public IActionResult Login()
        {
            return Ok();
        }
    }
}

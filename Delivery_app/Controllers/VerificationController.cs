using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delivery_app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Delivery_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        [HttpPost]
        public void handleVerificationCode(VerificationCode verification_code)
        {
            Console.WriteLine("Verification Code is {0}", verification_code.value.ToString());
        }
    }
}

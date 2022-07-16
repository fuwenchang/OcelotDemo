using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_A.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        public HealthCheckController() { }

        [HttpGet("Status")]
        public string Status() 
        {
            return "Ok";
        }
    }
}

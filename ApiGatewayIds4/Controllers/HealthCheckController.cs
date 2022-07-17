using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayIds4.Controllers
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

using Api_A.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_A.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetApiA/{id}")]
        public string Get(int id)
        {
            return $"{this.HttpContext.Request.Host.Port},����Api_A";
        }

        [Authorize(Policy = "policy1")]
        [HttpGet("GetApiAJwtPolicy/{id}")]
        public string GetApiAJwt(int id)
        {
            var httpContext = this.HttpContext.Request;
            return "Jwt��Ȩ����";
        }

        [ServiceFilter(typeof(SimpleActionFilter))]
        [Authorize(Roles = "admin")]
        [HttpGet("GetApiAJwtRole/{id}")]
        public string GetApiAJwtRole(int id)
        {
            return "Jwt��ɫ��Ȩ����";
        }

        [HttpGet("GetApiANoAuth/{id}")]
        public string GetApiANoAuth(int id)
        {
            var context = this.HttpContext.Request;
            return "û����Ȩ";
        }
    }
}
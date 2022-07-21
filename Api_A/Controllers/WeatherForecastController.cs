using Api_A.Filter;

using Hys.AddActivityLog;
using Hys.AddActivityLog.Filter;

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
        public ApiResult<string> Get(int id)
        {
            int a = 0;
            int b = 1;
            int c = b / a;
            return CommonResult.Success<string>($"{this.HttpContext.Request.Host.Port},这是Api_A");
        }

        [Authorize(Policy = "policy1")]
        [HttpGet("GetApiAJwtPolicy/{id}")]
        public string GetApiAJwt(int id)
        {
            var httpContext = this.HttpContext.Request;
            return "Jwt授权策略";
        }

        [ServiceFilter(typeof(SimpleActionFilter))]
        [Authorize(Roles = "admin")]
        [HttpGet("GetApiAJwtRole/{id}")]
        public string GetApiAJwtRole(int id)
        {
            return "Jwt角色授权策略";
        }

        [HttpGet("GetApiANoAuth/{id}")]
        public string GetApiANoAuth(int id)
        {
            var context = this.HttpContext.Request;
            return "没有授权";
        }
    }
}
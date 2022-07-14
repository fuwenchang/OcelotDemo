using Microsoft.AspNetCore.Mvc;

namespace Api_B.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetApiB/{id}")]
        public string Get(int id)
        {
            var x = HttpContext.Request;
            return $"{HttpContext.Request.Host.Port},’‚ «Api_B";
        }
    }
}
using Entity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace Api_A.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HealthCheckController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //[HttpGet("Status")]
        //public string Status() 
        //{
        //    var client = _httpClientFactory.CreateClient();

        //    try
        //    {
        //        HttpContent content = new StringContent(JsonConvert.SerializeObject(new HealthCheck()));
        //        var response = client.PostAsync("http://localhost:9001/esbcallHealthCheck/", content).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return "Ok";
        //        }
        //        else 
        //        {
        //            throw new Exception() ;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpGet("Status")]
        public string Status()
        {
            return "Ok";
        }
    }
}

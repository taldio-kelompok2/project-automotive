using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebApi.controller
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from Home Controller!");
        }
    }
}
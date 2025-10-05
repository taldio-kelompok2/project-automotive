using Microsoft.AspNetCore.Mvc;

namespace AutomotiveApp.WebAPI.Controllers
{

    [ApiController]
    [Route("/no")]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Hello Swagger!" });
        }
    }
}
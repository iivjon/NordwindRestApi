using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NordwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return("Hello World");
        }
    }
}

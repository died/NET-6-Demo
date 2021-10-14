using Microsoft.AspNetCore.Mvc;

namespace Net6_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _log;
        public TestController(ILogger<TestController> log)
        {
            _log = log;
        }

        [HttpGet]
        public dynamic Index()
        {
            var ex = new NotImplementedException();
            _log.LogError(ex, "Error log test");
            return Ok();
        }
    }
}
